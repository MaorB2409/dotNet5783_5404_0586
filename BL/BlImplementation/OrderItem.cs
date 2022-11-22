using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using Dal;
using DalApi;
using DO;

namespace BlImplementation;

internal class OrderItem : IOrderItem
{
    static IDal DOList = new DalList();
    public DO.OrderItem ItemOfOrder(int id, int productId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<DO.OrderItem> ItemsInOrder(int id)
    {
        throw new NotImplementedException();
    }
}
