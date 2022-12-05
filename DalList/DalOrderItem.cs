﻿using System;
using System.Threading;
using DO;
using DalApi;
namespace Dal;

public class DalOrderItem : IOrderItem
{
    DataSource _ds = DataSource.s_instance;//to access the data 

    public int Add(OrderItem oi)//add OrderItem to a list and return its id
    {
        if (oi.ID == 0)//want to add a new item to the list
        {
            oi.ID = DataSource.Config.NextOrderItemNumber;//set an id number to Product p
            _ds.orderItemList.Add(oi);//add p to the Product list
            return oi.ID;//return the id
        }
        int ind = _ds.orderItemList.FindIndex(x => x?.ID == oi.ID && x?.IsDeleted == false);//save index of orderItem with matching id if not deleted
        if (ind != -1)//exists already so cant add again
        {
            throw new Exception("Unothorized override");//error
        }
        ind = _ds.orderItemList.FindIndex(x => x?.ID == oi.ID && x?.IsDeleted == true);//save index of orderItem with matching id if deleted
        if (ind != -1)//already exists but deleted 
        {
            _ds.orderItemList.Add(oi);//add oi to the orderItem list
            return oi.ID;//return the id
        }
        throw new Exception("Unothorized override");//error

    }

    public OrderItem GetById(int id)
    {
        OrderItem? res = _ds.orderItemList.Find(x => x?.OrderID == id && x?.IsDeleted == false);//find a priduct with same id and exists
        if (res?.ID != id || res?.IsDeleted == true)//if not found
            throw new Exception("The OrderItem does not exist\n");
        return res.Value;
        
    }

    public void Delete(int id)
    {
        int index = _ds.orderItemList.FindIndex(x => x.Value.ID == id);

        if (index == -1)//if does not exist
            throw new IdNotExistException("Order item does not exist");
        _ds.orderItemList.RemoveAt(index);//remove from list
    }

    public void Update(OrderItem oi)
    {
        int index = _ds.orderItemList.FindIndex(x => x.Value.ID == oi.ID);

        if (index == -1)//if does not exist
            throw new IdNotExistException("The order item you wish to update does not exist");
        _ds.orderItemList[index] = oi;//place new order item in place of existing one 
    }

    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool>? filter)
    {
        if (filter == null)//select whole list
        {
            return from v in _ds.orderItemList
                   where v?.IsDeleted == false
                   select v;
        }
        return from v in _ds.orderItemList//select with filter
               where v?.IsDeleted == false && filter(v)
               select v;

    }

    public IEnumerable<OrderItem?> ItemsInOrder(int id)   //returns list of products in order number of id
    {
        return from v in _ds.orderItemList//go over OrderItem list
               where v?.OrderID == id && v?.IsDeleted== false//if found a matching id to the one inputted
               select v;
   
    }

    public OrderItem ItemOfOrder(int id, int productId)//returns specific product from order of id
    {
        OrderItem returnOI = new();
        foreach (OrderItem orderi in _ds.orderItemList)//go over OrderItem list
        {
            if (orderi.OrderID == id && orderi.ProductID == productId)//if found a matching id to the one inputted and product
            { 
                if(orderi.IsDeleted == false)//not deleted
                {
                    returnOI = orderi;//save that one
                }
            }
        }//find the order of id with product
        return returnOI;//return the OrderItem
    }



    public OrderItem GetByFilter(Func<OrderItem?, bool>? filter)
    {
        foreach (OrderItem o in _ds.orderItemList)
        {
            if (o.IsDeleted == false && filter(o))
            {
                return o;
            }
        }
        throw new Exceptions("Does not exist\n");
    }
}