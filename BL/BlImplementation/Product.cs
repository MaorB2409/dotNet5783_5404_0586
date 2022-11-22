using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using Dal;
using DalApi;

namespace BlImplementation;

internal class Product:IProduct
{
    static IDal DOList = new DalList();

    private IDal DOList = new DalList();//to access DO info
    public IEnumerable<ProductForList?> GetProductsForList(DO.Product? p)
    {
        //    return from DO.Product item in DOList.
        //           select new ProductForList()
        //           {
        //               ID = item.ID,
        //               ProductName = item.Name,
        //               Price = item.Price,
        //               Category = (BO.Enums.Category)item.Category
        //           };

    }//returns a list of products for the manager
    public Product? ManagerProduct(int id) {

    }//return a BO product of DO product with id
    public void AddProduct(Product? product)
    {

    }//gets a BO product, check if right and add a DO product 
    public void DeleteProduct(int id)
    {

    }//check in every order that DO product is deleted 
    public void UpdateProduct(Product? product)
    {

    }//get BO product, check if right and updates DO product

    public IEnumerable<ProductItem> GetCatalog()
    {

    }//get product list of DO and and return productItem list of BO

}
