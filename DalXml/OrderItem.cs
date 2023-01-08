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

        //check if the customer exists in th file
        var orderItemFromFile = (from orderItem in orderItemRoot.Elements()
                                where (orderItem.Element("ID").Value == item.ID.ToString())
                                select orderItem).FirstOrDefault();

        //throw an exception
        if (orderItemFromFile != null)
            throw new DalApi.IdExistException("the order already exit");

        //add the customer to the root element
        orderItemRoot.Add(
            new XElement("orderItem",
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
        
    }
    //throw DalApi.IDExist/////////////////////////////
    public IEnumerable<DO.OrderItem?> GetAll(Func<DO.OrderItem?, bool>? filter = null)
    {
        return (IEnumerable<DO.OrderItem?>)XmlTools.LoadListFromXMLElement(orderItemPath); //get all the elements from the file

        //return (from oi in orderItemRoot.Elements()
        //        select new OrderItem()
        //        {
        //            ID = int.Parse(oi.Element("ID").Value),
        //            ProductID = int.Parse(oi.Element("ProductID").Value),
        //            OrderID = int.Parse(oi.Element("OrderID").Value),
        //            Price = double.Parse(oi.Element("ProductID").Value),
        //            Amount =  int.Parse(oi.Element("Amount").Value)
        //        }).ToList();
    }

    public DO.OrderItem GetByFilter(Func<DO.OrderItem?, bool>? filter)
    {
        throw new NotImplementedException();
    }

    public DO.OrderItem GetById(int id)
    {
        XElement orderItemRoot = XmlTools.LoadListFromXMLElement(orderItemPath); //get all the elements from the file

        //check if the customer exists in th file
        var orderItemFromFile = (from oi in orderItemRoot.Elements()
                                where (oi.Element("ID").Value == id.ToString())
                                select oi).FirstOrDefault();

        //throw an exception
        if (orderItemFromFile == null)
            throw new DalApi.IdNotExistException("Order Item does not exist");

        return new OrderItem()
        {
            ID = id,
            ProductID = int.Parse(orderItemFromFile.Element("ProductID").Value),
            OrderID = int.Parse(orderItemFromFile.Element("OrderID").Value),
            Price = double.Parse(orderItemFromFile.Element("Price").Value),
            Amount = int.Parse(orderItemFromFile.Element("Amount").Value),


        };
    }

    public DO.OrderItem ItemOfOrder(int id, int productId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<DO.OrderItem?> ItemsInOrder(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(DO.OrderItem item)
    {
        throw new NotImplementedException();
    }
}
