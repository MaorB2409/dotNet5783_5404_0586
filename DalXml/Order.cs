﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;

internal class Order : IOrder
{
    string orderPath = @"Order.xml";
    public int Add(DO.Order item)
    {
        XElement orderRoot = XmlTools.LoadListFromXMLElement(orderPath); //get all the elements from the file

        //check if the order exists in th file
        var orderFromFile = (from order in orderRoot.Elements()
                                where (order!=null && order.Element("ID")!.Value == item.ID.ToString())
                                select order).FirstOrDefault();

        //throw an exception
        if (orderFromFile != null)
            throw new DalApi.IdExistException("the order already exists");

        //add the order to the root element
        orderRoot.Add(
            new XElement("Order",
            new XElement("ID", item.ID),
            new XElement("CostumerName", item.CostumerName),
            new XElement("CostumerEmail", item.CostumerEmail),
            new XElement("CostumerAddress", item.CostumerAddress),
            new XElement("OrderDate", item.OrderDate)));

        //save the root in the file
        XmlTools.SaveListToXMLElement(orderRoot, orderPath);
        return item.ID;
    }

    public void Delete(int id)
    {
        List<DO.Order?> orderList = XmlTools.LoadListFromXMLSerializer<DO.Order?>(orderPath);

        DO.Order temp = (from item in orderList
                                where item!=null&&item?.ID == id
                                      select (DO.Order)item).FirstOrDefault();

        if (temp.ID.Equals(default(Order)))
            throw new DalApi.IdNotExistException("the product does not exist");

        orderList.Remove(temp);

        XmlTools.SaveListToXMLSerializer(orderList, orderPath);
    }

    public IEnumerable<DO.Order?> GetAll(Func<DO.Order?, bool>? filter = null)
    {
        List<DO.Order?> orderList = XmlTools.LoadListFromXMLSerializer<DO.Order?>(orderPath).ToList();

        return (from order in orderList
                where filter(order)
                select order).ToList();
    }

    public DO.Order GetByFilter(Func<DO.Order?, bool>? filter)
    {
        List<DO.Order?> orderList = GetAll().ToList();

        return (from item in orderList
                where filter(item)
                select (DO.Order)item).FirstOrDefault();
    }

    public DO.Order GetById(int id)
    {
        List<DO.Order?> orderList = GetAll().ToList();

        return (from item in orderList
                where item!=null && item?.ID==id
                select (DO.Order)item).FirstOrDefault();
        throw new DalApi.IdNotExistException("the order requested does not exist");
    }

    public void Update(DO.Order item)
    {
        DO.Order? temp = GetById(item.ID);//get the order requested to update 
        List<DO.Order?> orderList = GetAll().ToList();//get all orders from ile
        orderList.Remove(temp);//remove the existing order
        orderList.Add(item);//add the updated order

        XmlTools.SaveListToXMLSerializer(orderList, orderPath);//save back into file
    }
}
