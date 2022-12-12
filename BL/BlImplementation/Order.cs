using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using DalApi;
using DO;

namespace BlImplementation;

internal class Order : BlApi.IOrder
{
    readonly private static IDal? DOList = DalApi.Factory.Get()??throw new BO.Exceptions("Factory does not exist\n");//to access DO info  
    public IEnumerable<OrderForList?> GetAllOrderForList()
    {
        IEnumerable<DO.Order?>? orders = DOList?.Order.GetAll();//get all orders from DO 
        IEnumerable<DO.OrderItem?>? orderItems = DOList?.OrderItem.GetAll();//get all orderItems from DO 
        return from DO.Order? ord in orders
               select new BO.OrderForList
               {
                   ID = ord?.ID ?? throw new BO.IdNotExistException("ID does not exist\n"),
                   Name = ord?.CostumerName??throw new BO.IncorrectInput("name does not exist\n"),
                   Status = GetStatus(ord ?? throw new BO.IncorrectInput("status does not exist\n")),
                   Amount = orderItems.Select(orderItems => orderItems?.ID == ord?.ID).Count(),
                   TotalPrice = (double)orderItems.Sum(orderItems => orderItems?.Price ?? throw new BO.IncorrectInput("price does not exist\n"))
              };
    
    }//calls get of DO order list, gets items for each order, and build orderorlist

    private BO.Enums.Status GetStatus(DO.Order order)
    {

        return order.DeliveryDate != null ? BO.Enums.Status.Recieved: order.ShipDate != null ?
            BO.Enums.Status.Shipped: BO.Enums.Status.JustOrdered;
    }

