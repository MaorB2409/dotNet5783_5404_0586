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
        string configPath = "Config.xml";
        string orderPath = "Order.xml";
        string orderItemPath = "OrderItem.xml";
        string productPath = "Product.xml";

        #region singlton
        public static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }
        DalXml()
        {

        }
        #endregion
        public IProduct Product { get; } = new Dal.Product();
        public IOrder Order { get; } = new Dal.Order();
        public IOrderItem OrderItem { get; } = new Dal.OrderItem();

    }
}
