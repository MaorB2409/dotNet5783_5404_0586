//using BlApi;
//using BlImplementation;
//using BO;
//using DO;
//using System;

//class program
//{

//    static IBl bl = new Bl();
//    enum Classes { product = 1, Order, ClientCart }
//    enum ProductActions { GetProds = 1, ProdDetailsManager, AddProd, DelProd, UpdateProd }

//    enum OrderActions { GetOrders = 1, OrderDetails, UpdateOrderDate, UpdateOrder, OrderStatus }

//    enum CartActions { AddProduct = 1, UpdateCart, AproveCart, PrintProds, ProductDetailClient }


//    static void Main(string[] args)
//    {

//        while (true)
//        {

//            Console.WriteLine("Please enter witch class you want to work on:");
//            printeEnum<Classes>();
//            Classes cls = (Classes)GetNumberFromUser();


//            switch (cls)
//            {
//                case ((Classes)0):
//                    Console.WriteLine("Bye Bye :)\n");
//                    return;

//                case (Classes.product):
//                    testProduct();
//                    break;

//                case (Classes.Order):
//                    testOrder();
//                    break;

//                case (Classes.ClientCart):
//                    testCart();
//                    break;
//            }

//        }







//    }


//    /// <summary>
//    /// manage activity for product
//    /// </summary>
//    static void testProduct()
//    {

//        while (true)
//        {

//            Console.WriteLine("Please enter witch Product action you want to work:\n" +
//                "To exit ProdutTest press 0");
//            printeEnum<ProductActions>();
//            ProductActions p = (ProductActions)GetNumberFromUser();


//            switch (p)
//            {
//                case ((ProductActions)0):
//                    Console.WriteLine("Out of Product test\n");
//                    return;

//                case ProductActions.GetProds:
//                    printList<ProductForList>(bl.IProduct.GetList());
//                    break;

//                case ProductActions.ProdDetailsManager:
//                    try
//                    {
//                        Console.WriteLine(bl.IProduct.GetProductManager(GetNumberFromUser("Enter Product ID: ")));
//                    }
//                    catch (Exception e)
//                    {
//                        Console.WriteLine(e.Message); return;
//                    }
//                    break;

//                case ProductActions.AddProd:
//                    AddProduct();
//                    break;

//                case ProductActions.DelProd:
//                    DelProd();
//                    break;

//                case ProductActions.UpdateProd:
//                    UpdateProd();
//                    break;
//            }

//        }

//    }


//    /// <summary>
//    /// manage activity for order
//    /// </summary>
//    static void testOrder()
//    {
//        while (true)
//        {


//            Console.WriteLine("Please enter witch Order action you want to work:\n" +
//                   "To exit OrderTest press 0");
//            printeEnum<OrderActions>();
//            OrderActions o = (OrderActions)GetNumberFromUser();

//            switch (o)
//            {
//                case ((OrderActions)0):
//                    Console.WriteLine("Exiting Order actions");
//                    return;
//                case OrderActions.GetOrders:
//                    printList<BO.OrderForList>(bl.IOrder.GetOrders());
//                    break;
//                case OrderActions.OrderDetails:
//                    Console.Error.WriteLine(bl.IOrder.GetOrder(GetNumberFromUser("Enter Product ID: ")));
//                    break;
//                case OrderActions.UpdateOrderDate:
//                    NewDateTime();
//                    break;
//                case OrderActions.UpdateOrder:
//                    OrderUpdate();
//                    break;
//                case OrderActions.OrderStatus:
//                    StatusOfOrder();
//                    break;
//            }
//        }

//    }


//    /// <summary>
//    /// manage activity for cart
//    /// </summary>
//    static void testCart()
//    {
//        Cart cart = new Cart() { OrderList = new List<BO.OrderItem>() };
//        while (true)
//        {

//            Console.WriteLine("your cart is :\n" + cart);

//            Console.WriteLine("Please enter witch Cart action you want to work:\n" +
//                "To exit CartTest press 0");
//            printeEnum<CartActions>();
//            CartActions c = (CartActions)GetNumberFromUser();

//            switch (c)
//            {
//                case ((CartActions)0):
//                    Console.WriteLine("Out of Cart test\n");
//                    return;
//                case CartActions.AddProduct:
//                    ProdToCart(cart);
//                    break;
//                case CartActions.UpdateCart:
//                    CartUpdate(cart);
//                    break;
//                case CartActions.PrintProds:
//                    printList<BO.ProductForList>(bl.IProduct.GetList());
//                    break;
//                case CartActions.ProductDetailClient:
//                    ProductDataCl(cart);
//                    break;

//                case CartActions.AproveCart:
//                    aproveCart(cart);
//                    return;
//            }
//        }

//    }


//    /// <summary>
//    /// get client info 
//    /// </summary>
//    /// <param name="cart"></param>
//    static void aproveCart(Cart cart)
//    {

//        Console.WriteLine("enter name");
//        string name = Console.ReadLine();

//        Console.WriteLine("enter mail");
//        string mail = Console.ReadLine();

//        Console.WriteLine("enter address");
//        string address = Console.ReadLine();


//        try
//        {
//            bl.ICart.AprrovedCart(cart);
//            Console.WriteLine("cart aproved  your cart is " + cart);
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e.Message);
//        }
//    }


//    /// <summary>
//    /// get info fro user to add new product 
//    /// </summary>
//    static void AddProduct()
//    {
//        BO.Product p = new BO.Product();

