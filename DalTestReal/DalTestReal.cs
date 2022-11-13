

using Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using static DO.Enums;

namespace DalTestReal
{
    class Program
    {
        static void Main(string[] args)
        {//class that can go to data source
            DalOrder dalOrder = new();
            DalOrderItem dalOrderItem = new();
            DalProduct dalProduct = new();
            Order order = new();
            OrderItem orderItem = new();
            Product product = new();

            while (true)
            {
                Console.WriteLine("Hello! \n" +
                    "Choose one of the following options:\n" +
                    "For actions on a Product, please click 1:\n" +
                    "For actions on an Order, please click 2:\n" +
                    "For actions on an OrderItem, please click 3:\n");

                int num;
                while (!System.Int32.TryParse(Console.ReadLine(), out num)) ;
                num--;//since enum starts at number 1, we substract 1 to make it run corectly
                Enums.Type type = (Enums.Type)num;

                if (num == -1) { Console.WriteLine("bye bye"); return; }

                Console.WriteLine("To add please click 1:\n" +
                            "To delete please click 2:\n" +
                            "To update please click 3:\n" +
                            "To get please click 4:\n" +
                            "To get all the list please click 5:\n");

                while (!System.Int32.TryParse(Console.ReadLine(), out num)) ;
                num--;
                Enums.Action action = (Enums.Action)num;

                switch (type, action)
                {
                    case (Enums.Type.Product, Enums.Action.Add)://product,add
                        try
                        {
                            product = ProductFunciton.Add();
                            Console.WriteLine("your new product ID is: " + dalProduct.Add(product) + "\n");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The list is full\n");
                        }
                        break;

                    case (Enums.Type.Product, Enums.Action.Del):
                        try
                        {
                            dalProduct.Delete(GetNumberFromUser("Enter product ID:\n"));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The object is not found\n");
                        }
                        break;

                    case (Enums.Type.Product, Enums.Action.Update):
                        try
                        {
                            product = ProductFunciton.Update();
                            dalProduct.Update(product);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The object is not found\n");
                        }
                        break;

                    case (Enums.Type.Product, Enums.Action.get):
                        try
                        {
                            Console.WriteLine(dalProduct.GetById(GetNumberFromUser("Enter ID\n")));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("the object is not faund\n");
                        }
                        break;

                    case (Enums.Type.Product, Enums.Action.GetList):
                        try
                        {
                            ProductFunciton.printList(dalProduct.GetAll());
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The list is empty\n");
                        }
                        break;

                    case (Enums.Type.Order, Enums.Action.Add):
                        try
                        {
                            order = OrderFunciton.Add();
                            Console.WriteLine("your new order ID is: " + dalOrder.Add(order) + "\n");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The list is full");
                        }
                        break;

                    case (Enums.Type.Order, Enums.Action.Del):
                        try
                        {
                            dalOrder.Delete(GetNumberFromUser("Enter Order ID: "));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The object is not found");
                        }
                        break;

                    case (Enums.Type.Order, Enums.Action.Update):
                        try
                        {
                            order = OrderFunciton.Update();
                            dalOrder.Update(order);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("the object is not faund");
                        }
                        break;

                    case (Enums.Type.Order, Enums.Action.get):
                        try
                        {
                            Console.WriteLine(dalOrder.GetById(GetNumberFromUser("Enter ID\n")));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("the object is not faund");
                        }
                        break;

                    case (Enums.Type.Order, Enums.Action.GetList):
                        try
                        {
                            OrderFunciton.printList(dalOrder.GetAll());
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("the list is empty");
                        }
                        break;

                    case (Enums.Type.OrderItem, Enums.Action.Add):
                        try
                        {
                            orderItem = OrderItemFunction.Add();
                            Console.WriteLine("your new order item ID is: " + dalOrderItem.Add(orderItem));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The list is full");
                        }
                        break;

                    case (Enums.Type.OrderItem, Enums.Action.Del):
                        try
                        {
                            dalOrderItem.Delete(GetNumberFromUser("Enter Order item ID: "));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The object is not found");
                        }
                        break;

                    case (Enums.Type.OrderItem, Enums.Action.Update):
                        try
                        {
                            orderItem = OrderItemFunction.Update();
                            dalOrderItem.Update(orderItem);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The object is not found");
                        }
                        break;

                    case (Enums.Type.OrderItem, Enums.Action.get):
                        try
                        {
                            Console.WriteLine(dalOrderItem.GetById(GetNumberFromUser("Enter ID\n")));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The object is not found");
                        }
                        break;

                    case (Enums.Type.OrderItem, Enums.Action.GetList):
                        try
                        {
                            OrderItemFunction.PrintList(dalOrderItem.GetAll());
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The list is empty");
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// class with all funcs on objects
        /// </summary>
        internal class ProductFunciton
        {
            /// <summary>
            /// adding object
            /// </summary>
            /// <returns></returns>
            static internal Product Add()
            {
                Product product = new Product();
                Console.WriteLine("enter product name:\n");
                product.Name = Console.ReadLine() ?? "";
                product.Price = GetNumberFromUser("enter product price:\n");
                product.Category = (Enums.Category)GetNumberFromUser("enter product category:\n");
                product.InStock = GetNumberFromUser("enter product stock\n");
                return product;
            }
            /// <summary>
            /// changing detail of product
            /// </summary>
            /// <returns></returns>
            static internal Product Update()
            {
                Product product = new();
                product.ID = GetNumberFromUser("enter product ID:\n");
                Console.WriteLine("enter product name:\n");
                product.Name = Console.ReadLine() ?? "";
                product.Price = GetNumberFromUser("enter product price:\n");
                product.Category = (Enums.Category)GetNumberFromUser("enter product category:\n");
                product.InStock = GetNumberFromUser("enter product stock:\n");
                return product;
            }
            /// <summary>
            /// prints all objects in array
            /// </summary>
            /// <param name="products"></param>
            static internal void printList(IEnumerable<Product> products)
            {
                foreach (Product i in products)
                {
                    Console.WriteLine(i);
                }//print the list
            }

        }
        /// <summary>
        /// class to all funcs of order item
        /// </summary>
        internal class OrderItemFunction
        {
            /// <summary>
            /// add item to a order
            /// </summary>
            /// <returns></returns>
            static internal OrderItem Add()
            {
                OrderItem orderItem = new OrderItem();
                DalOrder dalOrder = new DalOrder();
                DalProduct dalProduct = new DalProduct();
                bool check = true;
                while (check)
                {
                    try
                    {
                        orderItem.ProductID = GetNumberFromUser("enter product ID:\n");
                        dalProduct.GetById(orderItem.ProductID);
                        check = false;//if not found
                    }
                    catch
                    {
                        Console.WriteLine("the product id is not found let's try again\n");
                    }
                }

                check = true;//if found

                while (check)
                {
                    try
                    {
                        orderItem.OrderID = GetNumberFromUser("enter order id:\n");
                        dalOrder.GetById(orderItem.OrderID);
                        check = false;
                    }
                    catch
                    {
                        Console.WriteLine("the order id are not found let's try again\n");
                        OrderFunciton.Add();
                    }
                }
                orderItem.Amount = GetNumberFromUser("enter amount:\n");
                orderItem.Price = dalProduct.GetById(orderItem.ProductID).Price * orderItem.Amount;

                return orderItem;
            }
            /// <summary>
            /// change details of item
            /// </summary>
            /// <returns></returns>
            static internal OrderItem Update()
            {
                OrderItem orderItem = new();
                DalOrder dalOrder = new();
                DalProduct dalProduct = new();
                DalOrderItem dalOrderItem = new();
                bool check = true;

                while (check)//if found
                {
                    try
                    {
                        orderItem.ID = GetNumberFromUser("Enter order item ID\n");
                        dalOrderItem.GetById(orderItem.ID);
                        check = false;//not found
                    }
                    catch
                    {
                        Console.WriteLine("the order item id is not faund let's try again\n");
                    }
                }

                check = true;

                while (check)
                {
                    try
                    {
                        orderItem.ProductID = GetNumberFromUser("enter product ID:\n");
                        dalProduct.GetById(orderItem.ProductID);
                        check = false;
                    }
                    catch
                    {
                        Console.WriteLine("the product id is not faund let's try again\n");
                    }
                }

                check = true;

                while (check)
                {
                    try
                    {
                        orderItem.OrderID = GetNumberFromUser("enter order id:\n");
                        dalOrder.GetById(orderItem.OrderID);
                        check = false;
                    }
                    catch
                    {
                        Console.WriteLine("the order id are not faund let's try again\n");
                        OrderFunciton.Add();
                    }
                }

                orderItem.Amount = GetNumberFromUser("enter amount:\n");
                orderItem.Price = dalProduct.GetById(orderItem.ProductID).Price * orderItem.Amount;

                return orderItem;
            }
            /// <summary>
            /// print all items
            /// </summary>
            /// <param name="orderItems"></param>
            static internal void PrintList(IEnumerable<OrderItem> orderItems)
            {
                foreach (OrderItem i in orderItems)
                {
                    Console.WriteLine(i);
                }//print the list
            }

        }
        internal class OrderFunciton
        {
            /// <summary>
            /// add new order
            /// </summary>
            /// <returns></returns>
            static internal Order Add()
            {
                Order order = new Order();
                Console.WriteLine("enter customer name:\n");
                order.CostumerName = Console.ReadLine() ?? ""; 
                Console.WriteLine("enter customer mail:\n");
                order.CostumerEmail = Console.ReadLine() ?? ""; 
                Console.WriteLine("enter customer adress:\n");
                order.CostumerAddress = Console.ReadLine() ?? ""; 
                order.OrderDate = DateTime.Now;
                order.ShipDate = DateTime.MinValue;
                order.DeliveryDate = DateTime.MinValue;
                return order;
            }
            /// <summary>
            /// update an order details (not products)
            /// </summary>
            /// <returns></returns>
            static internal Order Update()
            {
                Order order = new();
                order.ID = GetNumberFromUser("enter order ID:\n");
                Console.WriteLine("enter customer name:\n");
                order.CostumerName = Console.ReadLine() ?? ""; 
                Console.WriteLine("enter customer mail:\n");
                order.CostumerEmail = Console.ReadLine() ?? ""; 
                Console.WriteLine("enter customer adress\n");
                order.CostumerAddress = Console.ReadLine() ?? ""; 
                order.OrderDate = DateTime.Now;
                order.ShipDate = DateTime.MinValue;
                order.DeliveryDate = DateTime.MinValue;
                return order;
            }
            /// <summary>
            /// print all the array
            /// </summary>
            /// <param name="order"></param>
            static internal void printList(IEnumerable<Order> order)
            {
                foreach (Order i in order)
                {
                    Console.WriteLine(i);
                }//print the list
            }

        }
        /// <summary>
        /// class with all funcs for order
        /// </summary>
        /// 



        /// <summary>
        /// a func to get number from user (and makes sure its not letters)
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        static int GetNumberFromUser(string txt)//the programer sends what he needs
        {
            Console.WriteLine(txt);
            int num;
            while (!System.Int32.TryParse(Console.ReadLine(), out num))//if not int
            {
                Console.WriteLine("ERROR format\n");//error
            }
            return num;//read users number
        }


    }
}
