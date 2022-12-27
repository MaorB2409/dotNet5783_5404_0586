using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using DalApi;
using System.Linq;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace BlImplementation;

internal class Product:BlApi.IProduct
{
    static readonly IDal? DOList = DalApi.Factory.Get()??throw new BO.Exceptions("Factory does not exist\n");//to access DO info
    public IEnumerable<ProductForList?> GetProductsForList()
    {
        return from DO.Product? item in DOList?.Product.GetAll()!
               where item!=null && item?.IsDeleted==false
               select new ProductForList
               {
                   ID = item?.ID ?? throw new BO.Exceptions("id is null\n"),
                   ProductName = item?.Name!,
                   Price = (double)item?.Price!,
                   Category = (BO.Enums.Category)item?.Category!
               };

    }//returns a list of products for the manager


    public BO.Product ManagerProduct(int id) {
        BO.Product p = new();//create a BO product
        DO.Product product;//create a DO product
        try
        {
            product = (DO.Product)(DOList?.Product.GetById(id)!);//get the matching product for the ID
        }
        catch (DalApi.IdNotExistException){
            throw new BO.IdNotExistException("id does not exist\n");
        }
        if (product.IsDeleted == false)//if found product
        {
            p.ID = id;
            p.Name = product.Name;
            p.Price = product.Price;
            p.Category = (BO.Enums.Category)product.Category;
            p.InStock = product.InStock;
            p.IsDeleted = product.IsDeleted;
            return p;
        }
        throw new BO.IdNotExistException();
    }//return a BO product of DO product with id
    public void AddProduct(BO.Product p)
    {
        if (p.Name == "" || p.Price <= 0 || p.InStock < 0 || p.Category<BO.Enums.Category.Kitchen || p.Category>BO.Enums.Category.Office)
        {
            throw new IncorrectInput("Incorrect Amount");
        }
        //DO.Product prod = DOList.Product.GetById(p.ID);//get product with id
        //if (prod.ID == p.ID)//already exists 
        //    throw new BO.IdExistException();

        DO.Product newProduct = new()
        {
            ID = p.ID,
            Name = p.Name ?? "",
            Price = p.Price,
            InStock = p.InStock,
            IsDeleted = false,
            Category = (DO.Enums.Category)p.Category,
        }; //create new DO product

        try
        {
            newProduct.ID = (int)(DOList?.Product.Add(newProduct)!);//add to product list
        }
        catch (DalApi.IdExistException)
        {
            throw new BO.IdExistException("product id already exists");
        }
    }//gets a BO product, check if right and add a DO product 
    public void DeleteProduct(int id)
    {
        var v = from ords in DOList?.Order.GetAll()
                where ords != null && ords?.IsDeleted == false
                select from oi in DOList?.OrderItem.GetAll()
                       where oi != null && oi?.IsDeleted == false && oi?.OrderID == ords?.ID && oi?.ProductID == id
                       select oi;
        if (v.Any() == false)//no matching order items were found
        {
            throw new BO.UnfoundException();//id not found
        }
        try
        {
            DOList?.Product.Delete(id);//remove orderItem
        }
        catch (DalApi.IdNotExistException)
        {
            throw new BO.IdNotExistException("Product does not exist");
        }
    }
    public void UpdateProduct(BO.Product p)
    {
        if (p.ID < 0 || p.Name == "" || p.Price <= 0 || p.InStock < 0)
        {
                throw new IncorrectInput("Incorrect Input");
        }
        DO.Product temp = new();
        temp.ID = p.ID;
        temp.Name = p.Name ?? "";
        temp.Price = p.Price;
        temp.InStock = p.InStock;
        temp.Category = (DO.Enums.Category)p.Category;
        temp.IsDeleted=false;
        try
        {
            DOList?.Product.Update(temp);
        }
        catch (DalApi.IdNotExistException)
        {
            throw new BO.IdNotExistException("Product does not exist");
        }
    }//get BO product, check if right and updates DO product


    public IEnumerable<ProductItem?> GetCatalog()
    {
        var v = from prods in DOList?.Product.GetAll()
                where prods != null && prods?.IsDeleted == false
                select new ProductItem()
                {
                    ID = prods?.ID ?? throw new BO.IdNotExistException("The product id does not exist"),
                    ProductName = prods?.Name!,
                    Price = (double)prods?.Price!,
                    Amount = (int)prods?.InStock!,
                    Category = (BO.Enums.Category)prods?.Category!,
                    InStock = prods?.InStock == 0 ? false : true
               };
        //foreach (ProductItem item in v)
        //{
        //    if (item.Amount > 0)
        //        item.InStock = true;
        //    item.InStock = false;
        //}
        return v;
    }//go over DO products and build BO product item list 
}