//        Console.WriteLine("Enter catagory of new product:");
//        printeEnum<Enums.Category>();
//        int cat = GetNumberFromUser();
//        if (!IntInEnum<Enums.Category>(cat))
//        {
//            Console.WriteLine("Catagory dose not exist");
//        }
//        p.Category = (Category)cat;

//        Console.WriteLine("Enter name of product:");
//        p.Name = Console.ReadLine();

//        Console.WriteLine("Enter in stock:");
//        p.InStock = GetNumberFromUser();
//        Console.WriteLine("Enter price of Product");
//        p.Price = GetNumberFromUser();

//        try
//        {
//            bl.IProduct.Add(p);
//        }
//        catch (ArgumentException e)
//        {
//            Console.WriteLine(e.Message);
//        }


//    }


//    /// <summary>
//    /// get id from user to delete pdroduct
//    /// </summary>
//    static void DelProd()
//    {
//        int id = GetNumberFromUser("Please enter requested product ID:");
//        try
//        {
//            bl.IProduct.Delete(id);
//        }
//        catch (UnfoundException e)
//        {
//            Console.WriteLine(e.Message);
//        }
//    }

//    /// <summary>
//    /// get info from user to updae product
//    /// </summary>
//    static void UpdateProd()
//    {
//        BO.Product p = new BO.Product();

//        p.ID = GetNumberFromUser("Please enter the Product ID:");
//        Console.WriteLine("Please enter Product new Name:");
//        p.Name = Console.ReadLine();
//        p.Price = GetNumberFromUser("Please enter Product new Price:");
//        p.InStock = GetNumberFromUser("Please enter Product In stock:");
//        try
//        {
//            bl.IProduct.Update(p);
//        }
//        catch (ArgumentException e)
//        {
//            Console.WriteLine(e.Message);
//        }
//    }


//    /// <summary>
//    /// get id from user to update the date
//    /// </summary>
//    static void NewDateTime()
//    {
//        int id = GetNumberFromUser("Please enter order id");
//        try
//        {
//            bl.IOrder.UpdateOrderShip(id);
//        }
//        catch (UnfoundException e)
//        {
//            Console.WriteLine(e.Message);
//        }

//    }


//    /// <summary>
//    /// get id from user to update delivery order  
//    /// </summary>
//    static void OrderUpdate()
//    {
//        int id = GetNumberFromUser("Please enter order id");
//        try
//        {
//            Console.WriteLine(bl.IOrder.UpdateOrderDelivery(id));
//        }
//        catch (UnfoundException e)
//        {
//            Console.WriteLine(e.Message);
//        }
//    }


//    /// <summary>
//    /// get order id and print the list info
//    /// </summary>
//    /// <returns></returns>
//    /// <exception cref="Exception"></exception>
//    static void StatusOfOrder()
//    {
//        int id = GetNumberFromUser("Please enter order id");
//        try
//        {
//            Console.WriteLine(bl.IOrder.OrderTracking(id));
//        }
//        catch (UnfoundException e)
//        {
//            Console.WriteLine(e.Message);
//        }
//    }


//    /// <summary>
//    /// get product id from user and u[date the cart
//    /// </summary>
//    /// <param name="cart"></param>
//    static void ProdToCart(Cart cart)
//    {
//        int id = GetNumberFromUser("enter product id to add to cart:");
//        try
//        {
//            cart = bl.ICart.AddProduct(cart, id);
//        }
//        catch (UnfoundException a)
//        {
//            Console.WriteLine(a.Message);
//        }

//    }


//    /// <summary>
//    /// get number and print the product client info
//    /// </summary>
//    /// <param name="cart"></param>
//    static void ProductDataCl(BO.Cart cart)
//    {
//        int id = GetNumberFromUser("please enter product id:");
//        try { Console.WriteLine(bl.IProduct.GetProductClient(id, cart)); }
//        catch (UnfoundException e)
//        {
//            Console.WriteLine(e.Message);
//        }
//    }




//    static void CartUpdate(BO.Cart cart)
//    {
//        int id = GetNumberFromUser("please enter product id");
//        int amount = GetNumberFromUser("please enter how much to add or reduce");
//        try
//        {
//            cart = bl.ICart.UpdateCart(cart, id, amount);
//        }

//        catch (UnfoundException e)
//        {
//            Console.WriteLine(e.Message);
//        }
//        catch (ArgumentException e)
//        {
//            Console.WriteLine(e.Message);
//        }
//        //need a not in stock exception
//    }

//    static bool IntInEnum<T>(int i) where T : System.Enum
//    {
//        return (Enum.GetValues(typeof(T)).Cast<int>().Contains(i));
//    }

//    static void printeEnum<T>() where T : System.Enum//enum x
//    {
//        foreach (int t in Enum.GetValues(typeof(T)))
//        {
//            Console.WriteLine(t.ToString() + ": " + Enum.GetName(typeof(T), t));
//        }
//    }

//    static void printList<T>(IEnumerable<T> lst)
//    {
//        foreach (T t in lst)
//        {
//            Console.WriteLine(t);
//        }
//    }

//    static int GetNumberFromUser(string txt = "")//the programer sends what he needs
//    {
//        Console.WriteLine(txt);
//        int num;
//        while (!System.Int32.TryParse(Console.ReadLine(), out num))//if not int
//        {
//            Console.WriteLine("ERROR format\n");//error
//        }
//        return num;//read users number
//    }
//}