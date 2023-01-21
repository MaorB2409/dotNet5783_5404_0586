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


        RunningNumber runningNum = runningList.FirstOrDefault(num => num.typeOfnumber == "OrderItem running number");

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

    public IEnumerable<DO.OrderItem?> GetAll(Func<DO.OrderItem?, bool>? filter = null)
    {
        var listOrders = XmlTools.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath)!;
        return filter == null ? listOrders.OrderBy(lec => ((DO.OrderItem)lec!).ID)
                              : listOrders.Where(filter).OrderBy(lec => ((DO.OrderItem)lec!).ID);
    }

    public DO.OrderItem GetByFilter(Func<DO.OrderItem?, bool>? filter)
    {
        List<DO.OrderItem?> orderItemList = GetAll().ToList();

        return (from item in orderItemList
                where filter(item)
                select (DO.OrderItem)item).FirstOrDefault();
    }

    public DO.OrderItem GetById(int id) =>
        XmlTools.LoadListFromXMLSerializer<DO.OrderItem>(orderItemPath).FirstOrDefault(p => p.ID== id);
    public DO.OrderItem ItemOfOrder(int id, int productId)
    {
        List<DO.OrderItem?> orderItemList = GetAll().ToList();

        return (from item in orderItemList
                where item != null && item?.ProductID == productId && item?.OrderID == id
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
    //   string orderItemPath = @"OrderItem.xml";
    //   string configPath = @"Config.xml";
    //   public int Add(DO.OrderItem item)
    //   {
    //       XElement orderItemRoot = XmlTools.LoadListFromXMLElement(orderItemPath); //get all the elements from the file

    //       //check if the orderItem exists in th file
    //       var orderItemFromFile = (from oi in orderItemRoot.Elements()
    //                                where (oi != null && oi.Element("ID")!.Value == item.ID.ToString())
    //                                select oi).FirstOrDefault();

    //       //throw an exception
    //       if (orderItemFromFile != null)
    //           throw new DalApi.IdExistException("the order already exists");

    //       //get running order item ID number
    //       List<RunningNumber> runningList = XmlTools.LoadListFromXMLSerializer<RunningNumber>(configPath);

    //       var runningNum = (from number in runningList
    //                         where (number.typeOfnumber == "Order item running number")
    //                         select number).FirstOrDefault();
    //       runningList.Remove(runningNum);//remove the saved number from list
    //       runningNum.numberSaved++;//add one to the saved number
    //       runningList.Add(runningNum);//add the number back to list
    //       int temp = (int)runningNum.numberSaved;//save the running number



    //       //add the orderItem to the root element
    //       orderItemRoot.Add(
    //           new XElement("OrderItem",
    //           new XElement("ID", temp),
    //           new XElement("ProductID", item.ProductID),
    //           new XElement("OrderID", item.OrderID),
    //           new XElement("Price", item.Price),
    //           new XElement("Amount", item.Amount)));

    //       //save the root in the file
    //       XmlTools.SaveListToXMLElement(orderItemRoot, orderItemPath);
    //       return item.ID;
    //   }

    //   public void Delete(int id)
    //   {
    //       List<DO.OrderItem?> orderItemList = XmlTools.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath);

    //       DO.OrderItem temp = (from item in orderItemList
    //                            where item != null && item?.ID == id
    //                            select (DO.OrderItem)item).FirstOrDefault();

    //       if (temp.ID.Equals(default(Order)))
    //           throw new DalApi.IdNotExistException("the product does not exist");

    //       orderItemList.Remove(temp);

    //       XmlTools.SaveListToXMLSerializer(orderItemList, orderItemPath);
    //   }
    //   public static DO.OrderItem? GetOrderItem(XElement p) =>
    //p?.ToInt("ID") is null ? null : new DO.OrderItem()
    //{
    //    ID = p?.ToInt("ID") ?? 0,
    //    ProductID = p?.ToInt("ProductID") ?? 0,
    //    OrderID = p?.ToInt("OrderID") ?? 0,
    //    Amount = p.ToInt("Amount") ?? 0,
    //    Price = p?.ToDoubleNullable("Price") ?? 0
    //};

    //   public IEnumerable<DO.OrderItem?> GetAll(Func<DO.OrderItem?, bool>? filter = null)
    //     =>
    //      filter is null
    //      ? XmlTools.LoadListFromXMLElement(orderItemPath).Elements().Select(p => GetOrderItem(p))
    //      : XmlTools.LoadListFromXMLElement(orderItemPath).Elements().Select(p => GetOrderItem(p)).Where(filter);


    //   public DO.OrderItem GetByFilter(Func<DO.OrderItem?, bool>? filter)
    //   {
    //       List<DO.OrderItem?> orderItemList = GetAll().ToList();

    //       return (from item in orderItemList
    //               where filter(item)
    //               select (DO.OrderItem)item).FirstOrDefault();
    //   }

    //   public DO.OrderItem GetById(int id)
    //   {
    //       List<DO.OrderItem?> orderItemList = GetAll().ToList();

    //       return (from item in orderItemList
    //               where item != null && item?.ID == id
    //               select (DO.OrderItem)item).FirstOrDefault();
    //       throw new DalApi.IdNotExistException("the Order Item requested does not exist");
    //   }

    //   public DO.OrderItem ItemOfOrder(int id, int productId)
    //   {
    //       List<DO.OrderItem?> orderItemList = GetAll().ToList();

    //       return (from item in orderItemList
    //               where item != null && item?.ProductID == productId && item?.OrderID==id
    //               select (DO.OrderItem)item).FirstOrDefault();
    //       throw new DalApi.IdNotExistException("the Order Item requested does not exist");
    //   }

    //   public IEnumerable<DO.OrderItem?> ItemsInOrder(int id)
    //   {
    //       List<DO.OrderItem?> orderItemList = GetAll().ToList();

    //       return (from item in orderItemList
    //               where item?.OrderID == id
    //               select item).ToList();
    //   }

    //   public void Update(DO.OrderItem item)
    //   {
    //       DO.OrderItem? temp = GetById(item.ID);//get the order item requested to update 
    //       List<DO.OrderItem?> orderItemList = GetAll().ToList();//get all order items from ile
    //       orderItemList.Remove(temp);//remove the existing order item
    //       orderItemList.Add(item);//add the updated order item

    //       XmlTools.SaveListToXMLSerializer(orderItemList, orderItemPath);//save back into file    
    //   }
}
