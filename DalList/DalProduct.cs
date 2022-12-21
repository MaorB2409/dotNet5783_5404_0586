using System;
using System.Threading;
using DO;
using DalApi;
using System.Security.Cryptography.X509Certificates;

namespace Dal;

public class DalProduct : IProduct
{
    readonly DataSource _ds = DataSource.s_instance;//to access the data 

    public int Add(DO.Product p)//add Product to a list and return its id
    {
        if (p.ID == 0)//want to add a new item to the list
        {
            p.ID = DataSource.Config.NextProductNumber;//set an id number to Product p
            _ds.productList.Add(p);//add p to the Product list
            return p.ID;//return the id
        }
        int ind = _ds.productList.FindIndex(x => x?.ID == p.ID && x?.IsDeleted == false);//save index of product with matching id if not deleted
        if (ind != -1)//exists already so cant add again
        {
            throw new IdExistException("Unothorized override");//error
        }
        _ds.productList.Add(p);//add p to the Product list
        return p.ID;//return the id
        //ind = _ds.productList.FindIndex(x => x?.ID == p.ID && x?.IsDeleted == true);//save index of product with matching id if deleted
        //if (ind != -1)//already exists but deleted 
        //{
        //    _ds.productList.Add(p);//add p to the Product list
        //    return p.ID;//return the id
        //}
        //throw new IdExistException("Unothorized override");//error
    }

    public Product GetById(int id)
    {
        Product? res = _ds.productList.Find(x => x?.ID == id && x?.IsDeleted == false);//find a priduct with same id and exists
        if (res?.ID != id || res?.IsDeleted == true)//if not found
            throw new IdNotExistException("The product does not exist\n");
        return res ?? throw new IdNotExistException("id does not exist\n");
    }

    public void Delete(int id)
    {
        int index = _ds.productList.FindIndex(x => x?.ID == id);

        if (index == -1)//if does not exist
            throw new IdNotExistException("Product does not exist");
        _ds.productList.RemoveAt(index);//remove from list
    }
    public void Update(Product p)
    {
        int index = _ds.productList.FindIndex(x => x?.ID == p.ID);

        if (index == -1)//if does not exist
            throw new IdNotExistException("The product you wish to update does not exist");
        _ds.productList[index] = p;//place new product in place of existing one 
    }

    public IEnumerable<Product?> GetAll(Func<Product?, bool>? filter)
    {
        if (filter == null)//select whole list
        {
            return from v in _ds.productList
                   where v?.IsDeleted == false
                   select v;
        }
        return from v in _ds.productList//select with filter
               where v?.IsDeleted == false && filter(v)
               select v;
    }

    public Product GetByFilter(Func<Product?, bool>? filter)
    {
        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter));//filter is null
        }
        Product? p = _ds.productList.FirstOrDefault(x => x != null && x?.IsDeleted == false && filter(x));
        if (p != null)
        {
            return (Product)p;
        }
        //foreach (Product? p in _ds.productList)
        //{
        //    if (p != null && p?.IsDeleted == false && filter(p))
        //    {
        //        return (Product)p;
        //    }
        //}
        throw new Exceptions("Does not exist\n");
    }
    

}