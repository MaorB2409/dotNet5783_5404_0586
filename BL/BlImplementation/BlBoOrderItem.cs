using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using DO;

namespace BlImplementation;

internal class BlBoOrderItem : IBoOrderItem
{
    public OrderItem ItemOfOrder(int id, int productId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OrderItem> ItemsInOrder(int id)
    {
        throw new NotImplementedException();
    }
}
