﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using DalApi;
using DO;

namespace BlImplementation;

internal class Cart:ICart
{
    readonly private static IDal DOList = DalApi.Factory.Get();//to access DO info 
    public BO.Cart AddToCart(BO.Cart myCart, int id)
    {
        int ind = myCart.orderItems.FindIndex(x => x!=null && x.ID == id); //save index of order with ID in cart
        DO.Product? product = new DO.Product?();//create a DO product
        product = DOList.Product.GetById(id);//get the matching product for the ID
        if (product?.InStock < 1 || product?.IsDeleted==true)
        {
            throw new BO.UnfoundException("Product unavailable");
        }
        if (ind != -1)//exists in cart
        {
            myCart.orderItems[ind].Amount++; //add another product to the cart
            myCart.orderItems[ind].Price += myCart.orderItems[ind].ProductPrice;//add to the total price 
            myCart.Price += myCart.orderItems[ind].Price;//update cart price
            return myCart;
        }
        BO.OrderItem oi = new BO.OrderItem//create new orderitem that is being added 
        {
            ID = id,
            ProductName = product.Value.Name,
            ProductPrice = (double)product.Value.Price,
            IsDeleted = false,
            Amount = 1,
            Price = (double)product.Value.Price,
            ProductID = product.Value.ID
        };
        myCart.orderItems.Add(oi);//add the orderitem to cart
        myCart.Price += oi.Price;//update price of the cart
        return myCart;

    }
    public BO.Cart UpdateCart(BO.Cart myCart, int id, int amount){


        int ind = myCart.orderItems.FindIndex(x => x.ProductID == id); //save index of product with ID in cart
        DO.Product product = new DO.Product();//create a DO product
        product = DOList.Product.GetById(id);//get the matching product for the ID
        //if (product.InStock + myCart.orderItems[ind].Amount - amount < 1) //if the new amount is impossible
        //{
        //    throw new BO.UnfoundException("Incorrect Amount");
        //}
        if (ind != -1)//if in cart
        {
            if (amount == 0)
            {
                BO.OrderItem temp= myCart.orderItems[ind];//save the orderitem with id
                myCart.orderItems.Remove(temp);//remove orderItem from cart
                myCart.Price -= myCart.orderItems[ind].Price;
                return myCart;
            }
            myCart.Price -= myCart.orderItems[ind].Price * myCart.orderItems[ind].Amount; //substract price of product from cart
            myCart.orderItems[ind].Amount = amount;//set new amount
            myCart.Price += myCart.orderItems[ind].Price * amount;//add the new price
            return myCart;
        }
        throw new BO.IdNotExistException();
    }
    public void MakeOrder(BO.Cart myCart, string CustomerName, string CustomerEmail, string CustomerAddress)
    {
        if (CustomerName == "" || CustomerEmail=="" || CustomerAddress=="")//check input
        {
            throw new BO.UnfoundException("Incorrect Input");
        }
        DO.OrderItem oi = new();//create order item
        foreach (BO.OrderItem? item in myCart.orderItems)//go over orderItems in the cart
        {
            if(item.ProductID==DOList.Product.GetById(item.ProductID).ID && item.Amount>0 && item.Amount<= DOList.Product.GetById(item.ProductID).InStock)//if orderItem exists and is instock
            {
                DO.Order order = new DO.Order();//new DO order
                order.OrderDate = DateTime.Now;//ordered now
                int num = DOList.Order.Add(order);//add to DO orderlist and get order id
                oi.ProductID = item.ProductID;//save product id
                oi.OrderID = num;//save order id
                DOList.OrderItem.Add(oi);//add to DO order item list 
                DO.Product p = DOList.Product.GetById(oi.ProductID);//get matching product
                p.InStock -= item.Amount;//subtract the amount of products in stock
                DOList.Product.Update(p);//update product in DO
            }
            throw new BO.Exceptions("can not place order");
        }
       
    }
}
