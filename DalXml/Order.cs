using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;
using static DataSource;

internal class Order : IOrder
{
    string orderPath = @"Order.xml";
    string configPath = @"Config.xml";
    public int Add(DO.Order order)
    {
        var listOrders = XmlTools.LoadListFromXMLSerializer<DO.Order>(orderPath);

        if (listOrders.Exists(lec => lec.ID == order.ID && lec.IsDeleted == false))
            throw new IdExistException("The order already exists");

        var runningList = XmlTools.LoadListFromXMLSerializer<RunningNumber>(configPath);


        RunningNumber runningNum = runningList.FirstOrDefault(num => num.typeOfnumber == "Order running number");

        runningList.Remove(runningNum);

        runningNum.numberSaved++;

        order.ID = (int)runningNum.numberSaved;

        listOrders.Add(order);
        runningList.Add(runningNum);

        XmlTools.SaveListToXMLSerializer(listOrders, orderPath);
        XmlTools.SaveListToXMLSerializer(runningList, configPath);

        return order.ID;
        //XElement orderRoot = XmlTools.LoadListFromXMLElement(orderPath); //get all the elements from the file

        ////check if the order exists in th file
        //var orderFromFile = (from order in orderRoot.Elements()
        //                        where (order!=null && order.Element("ID")!.Value == item.ID.ToString())
        //                        select order).FirstOrDefault();

        ////throw an exception
        //if (orderFromFile != null)
        //    throw new DalApi.IdExistException("the order already exists");

        ////get running order ID number
        //List<RunningNumber> runningList = XmlTools.LoadListFromXMLSerializer<RunningNumber>(configPath);

        //var runningNum = (from number in runningList
        //                  where (number.typeOfnumber == "Order running number")
        //                  select number).FirstOrDefault();
        //runningList.Remove(runningNum);//remove the saved number from list
        //runningNum.numberSaved++;//add one to the saved number
        //runningList.Add(runningNum);//add the number back to list
        //int temp = (int)runningNum.numberSaved;//save the running number

        ////add the order to the root element
        //orderRoot.Add(
        //    new XElement("Order",
        //    new XElement("ID", temp),
        //    new XElement("CostumerName", item.CostumerName),
        //    new XElement("CostumerEmail", item.CostumerEmail),
        //    new XElement("CostumerAddress", item.CostumerAddress),
        //    new XElement("OrderDate", item.OrderDate),
        //    new XElement("ShipDate", item.ShipDate),
        //    new XElement("DeliveryDate", item.DeliveryDate)));

        ////save the root in the file
        //XmlTools.SaveListToXMLElement(orderRoot, orderPath);
        //return item.ID;
    }

    public void Delete(int id)
    {

        var listOrders = XmlTools.LoadListFromXMLSerializer<DO.Order>(orderPath);

        var o = listOrders.FirstOrDefault(p => p.ID == id);

        if (o.IsDeleted)
            throw new IdNotExistException("The order does not exist");

        listOrders.Remove(o);

        DO.Order order = new()
        {
            ID = id,
            CostumerName = o.CostumerName,
            CostumerEmail = o.CostumerEmail,
            CostumerAddress = o.CostumerAddress,
            OrderDate = o.OrderDate,
            DeliveryDate = o.DeliveryDate,
            ShipDate = o.ShipDate,
            IsDeleted = true
        };

        listOrders.Add(order);
        XmlTools.SaveListToXMLSerializer(listOrders, orderPath);
        //List<DO.Order?> orderList = XmlTools.LoadListFromXMLSerializer<DO.Order?>(orderPath);

        //DO.Order temp = (from item in orderList
        //                        where item!=null&&item?.ID == id
        //                              select (DO.Order)item).FirstOrDefault();

        //if (temp.ID.Equals(default(Order)))
        //    throw new DalApi.IdNotExistException("the product does not exist");

        //orderList.Remove(temp);

        //XmlTools.SaveListToXMLSerializer(orderList, orderPath);
    }





    public void Update(DO.Order order)
    {

        var listOrders = XmlTools.LoadListFromXMLSerializer<DO.Order>(orderPath);
        var o = listOrders.FirstOrDefault(p => p.ID == order.ID);
        listOrders.Remove(o);
        listOrders.Add(order);
        XmlTools.SaveListToXMLSerializer(listOrders, orderPath);
        //DO.Order? temp = GetById(item.ID);//get the order requested to update 
        //List<DO.Order?> orderList = GetAll().ToList();//get all orders from ile
        //orderList.Remove(temp);//remove the existing order
        //orderList.Add(item);//add the updated order

        //XmlTools.SaveListToXMLSerializer(orderList, orderPath);//save back into file
    }
    /*
     
        public int ID { get; set; } //item ID
    public string CostumerName { get; set; }
    public string CostumerEmail { get; set; }
    public string CostumerAddress { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ShipDate { get; set; }
    public DateTime? DeliveryDate { get; set; }*/

    public static DO.Order? GetOrder(XElement o) =>
    o?.ToInt("ID") is null ? null : new DO.Order()
    {
        ID = o.ToInt("ID") ?? 0,
        CostumerName = (string?)(o?.Element("CostumerName")?.Value)??null,
        CostumerEmail = (string?)(o.Element("CostumerEmail")?.Value)??null,
        CostumerAddress = (string?)(o.Element("CostumerAddress")?.Value)??null,
        OrderDate= o.ToDateTimeNullable(o?.Element("OrderDate").Value)??null,
        ShipDate= o.ToDateTimeNullable(o?.Element("OrderDate").Value)??null,
        DeliveryDate = o.ToDateTimeNullable(o?.Element("OrderDate").Value)??null

    };

    public IEnumerable<DO.Order?> GetAll(Func<DO.Order?, bool>? filter = null)
    {

        var listOrders = XmlTools.LoadListFromXMLSerializer<DO.Order?>(orderPath)!;
        return filter == null ? listOrders.OrderBy(lec => ((DO.Order)lec!).ID)
                              : listOrders.Where(filter).OrderBy(lec => ((DO.Order)lec!).ID);
    }
       //=>
       //filter is null
       //? XmlTools.LoadListFromXMLElement(orderPath).Elements().Select(p => GetOrder(p))
       //: XmlTools.LoadListFromXMLElement(orderPath).Elements().Select(p => GetOrder(p)).Where(filter);

    public DO.Order GetByFilter(Func<DO.Order?, bool>? filter)
    {
        List<DO.Order?> orderList = GetAll().ToList();

        return (from item in orderList
                where filter(item)
                select (DO.Order)item).FirstOrDefault();
    }

    public DO.Order GetById(int id)
    //{
            =>XmlTools.LoadListFromXMLSerializer<DO.Order>(orderPath).FirstOrDefault(p => p.ID == id);
        //List<DO.Order?> orderList = GetAll().ToList();

        //return (from item in orderList
        //        where item!=null && item?.ID==id
        //        select (DO.Order)item).FirstOrDefault();
        //throw new DalApi.IdNotExistException("the order requested does not exist");
    //}


}
