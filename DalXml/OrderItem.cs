using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;

internal class OrderItem : IOrderItem
{
    string orderItemPath = @"OrderItem.xml";
    public int Add(DO.OrderItem item)
    {
        XElement orderItemRoot = XmlTools.LoadListFromXMLElement(orderItemPath); //get all the elements from the file

        //check if the orderItem exists in th file
        var orderItemFromFile = (from oi in orderItemRoot.Elements()
                                 where (oi != null && oi.Element("ID")!.Value == item.ID.ToString())
                                 select oi).FirstOrDefault();

        //throw an exception
        if (orderItemFromFile != null)
            throw new DalApi.IdExistException("the order already exists");

        //add the orderItem to the root element
        orderItemRoot.Add(
            new XElement("OrderItem",
            new XElement("ID", item.ID),
            new XElement("ProductID", item.ProductID),
            new XElement("OrderID", item.OrderID),
            new XElement("Price", item.Price),
            new XElement("Amount", item.Amount)));

        //save the root in the file
        XmlTools.SaveListToXMLElement(orderItemRoot, orderItemPath);
        return item.ID;
    }

    public void Delete(int id)
    {
        List<DO.OrderItem?> orderItemList = XmlTools.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath);

        DO.OrderItem temp = (from item in orderItemList
                             where item != null && item?.ID == id
                             select (DO.OrderItem)item).FirstOrDefault();

        if (temp.ID.Equals(default(Order)))
            throw new DalApi.IdNotExistException("the product does not exist");

        orderItemList.Remove(temp);

        XmlTools.SaveListToXMLSerializer(orderItemList, orderItemPath);
    }

    public IEnumerable<DO.OrderItem?> GetAll(Func<DO.OrderItem?, bool>? filter = null)
    {
        List<DO.OrderItem?> orderItemList = XmlTools.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath).ToList();

        return (from order in orderItemList
                where filter(order)
                select order).ToList();
    }

    public DO.OrderItem GetByFilter(Func<DO.OrderItem?, bool>? filter)
    {
        List<DO.OrderItem?> orderItemList = GetAll().ToList();

        return (from item in orderItemList
                where filter(item)
                select (DO.OrderItem)item).FirstOrDefault();
    }

    public DO.OrderItem GetById(int id)
    {
        List<DO.OrderItem?> orderItemList = GetAll().ToList();

        return (from item in orderItemList
                where item != null && item?.ID == id
                select (DO.OrderItem)item).FirstOrDefault();
        throw new DalApi.IdNotExistException("the Order Item requested does not exist");
    }

    public DO.OrderItem ItemOfOrder(int id, int productId)
    {
        List<DO.OrderItem?> orderItemList = GetAll().ToList();

        return (from item in orderItemList
                where item != null && item?.ProductID == productId && item?.OrderID==id
                select (DO.OrderItem)item).FirstOrDefault();
        throw new DalApi.IdNotExistException("the Order Item requested does not exist");
    }

    public IEnumerable<DO.OrderItem?> ItemsInOrder(int id)
    {
        List<DO.OrderItem?> orderItemList = GetAll().ToList();

        return (from item in orderItemList
                where item?.OrderID == id
                select item).ToList();
    }

    public void Update(DO.OrderItem item)
    {
        DO.OrderItem? temp = GetById(item.ID);//get the order item requested to update 
        List<DO.OrderItem?> orderItemList = GetAll().ToList();//get all order items from ile
        orderItemList.Remove(temp);//remove the existing order item
        orderItemList.Add(item);//add the updated order item

        XmlTools.SaveListToXMLSerializer(orderItemList, orderItemPath);//save back into file    
    }
}
