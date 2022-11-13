using System;
using System.Threading;
using DO;
using DalApi;
namespace Dal;

public class DalProduct:IProduct
{
   // DataSource _ds=DataSource.s_instance;//to access the data ////////////////

    public int Add(Product p)//add Product to a list and return its id
    { 
        if (p.ID != 0 && DataSource.productList.ElementAt(p.ID).IsDeleted == true)//if the item already exists and was "deleted" => we came from the 'update' operation
        {
            int ind = 0;
            foreach (Product product in DataSource.productList)//go over Product list
            {
                if (product.ID == p.ID)//if p exists in list
                    ind = DataSource.productList.IndexOf(product);//save place of the matching existing order 
            }
            DataSource.productList[ind] = p;//place p Product in that place of ind
            return p.ID;//return the id
        }
        else
        {
            if (p.ID != 0 && DataSource.productList.ElementAt(p.ID).IsDeleted == false)//the item's ID exists, but it was not deleted from the collection => throw exression
                throw new Exception("Unothorized override");
            else
            {
                p.ID = DataSource.Config.s_nextProductNumber;//set an id number to Product p
                DataSource.productList.Add(p);//add p to the Product list
                return p.ID;//return the id
            }
        } 
    }

    public Product GetById(int id)
    {
        Product res = DataSource.productList.Find(x => x.ID == id );
        if (res.ID!=id)
            throw new Exception("The product does not exist\n");
        return res;
    } //=>DataSource.productList.FirstOrDefault()??throw new Exception("missing product ID");//get an product by its id
    
    public void Delete(int id)
    {
        int ind = 0;
        foreach (Product product in DataSource.productList)//gets the index
        {
            if (product.ID == id)//if found id in the Product list
                ind = DataSource.productList.IndexOf(product);//save index of that Product
        }
        Product p = DataSource.productList[ind];//p is the Product of that placement
        p.IsDeleted = true;//change flag
        DataSource.productList[ind] = p; //updates "IsDeleted" to true in the Product collection
    }

    public void Update(Product p)
    {
        bool flag = false;
        foreach (Product it in DataSource.productList)//go over Product list
        {
            if (p.ID == it.ID)//if found a matching id
                flag = true;
        }
        if (flag == true)//if found a matching id
        {
            int ind = 0;
            foreach (Product product in DataSource.productList)//go over Product list
            {
                if (product.ID == p.ID)//if found a matching id to the one inputted
                    ind = DataSource.productList.IndexOf(product);//save the index 
            }
            Product pr = DataSource.productList[ind];//pr is the Product in ind index
            Delete(p.ID);//delete the existing Product of matching id
            Add(p);//add the new Product
        }
        else
            throw new Exception("The order you wish to update does not exist");
    }

    public IEnumerable<Product> GetAll()
    {
        List<Product> list = new List<Product>{ };
        foreach (Product it in DataSource.productList)
        {
            list.Add(it);
        }
        return list;//return the new list of Products
       // return (from Product item in DataSource.productList select item).ToList();
    }
}
