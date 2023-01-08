using System;
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

        //check if the customer exists in th file
        var customerFromFile = (from order in orderRoot.Elements()
                                where (order.Element("ID").Value == item.ID.ToString())
                                select order).FirstOrDefault();

        //throw an exception
        if (customerFromFile != null)
            throw new DalApi.IdExistException("the order already exit");

        //add the customer to the root element
        orderRoot.Add(
            new XElement("order",
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
        
    }

    public IEnumerable<DO.Order?> GetAll(Func<DO.Order?, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public DO.Order GetByFilter(Func<DO.Order?, bool>? filter)
    {
        throw new NotImplementedException();
    }

    public DO.Order GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(DO.Order item)
    {
        throw new NotImplementedException();
    }
}
