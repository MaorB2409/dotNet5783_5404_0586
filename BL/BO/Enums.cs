namespace BO;
public struct Enums
{
    public enum Category { Kitchen, Bedroom, DiningRoom, LivingRoom, Outdoors, Office, NoCategory };
    public enum Status { JustOrdered,Processing,Shipped, Arrived,Recieved};
    public enum Action { Add, Del, Update, get, GetList };
    public enum Type { Cart, Order, Product };
}