using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;


namespace Dal
{
    public struct RunningNumber
    {
        public double numberSaved { get; set; }
        public string typeOfnumber { get; set; }
    }
    sealed internal class DalXml : IDal
    {
        static string configPath = "Config.xml";
        static string orderPath = "Order.xml";
        static string orderItemPath = "OrderItem.xml";
        static string productPath = "Product.xml";

        #region singlton
        public static IDal Instance { get; } = new DalXml();

        //public static readonly IDal instance = new DalXml();
        //public static IDal Instance { get => instance; }
        static DalXml() {
            List<RunningNumber> configs = new()
            {
                new RunningNumber(){numberSaved=40,typeOfnumber="OrderItem running number"},
                new RunningNumber(){numberSaved=1020,typeOfnumber="Order running number"},
                new RunningNumber(){numberSaved=1010,typeOfnumber="Product ID running number"}
            };
            XmlTools.SaveListToXMLSerializer<RunningNumber>(configs, configPath);
            foreach (var item in DataSource.s_instance.productList)
            {
                CreateProduct((DO.Product)item!);
            }
            XmlTools.SaveListToXMLSerializer<DO.Order?>(DataSource.s_instance.orderList, orderPath);
            XmlTools.SaveListToXMLSerializer<DO.OrderItem?>(DataSource.s_instance.orderItemList, orderItemPath);
            //foreach (var item in DataSource.s_instance.orderList)
            //{
            //    CreateOrder((DO.Order)item!);
            //}
            //foreach (var item in DataSource.s_instance.orderItemList)
            //{
            //    CreateOrderItem((DO.OrderItem)item!);
            //}
        }
        //static DalXml() { }
        static public void CreateProduct(DO.Product prod)
        {
            XElement productRoot = XmlTools.LoadListFromXMLElement(productPath);
            var productFromFile = (from product in productRoot.Elements()
                                   where (product.Element("ID")?.Value == prod.ID.ToString())
                                   select product).FirstOrDefault();
            if (productFromFile != null)
                throw new IdExistException("the product already exists");
            productRoot.Add(
                new XElement("Product",
                new XElement("ID", prod.ID),
                new XElement("Name", prod.Name),
                new XElement("Price", prod.Price),
                new XElement("InStock", prod.InStock),
                new XElement("Category", prod.Category)));
            XmlTools.SaveListToXMLElement(productRoot, productPath);
        }



        //static public void CreateOrder(DO.Order ord)
        //{
        //    XElement orderRoot = XmlTools.LoadListFromXMLElement(orderPath);
        //    var orderFromFile = (from order in orderRoot.Elements()
        //                           where (order.Element("ID")?.Value == ord.ID.ToString())
        //                           select order).FirstOrDefault();
        //    if (orderFromFile != null)
        //        throw new IdExistException("the product already exists");
        //    orderRoot.Add(
        //        new XElement("Order",
        //        new XElement("ID", ord.ID),
        //        new XElement("CostumerName", ord.CostumerName),
        //        new XElement("CostumerEmail", ord.CostumerEmail),
        //        new XElement("CostumerAddress", ord.CostumerAddress),
        //        new XElement("OrderDate", ord.OrderDate),
        //        new XElement("ShipDate", ord.ShipDate),
        //        new XElement("DeliveryDate", ord.DeliveryDate)
        //        ));
        //    XmlTools.SaveListToXMLElement(orderRoot, orderPath);
        //}


        //static public void CreateOrderItem(DO.OrderItem o)
        //{
        //    XElement orderItemRoot = XmlTools.LoadListFromXMLElement(orderItemPath);
        //    var orderItemFromFile = (from ord in orderItemRoot.Elements()
        //                             where (ord.Element("ID")?.Value == o.ID.ToString())
        //                             select ord).FirstOrDefault();
        //    if (orderItemFromFile != null)
        //        throw new IdExistException("the product already exists");
        //    orderItemRoot.Add(
        //        new XElement("Product",
        //        new XElement("ID", o.ID),
        //        new XElement("ProductID", o.ProductID),
        //        new XElement("OrderID", o.OrderID),
        //        new XElement("Price", o.Price),
        //        new XElement("Amount", o.Amount)));
        //    XmlTools.SaveListToXMLElement(orderItemRoot, orderItemPath);
        //}
        #endregion
        public IProduct Product { get; } = new Dal.Product();
        public IOrder Order { get; } = new Dal.Order();
        public IOrderItem OrderItem { get; } = new Dal.OrderItem();

    }
}
