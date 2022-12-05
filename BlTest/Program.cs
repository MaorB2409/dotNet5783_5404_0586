using BlApi;
using BlImplementation;
using BO;
using DO;
using System;
namespace BlTest;
class Program
{
    static IBl bl = new Bl();
    static void Main(string[] args)
    {
        Cart? cart = new() { orderItems = new List<BO.OrderItem?>() };//new cart
        BO.Product? p = new();
        BO.Order? o = new();
        int id,cat;
        while (true)
        {
            Console.WriteLine("Hello! \n" +
                "Choose one of the following options:\n" +
                "For actions on Cart, please click 1:\n" +
                "For actions on an Order, please click 2:\n" +
                "For actions on a Product, please click 3:\n");

            int num;
            while (!System.Int32.TryParse(Console.ReadLine(), out num)) ;
            num--;//since enum starts at number 1, we substract 1 to make it run corectly
            BO.Enums.Type type = (BO.Enums.Type)num;

            if (num == -1) { Console.WriteLine("bye bye"); return; }

            int choice = 0;
            switch (type)
            {
                case BO.Enums.Type.Cart:
                    Console.WriteLine("To add please click 1:\n" +
                      "To update please click 2:\n" +
                      "To place order please click 3:\n" +
                      "To return to main menu please click 4:\n");
                    choice=0;
                    System.Int32.TryParse(Console.ReadLine(), out choice);
                    choice--;//case starts at 0
                    switch (choice)
                    {
                        case 0:
                            id = GetNumberFromUser("please enter your product id:");
                            try
                            {
                                Console.WriteLine(bl.Cart.AddToCart(cart, id));
                            }
                            catch (UnfoundException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 1:
                            id = GetNumberFromUser("please enter your product id:");
                            int amount = GetNumberFromUser("please enter how much to add or reduce:");
                            try
                            {
                                cart = bl.Cart.UpdateCart(cart, id, amount);
                            }

                            catch (UnfoundException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (IdNotExistException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 2:

                            Console.WriteLine("enter name");
                            string name = Console.ReadLine()??"";

                            Console.WriteLine("enter mail");
                            string mail = Console.ReadLine()??"";

                            Console.WriteLine("enter address");
                            string address = Console.ReadLine() ?? "";

                            try
                            {
                                bl.Cart.MakeOrder(cart, name, mail, address);
                                Console.WriteLine("cart aproved!");
                            }
                            catch (UnfoundException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (BO.Exceptions e)
                            {
                                Console.WriteLine(e.Message);
                            }

                            break;
                        case 3:
                            Console.WriteLine("back to main menu...");

                            break;
                        default:
                            Console.WriteLine("wrong number");
                            break;
                    }

                    break;
                case BO.Enums.Type.Order:
                    Console.WriteLine("To get list of orders please click 1:\n" +
                      "To get order info please click 2:\n" +
                      "To update order ship date please click 3:\n" +
                      "To update order delivery date please click 4:\n" +
                      "To track order please click 5:\n" +
                      "To return to main menu please click 6:\n");
                    choice = 0;
                    System.Int32.TryParse(Console.ReadLine(), out choice);
                    choice--;//case starts at 0
                    switch (choice)
                    {
                        case 0:
                            try
                            {
                                printList<BO.OrderForList?>(bl.Order.GetAllOrderForList());
                            }
                            catch (UnfoundException e)//for inner status func
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 1:
                            id = GetNumberFromUser("Please enter order id:");
                            try
                            {
                                Console.WriteLine(bl.Order.GetBoOrder(id));
                            }
                            catch (IdNotExistException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 2:
                            id = GetNumberFromUser("Please enter order id:");
                            try
                            {
                                Console.WriteLine(bl.Order.ShipUpdate(id));
                            }
                            catch (UnfoundException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (IdNotExistException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 3:
                            id = GetNumberFromUser("Please enter order id:");
                            try
                            {
                                Console.WriteLine(bl.Order.DeliveredUpdate(id));
                            }
                            catch (UnfoundException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (IdNotExistException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 4:
                            id = GetNumberFromUser("Please enter order id:");
                            try
                            {
                                Console.WriteLine(bl.Order.GetOrderTracking(id));
                            }
                            catch (UnfoundException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 5:
                            Console.WriteLine("back to main menu...");
                            break;
                        default:
                            Console.WriteLine("wrong number");
                            break;
                    }
                    break;
                case BO.Enums.Type.Product:
                    Console.WriteLine("To get list of products for manager please click 1:\n" +
                      "To get a products info for manager please click 2:\n" +
                      "To add a product for manager please click 3:\n" +
                      "To delete a product for manager please click 4:\n" +
                      "To update product for manager please click 5:\n" +
                      "To get catalog for customer please click 6:\n" +
                      "To return to main menu please click 7:\n"
                      );
                    choice = 0;
                    System.Int32.TryParse(Console.ReadLine(), out choice);
                    choice--;//case starts at 0
                    switch (choice)
                    {
                        case 0:
                            printList<ProductForList?>(bl.Product.GetProductsForList());
                            break;
                        case 1:
                            try
                            {
                                Console.WriteLine(bl.Product.ManagerProduct(GetNumberFromUser("Enter Product ID: ")));
                            }
                            catch (BO.IdNotExistException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 2:
                            //Console.WriteLine("Enter ID of new product:");
                            //p.ID = GetNumberFromUser();
                            Console.WriteLine("Enter category of new product:");
                            cat = GetNumberFromUser();
                            p.Category = (BO.Enums.Category)cat;
                            Console.WriteLine("Enter name of product:");
                            p.Name = Console.ReadLine();
                            Console.WriteLine("Enter in stock:");
                            p.InStock = GetNumberFromUser();
                            Console.WriteLine("Enter price of Product");
                            p.Price = GetNumberFromUser();
                            p.IsDeleted = false;
                            try
                            {
                                bl.Product.AddProduct(p);
                            }

                            catch (IdExistException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (IncorrectInput e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 3:
                            id = GetNumberFromUser("Please enter product ID:");
                            try
                            {
                                bl.Product.DeleteProduct(id);
                            }
                            catch (UnfoundException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 4:
                            Console.WriteLine("Enter ID of new product:");
                            p.ID = GetNumberFromUser();
                            Console.WriteLine("Enter category of new product:");
                            cat = GetNumberFromUser();
                            p.Category = (BO.Enums.Category)cat;
                            Console.WriteLine("Enter name of product:");
                            p.Name = Console.ReadLine();
                            Console.WriteLine("Enter in stock:");
                            p.InStock = GetNumberFromUser();
                            Console.WriteLine("Enter price of Product");
                            p.Price = GetNumberFromUser();
                            p.IsDeleted = false;
                            try
                            {
                                bl.Product.UpdateProduct(p);
                            }

                            catch (IdExistException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (BO.IncorrectInput e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 5:
                            printList<ProductForList?>(bl.Product.GetProductsForList());
                            break;
                        case 6:
                            Console.WriteLine("back to main menu...");
                            break;
                        default:
                            Console.WriteLine("wrong number");
                            break;
                    }
                    break;


            }
        }
    }


    /// <summary>
    /// a func to get number from user (and makes sure its not letters)
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    static int GetNumberFromUser(string txt = "")//the programer sends what he needs
    {
        Console.WriteLine(txt);
        int num;
        while (!System.Int32.TryParse(Console.ReadLine(), out num))//if not int
        {
            Console.WriteLine("ERROR format\n");//error
        }
        return num;//read users number
    }

    static void printList<T>(IEnumerable<T> lst)
    {
        foreach (T t in lst)
        {
            Console.WriteLine(t);
        }
    }
}