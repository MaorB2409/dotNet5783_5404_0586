using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DO;

namespace BlApi;

public interface IBoOrderItem
{
    public IEnumerable<OrderItem> ItemsInOrder(int id); //returns list of products in order number of id
    public OrderItem ItemOfOrder(int id, int productId); //returns specific product from order of id
}
