using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
using DO;

internal class Product : IProduct
{
    public int Add(DO.Product item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<DO.Product?> GetAll(Func<DO.Product?, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public DO.Product GetByFilter(Func<DO.Product?, bool>? filter)
    {
        throw new NotImplementedException();
    }

    public DO.Product GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(DO.Product item)
    {
        throw new NotImplementedException();
    }
}
