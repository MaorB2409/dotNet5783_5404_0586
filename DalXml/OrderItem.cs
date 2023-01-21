using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dal;
using DalApi;
using DO;
using global::Dal.Dal;
using System.Xml.Linq;
using static DataSource;

internal class OrderItem : IOrderItem
{
    string orderItemPath = @"OrderItem.xml";
    string configPath = @"Config.xml";
    public int Add(DO.OrderItem order)
    {
        var listOrders = XmlTools.LoadListFromXMLSerializer<DO.OrderItem>(orderItemPath);

        if (listOrders.Exists(lec => lec.ID == order.ID && lec.IsDeleted == false))
            throw new IdExistException("order item already exists");

        var runningList = XmlTools.LoadListFromXMLSerializer<RunningNumber>(configPath);


        RunningNumber runningNum = runningList.FirstOrDefault(num => num.typeOfnumber == "Order item running number");

        runningList.Remove(runningNum);
        runningNum.numberSaved++;

        order.ID = (int)runningNum.numberSaved;

        listOrders.Add(order);
        runningList.Add(runningNum);

        XmlTools.SaveListToXMLSerializer(listOrders, orderItemPath);
        XmlTools.SaveListToXMLSerializer(runningList, configPath);

        return order.ID;


    }

    public void Delete(int id)
    {
        var listOrders = XmlTools.LoadListFromXMLSerializer<DO.OrderItem>(orderItemPath);

        var o = listOrders.FirstOrDefault(p => p.ID == id);

        if (o.IsDeleted)
            throw new IdNotExistException("The order item does not exist");

        listOrders.Remove(o);

        DO.OrderItem order = new()
        {
            ID = id,
            ProductID = o.ProductID,
            OrderID = o.OrderID,
            Price = o.Price,
            Amount = o.Amount
        };

        listOrders.Add(order);
        XmlTools.SaveListToXMLSerializer(listOrders, orderItemPath);
    }
    public static DO.OrderItem? GetOrderItem(XElement p) =>
 p?.ToInt("ID") is null ? null : new DO.OrderItem()
 {
     ID = p?.ToInt("ID") ?? 0,
     ProductID = p?.ToInt("ProductID") ?? 0,
     OrderID = p?.ToInt("OrderID") ?? 0,
     Amount = p.ToInt("Amount") ?? 0,
     Price = p?.ToDoubleNullable("Price") ?? 0
 };

    public IEnumerable<DO.OrderItem?> GetAll(Func<DO.OrderItem?, bool>? filter = null)
      =>
       filter is null
       ? XmlTools.LoadListFromXMLElement(orderItemPath).Elements().Select(p => GetOrderItem(p))
       : XmlTools.LoadListFromXMLElement(orderItemPath).Elements().Select(p => GetOrderItem(p)).Where(filter);


    public DO.OrderItem GetByFilter(Func<DO.OrderItem?, bool>? filter)
    {
        List<DO.OrderItem?> orderItemList = GetAll().ToList();

        return (from item in orderItemList
                where filter(item)
                select (DO.OrderItem)item).FirstOrDefault();
    }

    public DO.OrderItem GetById(int id) =>
        XmlTools.LoadListFromXMLSerializer<DO.Order?>(orderItemPath).FirstOrDefault(p => p.ID == id);
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

    public void Update(DO.OrderItem order)
    {
        var listOrders = XmlTools.LoadListFromXMLSerializer<DO.OrderItem>(orderItemPath);
        var o = listOrders.FirstOrDefault(p => p.ID == order.ID);
        listOrders.Remove(o);
        listOrders.Add(order);
        XmlTools.SaveListToXMLSerializer(listOrders, orderItemPath);
    }
}
