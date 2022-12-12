using System;
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
        int ind = myCart.orderItems!.FindIndex(x => x!=null && x.ID == id); //save index of order with ID in cart
        DO.Product? product = new DO.Product?();//create a DO product
        try
        {
            product = DOList?.Product.GetById(id);//get the matching product for the ID
        }
        catch (DalApi.IdNotExistException)
        {
            throw new BO.IdNotExistException("id does not exist\n");
        }
        if (product?.InStock < 1 || product?.IsDeleted==true)
        {
            throw new BO.UnfoundException("Product unavailable");
        }
        if (ind != -1)//exists in cart
        {
            myCart.orderItems[ind]!.Amount++; //add another product to the cart
            myCart.orderItems[ind]!.Price += myCart.orderItems[ind]!.ProductPrice;//add to the total price 
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
            ProductID = product?.ID ?? throw new Exception()
        };
        try
        {
            myCart.orderItems.Add(oi);//add the orderitem to cart
        }
        catch (DalApi.IdExistException)
        {
            throw new BO.IdExistException("product id already exists");
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
            throw new BO.IdNotExistException("id does not exist\n");
        }
        //if (product.InStock + myCart.orderItems[ind].Amount - amount < 1) //if the new amount is impossible
        //{
        //    throw new BO.UnfoundException("Incorrect Amount");
        //}
        if (ind != -1)//if in cart
        {
            if (amount == 0)
            {
                BO.OrderItem temp= myCart.orderItems[ind]!;//save the orderitem with id
                myCart.orderItems.Remove(temp);//remove orderItem from cart
                myCart.Price -= myCart.orderItems[ind]!.Price;
                return myCart;
            }
            myCart.Price -= myCart.orderItems[ind]!.Price * myCart.orderItems[ind]!.Amount; //substract price of product from cart
            myCart.orderItems[ind]!.Amount = amount;//set new amount
            myCart.Price += myCart.orderItems[ind]!.Price * amount;//add the new price
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
        foreach (BO.OrderItem? item in myCart.orderItems!)//go over orderItems in the cart
        {
            try
            {
                if (item!.ProductID == DOList?.Product.GetById(item.ProductID).ID && item.Amount > 0 && item.Amount <= DOList?.Product.GetById(item.ProductID).InStock)//if orderItem exists and is instock
                {
                    DO.Order order = new DO.Order();//new DO order
                    order.OrderDate = DateTime.Now;//ordered now
                    int num;
                    try
                    {
                        num = (int)(DOList?.Order.Add(order)!);//add to DO orderlist and get order id
                    }
                    catch (DalApi.IdNotExistException)
                    {
                        throw new BO.IdNotExistException("id does not exist\n");
                    }
                    oi.ProductID = item.ProductID;//save product id
                    oi.OrderID = num;//save order id
                    try
                    {
                        DOList.OrderItem.Add(oi);//add to DO order item list 
                    }
                    catch (DalApi.IdNotExistException)
                    {
                        throw new BO.IdNotExistException("id does not exist\n");
                    }
                    DO.Product p;
                    try
                    {
                        p = (DO.Product)(DOList?.Product.GetById(oi.ProductID)!);//get matching product
                    }
                    catch (DalApi.IdNotExistException)
                    {
                        throw new BO.IdNotExistException("id does not exist\n");
                    }
                    p.InStock -= item.Amount;//subtract the amount of products in stock
                    try
                    {
                        DOList?.Product.Update(p);//update product in DO
                    }
                    catch (DalApi.IdNotExistException)
                    {
                        throw new BO.IdNotExistException("Product does not exist");
                    }
                }
            }
            catch (DalApi.IdNotExistException)
            {
                throw new BO.IdNotExistException("id does not exist\n");
            }
            throw new BO.Exceptions("can not place order");
        }
       
    }
}