    public BO.Order GetBoOrder(int id)
    {
        if (id < 0)//id is negative
        {
            throw new BO.IdNotExistException();
        }
        DO.Order ord;
        try
        {
            ord = (DO.Order)(DOList?.Order.GetById(id)!);//get right DO Order ?????????????
        }
        catch (DalApi.IdNotExistException)
        {
            throw new BO.IdNotExistException("id does not exist\n");
        }
        double priceTemp = 0;
        foreach(DO.OrderItem? o in DOList?.OrderItem.GetAll()!)
        {
            if (o?.IsDeleted == false && o?.OrderID == id)
            {
                priceTemp+=o?.Price ?? 0;//add up all of prices in the order
            }
        }
        if(ord.IsDeleted == false && ord.ID == id)//if exists 
        {
            return new BO.Order
            {
                ID = id,
                CostumerAddress = ord.CostumerAddress,
                CostumerEmail = ord.CostumerEmail,
                CostumerName = ord.CostumerName,
                OrderDate = ord.OrderDate ?? throw new Exception(),
                ShipDate = ord.ShipDate ?? throw new Exception(),
                DeliveryDate = ord.DeliveryDate ?? throw new Exception(),
                Status = GetStatus(ord),
                TotalPrice = priceTemp,
                IsDeleted = ord.IsDeleted
            };//new BO Order
        }
        throw new BO.UnfoundException("Order does not exist\n");
    }//get order number, check if exists, update date in DO order, and return BO order that has been "shipped"
    public BO.Order DeliveredUpdate(int orderId)
    {
        DO.Order oId;
        try
        {
            oId = (DO.Order)(DOList?.Order.GetById(orderId)!);//get the order from DO of orderId-or catch exception
        }
        catch (DalApi.IdNotExistException)
        {
            throw new BO.IdNotExistException("id does not exist\n");
        }

        if (oId.ID == orderId /*&& oId.DeliveryDate < DateTime.Today*/)//if oId exists and has not been shipped 
        {
            DO.Order o = new DO.Order()
            {
                ID = orderId,
                CostumerAddress = oId.CostumerAddress,
                CostumerEmail = oId.CostumerEmail,
                CostumerName = oId.CostumerName,
                OrderDate = oId.OrderDate,
                ShipDate = oId.ShipDate,
                DeliveryDate = DateTime.Now,//the only difference
                IsDeleted = oId.IsDeleted
            };//set new delivery date in new DO Order
            try
            {
                DOList?.Order.Update(o);//update the order in DO
            }
            catch (DalApi.IdNotExistException)
            {
                throw new BO.IdNotExistException("Product does not exist");
            }
            double priceTemp = 0;
            foreach (DO.OrderItem? temp in DOList?.OrderItem.GetAll()!)
            {
                if (temp?.IsDeleted == false && temp?.OrderID == o.ID)
                {
                    priceTemp += temp?.Price ?? 0;//add up all of prices in the order
                }
            }
            return new BO.Order
            {
                ID = orderId,
                CostumerAddress = oId.CostumerAddress,
                CostumerEmail = oId.CostumerEmail,
                CostumerName = oId.CostumerName,
                OrderDate = oId.OrderDate ?? throw new Exception(),
                ShipDate = oId.ShipDate ?? throw new Exception(),
                DeliveryDate = DateTime.Now,
                Status = GetStatus(o),
                TotalPrice = priceTemp,
                IsDeleted = oId.IsDeleted
            };//new BO Order
        }
        throw new BO.UnfoundException("Order does not exist\n");

    }//get order number, check if exists, update date in DO order, and return BO order that has been "delivered" 
    public BO.Order ShipUpdate(int orderId)
    {
        DO.Order oId;
        try
        {
            oId = (DO.Order)(DOList?.Order.GetById(orderId)!);//get the order from DO of orderId-or catch exception
        }
        catch (DalApi.IdNotExistException)
        {
            throw new BO.IdNotExistException("id does not exist\n");
        }
        if (oId.ID == orderId /*&& oId.ShipDate < DateTime.Today*/ )//if oId exists and has not been shipped 
        {
            DO.Order o = new()
            {
                ID = orderId,
                CostumerAddress = oId.CostumerAddress,
                CostumerEmail = oId.CostumerEmail,
                CostumerName = oId.CostumerName,
                OrderDate = oId.OrderDate,
                ShipDate = DateTime.Now,
                DeliveryDate = null,
                IsDeleted = oId.IsDeleted
            };//set new ship date in new DO Order
            try
            {
                DOList?.Order.Update(o);//update the order in DO
            }
            catch (DalApi.IdNotExistException)
            {
                throw new BO.IdNotExistException("Product does not exist");
            }
            double priceTemp = 0;
            foreach (DO.OrderItem? temp in DOList?.OrderItem.GetAll()!)
            {
                if (temp?.IsDeleted == false && temp?.OrderID == o.ID)
                {
                    priceTemp += temp?.Price ?? 0;//add up all of prices in the order
                }
            }
            return new BO.Order
            {
                ID = orderId,
                CostumerAddress = oId.CostumerAddress,
                CostumerEmail = oId.CostumerEmail,
                CostumerName = oId.CostumerName,
                OrderDate = oId.OrderDate ?? throw new Exception(),
                ShipDate = DateTime.Now,
                Status = GetStatus(o),
                TotalPrice = priceTemp,
                DeliveryDate = null,
                IsDeleted = oId.IsDeleted
            };//new BO Order
        }
        throw new BO.UnfoundException("Order does not exist\n");
    }
    public OrderTracking GetOrderTracking(int orderId)
    {
        OrderTracking ot=new();//create new order tracking
        ot.Tracking = new();
        foreach (DO.Order? item in DOList?.Order.GetAll()!)//go over all orders in DO
        {
            if (item?.ID == orderId )//if order exists 
            {
                ot.ID = orderId;//save id
                if (item?.DeliveryDate != null)//if order delivered 
                {
                    ot.Status = BO.Enums.Status.Shipped;//save status
                    ot.Tracking.Add(Tuple.Create(item?.OrderDate ?? throw new BO.UnfoundException(), "The order has been created\n"));//save tracking
                    ot.Tracking.Add(Tuple.Create(item?.ShipDate ?? throw new BO.UnfoundException(), "The order has been shipped\n"));//save tracking
                    ot.Tracking.Add(Tuple.Create(DateTime.Now, "The order has been delivered\n"));//save tracking
                    return ot;
                }
                if (item?.ShipDate != null)//if order shipped 
                {
                    ot.Status = BO.Enums.Status.Shipped;//save status
                    ot.Tracking.Add(Tuple.Create(item?.OrderDate ?? throw new BO.UnfoundException(), "The order has been created\n"));//save tracking
                    ot.Tracking.Add(Tuple.Create(DateTime.Now, "The order has been shipped\n"));//save tracking
                    //ot.Tracking.Add(Tuple.Create(null, "The order has been delivered\n"));//save tracking
                    return ot;
                }
                if (item?.OrderDate == DateTime.Now)//if order created now
                {
                    ot.Status = BO.Enums.Status.JustOrdered;//save status
                    ot.Tracking.Add(Tuple.Create( DateTime.Now,"The order has been created\n"));//save tracking
                    //ot.Tracking.Add(Tuple.Create (item?.ShipDate??throw new BO.UnfoundException(), "The order has been shipped\n"));//save tracking
                    //ot.Tracking.Add(Tuple.Create(item?.DeliveryDate ?? throw new BO.UnfoundException(), "The order has been delivered\n" ));//save tracking
                    return ot;
                }
               
                
            }
        }
        throw new BO.UnfoundException("Order does not exist\n");//order does not exist
    }//get order id, check if exists, and build strings of dates and status in DO orders


}
