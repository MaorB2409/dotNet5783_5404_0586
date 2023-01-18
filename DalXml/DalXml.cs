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
        public static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }
        static DalXml() {
            List<RunningNumber> configs = new()
            {
                new RunningNumber(){numberSaved=DataSource.Config.s_startOrderItemNumber,typeOfnumber="OrderItem running number"},
                new RunningNumber(){numberSaved=DataSource.Config.s_startOrderNumber,typeOfnumber="Order running number"}
            };
            XmlTools.SaveListToXMLSerializer<RunningNumber>(configs, configPath);
            foreach (var item in DataSource.s_instance.productList)
            {
                CreateProduct((DO.Product)item!);
            }
            XmlTools.SaveListToXMLSerializer<DO.Order?>(DataSource.s_instance.orderList, orderPath);
            XmlTools.SaveListToXMLSerializer<DO.OrderItem?>(DataSource.s_instance.orderItemList, orderItemPath);
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
        
        #endregion
        public IProduct Product { get; } = new Dal.Product();
        public IOrder Order { get; } = new Dal.Order();
        public IOrderItem OrderItem { get; } = new Dal.OrderItem();

    }
}
