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
    readonly private static IDal? DOList = DalApi.Factory.Get()?? throw new BO.Exceptions("Factory does not exist\n");//to access DO info
    public BO.Cart AddToCart(BO.Cart myCart, int id)
    {
        if (myCart.orderItems == null)//if first order item in cart 
        {
            DO.Product? p = new DO.Product?();//create a DO product
            try
            {
                p = DOList?.Product.GetById(id);//get the matching product for the ID
            }
            catch (DalApi.IdNotExistException)
            {
                throw new BO.IdNotExistException("The product requested does not exist\n");
            }
            if (p == null || p?.InStock < 1 || p?.IsDeleted == true)//if the product does not exist or out of stock
            {
                throw new BO.UnfoundException("The product requested is unavailable");
            }
            BO.OrderItem o = new BO.OrderItem//create new orderitem that is being added 
            {
                ID = id,
                ProductName = p?.Name!,
                ProductPrice = (double)p?.Price!,
                IsDeleted = false,
                Amount = 1,
                Price = (double)p?.Price!,
                ProductID = p?.ID ?? throw new BO.Exceptions("The id of the product from the list is null")
            };
            myCart.orderItems = new List<BO.OrderItem?>();//new list in cart bcs first order item
            myCart.orderItems!.Add(o);//add the orderitem to cart
            myCart.Price = (double)p?.Price!;//add the prudoct price to cart price
            return myCart;
        }
        int ind = myCart.orderItems!.FindIndex(x => x!=null && x.ID == id); //save index of order with ID in cart
        DO.Product? product = new DO.Product?();//create a DO product
        try
        {
            product = DOList?.Product.GetById(id);//get the matching product for the ID
        }
        catch (DalApi.IdNotExistException)
        {
            throw new BO.IdNotExistException("The product requested does not exist\n");
        }
        if (product==null || product?.InStock < 1 || product?.IsDeleted==true)
        {
            throw new BO.UnfoundException("The product requested is unavailable");
        }
        if (ind != -1)//exists in cart
        {
            myCart.orderItems[ind]!.Amount++; //add another product to the cart
            myCart.orderItems[ind]!.ProductPrice += myCart.orderItems[ind]!.Price;//add to the total price of order item
            myCart.Price += myCart.orderItems[ind]!.Price;//update cart price
            return myCart;
        }
        BO.OrderItem oi = new BO.OrderItem//create new orderitem that is being added 
        {
            ID = id,
            ProductName = product?.Name!,
            ProductPrice = (double)product?.Price!,
            IsDeleted = false,
            Amount = 1,
            Price = (double)product?.Price!,
            ProductID = product?.ID ?? throw new BO.Exceptions("The id of the product from the list is null")
        };
        try
        {
            myCart.orderItems.Add(oi);//add the orderitem to cart
        }
        catch (DalApi.IdExistException)
        {
            throw new BO.IdExistException("The product requested already exists");
        }
        myCart.Price += oi.Price;//update price of the cart
        return myCart;

    }
    public BO.Cart UpdateCart(BO.Cart myCart, int id, int amount){


        int ind = myCart.orderItems!.FindIndex(x => x!.ProductID == id); //save index of product with ID in cart
        DO.Product product = new DO.Product();//create a DO product
        try
        {
            product = (DO.Product)DOList?.Product.GetById(id)!;
        }
        catch (DalApi.IdNotExistException)
        {
            throw new BO.IdNotExistException("The product requested does not exist\n");
        }
        if (ind != -1)//if in cart
        {
            if (amount == 0)//to delete a product
            {
                BO.OrderItem temp= myCart.orderItems[ind]!;//save the orderitem with id
                myCart.Price -= myCart.orderItems[ind]!.ProductPrice;//update cart price
                myCart.orderItems.Remove(temp);//remove orderItem from cart
                return myCart;
            }
            myCart.Price -= myCart.orderItems[ind]!.ProductPrice; //substract price of product from cart
            myCart.orderItems[ind]!.Amount = amount;//set new amount
            myCart.orderItems[ind]!.ProductPrice = myCart.orderItems[ind]!.Price * amount;//set new order item total price according to new amount
            myCart.Price += myCart.orderItems[ind]!.ProductPrice;//add the new price
            return myCart;
        }
        throw new BO.IdNotExistException("The product requested does not exist");//product does not exist in cart 
    }
    public int MakeOrder(BO.Cart myCart)
    {
        if (myCart.CustomerName == "" || myCart.CustomerEmail =="" || myCart.CustomerAddress =="")//check input
        {
            throw new BO.UnfoundException("Incorrect Input of an order entered");
        }
        IEnumerable<DO.Product?> productList = DOList?.Product.GetAll()!;//get all products from dal
        IEnumerable<string> checkOrderItem = from BO.OrderItem item in myCart.orderItems!
                                             let product = productList.FirstOrDefault(x => x?.ID == item.ID)
                                             where item.Amount < 1 || product?.InStock < item.Amount
                                             select item.ProductName + " is not in stock\n";//check if all of the products in cart are in stock
        if (checkOrderItem.Any())//if no products are available 
            throw new BO.IdNotExistException("The product requested does not exist\n");

        int? orderId=DOList?.Order.Add(new DO.Order()
        {
            CostumerAddress = myCart.CustomerAddress!,
            CostumerEmail = myCart.CustomerEmail!,
            CostumerName = myCart.CustomerName!,
            IsDeleted = false,
            OrderDate = DateTime.Now
        });//add a new order for the cart and get order ID
        try
        {
            myCart.orderItems!.ForEach(x => DOList?.OrderItem.Add(new DO.OrderItem()
            {
                Amount = (int)x?.Amount!,
                ID = x.ID,
                IsDeleted = x.IsDeleted,
                OrderID = (int)orderId!,
                Price = x.Price,
                ProductID = x.ProductID
            }));//go over cart order items and add each to dal
        }
        catch (DalApi.Exceptions ex)
        {
            throw new BO.Exceptions(ex.Message);
        }
        catch(DalApi.IdExistException ex)
        {
            throw new BO.Exceptions(ex.Message);
        }
        IEnumerable<DO.Product?> products;
        try
        {
            products = from item in myCart.orderItems
                       where item != null
                       select DOList?.Product.GetById(item.ProductID);//list of products in cart
        }
        catch (DalApi.IdNotExistException)
        {
            throw new BO.IdNotExistException("The product requested does not exist\n");
        }

        //products.Zip(myCart.orderItems, (first, second) => first.InStock -= second!.Amount).ToList().ForEach(x => DOList?.Product.GetByFilter(x));
        ////update the amount of products
        foreach (BO.OrderItem? item in myCart.orderItems!)//go over orderItems in the cart
        {
            try
            {
                DO.Product? prod = DOList?.Product.GetById(item.ProductID);
                if (item!.ProductID == prod?.ID && item.Amount > 0 && item.Amount <= prod?.InStock)//if orderItem exists and is instock
                {
                    DO.Product p;
                    try
                    {
                        p = (DO.Product)(DOList?.Product.GetById(item.ProductID)!);//get matching product
                    }
                    catch (DalApi.IdNotExistException)
                    {
                        throw new BO.IdNotExistException("The product requested does not exist\n");
                    }
                    p.InStock -= item.Amount;//subtract the amount of products in stock
                    try
                    {
                        DOList?.Product.Update(p);//update product in DO
                    }
                    catch (DalApi.IdNotExistException)
                    {
                        throw new BO.IdNotExistException("The product requested does not exist\n");
                    }
                }
            }
            catch (DalApi.IdNotExistException)
            {
                throw new BO.IdNotExistException("The product requested does not exist\n");
            }
            //throw new BO.Exceptions("Can not place the order requested");
        }
        return (int)orderId!;
    }
}
