using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dal;
using DalApi;
using DO;

internal class OrderItem : IOrderItem
{
    public int Add(DO.OrderItem item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<DO.OrderItem?> GetAll(Func<DO.OrderItem?, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public DO.OrderItem GetByFilter(Func<DO.OrderItem?, bool>? filter)
    {
        throw new NotImplementedException();
    }

    public DO.OrderItem GetById(int id)
    {
        throw new NotImplementedException();
    }

    public DO.OrderItem ItemOfOrder(int id, int productId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<DO.OrderItem?> ItemsInOrder(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(DO.OrderItem item)
    {
        throw new NotImplementedException();
    }
}
