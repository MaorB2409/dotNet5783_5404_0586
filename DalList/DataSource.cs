using DO;
using System.Collections.Generic;
namespace DalApi;

internal class DataSource
{
    internal static DataSource s_instance { get; }//property that returns a copy of data
    static readonly Random randNum = new Random();//rand number pick
    public List<Order?> orderList { get; set; } = new List<Order?> ();//list of orders
    public List<OrderItem?> orderItemList { get; set; } = new List<OrderItem?> ();//list of orderItems
    public List<Product?> productList { get; set; } = new List<Product?> ();//list of products
    public DataSource() { s_Initialize(); }//private ctor that calls initialize on all the data
    static DataSource() => s_instance = new DataSource();
    internal static class Config//internal class
    {
        internal const int s_startOrderNumber = 1000;//order number starts at 1000
        public static int s_nextOrderNumber = s_startOrderNumber;//next order number is now order number 
        internal static int NextOrderNumber { get => ++s_nextOrderNumber; }//change the value of next order for the next order
        internal const int s_startProductNumber = 100000;//product number starts at 0
        public static int s_nextProductNumber = s_startProductNumber;//next Product number is now Product number 
        internal static int NextProductNumber { get => ++s_nextProductNumber; }//change the value of next Product for the next Product
        internal const int s_startOrderItemNumber = 0;//OrderItem number starts at 0
        public static int s_nextOrderItemNumber = s_startOrderItemNumber;//next OrderItem number is now OrderItem number 
        internal static int NextOrderItemNumber { get => ++s_nextOrderItemNumber; }//change the value of next OrderItem for the next OrderItem

    }

    void s_Initialize()//creates new lists for the products etc
    {
        CreateProducts();//func to create a product
        CreateOrders();//func to create a order
        CreateOrderItem();//func to create a orderItems
    }
    private void CreateProducts()//func to create a product 
    {
        string[] NameOfFurniture = { "Couch", "Table", "Bed", "Chair", "Closet", "BookCase" };
        for (int i = 0; i < 10; i++)
        {
            productList.Add(
                new()
                {
                    ID = Config.NextProductNumber,
                    Price = randNum.Next(100, 2500),
                    Name = NameOfFurniture[randNum.Next(NameOfFurniture.Length)],
                    Category = (Enums.Category)randNum.Next(0, 6),
                    InStock = randNum.Next(19, 48),
                    IsDeleted = false
                });
        }

    }
    private void CreateOrders()//func to create a order
    {
        #region arrays:customerName,customerEmail, and address.
        string[] customerName = { "Haim", "Hagit", "Maor", "Chen", "Avihu", "Avinoam", "Yonatan", "Binyamin", "Yoni", "Batya", "Shana", "Sara", "Avraham", "Talia", "Shalom", "Asher", "Harry", "Yosef", "Rina", "Aron" };
        string[] customerEmail = {"aa@gamil.com", "bb@gamil.com", "cc@gamil.com", "dd@gamil.com", "ee@gamil.com", "ff@gamil.com", "gg@gamil.com", "hh@gamil.com",  "ii@gamil.com", "jj@gamil.com", "kk@gamil.com",
        "ll@gamil.com", "mm@gamil.com", "oo@gamil.com", "pp@gamil.com", "qq@gamil.com", "rr@gamil.com", "ss@gamil.com","tt@gamil.com"};
        string[] address = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t" };

        for (int i = 0; i < 20; i++)
        {
            Order myOrder = new()
            {
                ID = Config.NextOrderNumber,
                CostumerName = customerName[randNum.Next(customerName.Length)],
                CostumerEmail = customerEmail[randNum.Next(customerEmail.Length)],
                CostumerAddress = address[randNum.Next(address.Length)],
                IsDeleted = false,
                OrderDate = DateTime.Now - new TimeSpan(randNum.Next(60), randNum.Next(60), randNum.Next(24))
            };
            //myOrder.OrderDate=DateTime.Now-new TimeSpan();//we need to check percentages
            if(i>10 && i< 15)//5 אובייקטים נאתחל שנשלחו אבל לא הגיעו עדיין
            {
                myOrder.ShipDate = myOrder.OrderDate + new TimeSpan(randNum.Next(60), randNum.Next(60), randNum.Next(24), randNum.Next(3));
                myOrder.DeliveryDate = null;
            }
            else
            {
                myOrder.ShipDate = myOrder.OrderDate + new TimeSpan(randNum.Next(60), randNum.Next(60), randNum.Next(24), randNum.Next(3));
                myOrder.DeliveryDate = myOrder.ShipDate + new TimeSpan(randNum.Next(60), randNum.Next(60), randNum.Next(24), randNum.Next(3));
            }

            orderList.Add(myOrder);//add the order to the list
        }

    }
    private void CreateOrderItem()//func to create a orderItems
    {
        for (int i = 0; i < 40; i++)
        {
            Product? product = productList[randNum.Next(productList.Count)];
            orderItemList.Add(
                new OrderItem
                {
                    ID = Config.NextOrderItemNumber,
                    ProductID = product?.ID ?? throw new Exception(),
                    OrderID = randNum.Next(Config.s_startOrderNumber, Config.s_startOrderNumber + orderList.Count),
                    Price = (double)product?.Price!,
                    IsDeleted = false,
                    Amount = randNum.Next(5)
                });
        }
    }



}
#endregion