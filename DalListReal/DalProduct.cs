using System;
using System.Threading;
using DO;
using DalApi;
namespace Dal;

public class DalProduct : IProduct
{
    DataSource _ds = DataSource.s_instance;//to access the data 

    public int Add(Product? p)//add Product to a list and return its id
    {
        if (p.HasValue && p?.ID == 0)//want to add a new item to the list
        {
            p.ID = DataSource.Config.NextProductNumber;//set an id number to Product p
            _ds.productList.Add(p);//add p to the Product list
            return p.ID;//return the id
        }
        int ind = _ds.productList.FindIndex(x => x?.ID == p?.ID && x?.IsDeleted == false);//save index of product with matching id if not deleted
        if (ind != -1)//exists already so cant add again
        {
            throw new Exception("Unothorized override");//error
        }
        ind = _ds.productList.FindIndex(x => x?.ID == p?.ID && x?.IsDeleted == true);//save index of product with matching id if deleted
        if (ind != -1)//already exists but deleted 
        {
            _ds.productList.Add(p);//add p to the Product list
            return p.ID;//return the id
        }
        throw new Exception("Unothorized override");//error
    }

    public Product? GetById(int id)
    {
        Product res = _ds.productList.Find(x => x.ID == id && x.IsDeleted == false);//find a priduct with same id and exists
        if (res.ID != id || res.IsDeleted == true)//if not found
            throw new Exception("The product does not exist\n");
        return res;
    }

    public void Delete(int id)
    {
        int ind = 0;
        foreach (Product? product in _ds.productList)//gets the index
        {
            if (product?.ID == id)//if found id in the Product list
                ind = _ds.productList.IndexOf(product);//save index of that Product
        }
        Product? p = _ds.productList[ind];//p is the Product of that placement
        p.IsDeleted = true;//change flag
        _ds.productList[ind] = p; //updates "IsDeleted" to true in the Product collection
    }

    public void Update(Product? p)
    {
        bool flag = false;
        foreach (Product it in _ds.productList)//go over Product list
        {
            if (p.ID == it.ID && it.IsDeleted == false)//if found a matching id
                flag = true;
        }
        if (flag == true)//if found a matching id-delete existing and add new
        {
            Delete(p.ID);//delete the existing Product of matching id
            Add(p);//add the new Product
        }
        else
            throw new Exception("The order you wish to update does not exist");
    }

    public IEnumerable<Product?> GetAll()
    {
        List<Product?> list = new List<Product?> { };
        foreach (Product? it in _ds.productList)
        {
            if (it?.IsDeleted == false)//if the product exists 
            {
                list.Add(it);
            }
        }
        return list;
    }
}