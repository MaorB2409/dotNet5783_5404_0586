using DalApi;
using DO;

namespace DalApi;
/// <summary>
/// inherits from ICrud for OrderItem
/// </summary>
public interface IOrderItem : ICrud<OrderItem>
{
    IEnumerable<OrderItem?> ItemsInOrder(int id); //returns list of products in order number of id
    OrderItem ItemOfOrder(int id, int productId); //returns specific product from order of id

}