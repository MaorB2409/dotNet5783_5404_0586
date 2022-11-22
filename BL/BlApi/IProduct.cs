using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi;

public interface IProduct
{
    //manager funcs 
    public IEnumerable<ProductForList> GetProductsForList(DO.Product? p);//returns a list of products for the manager
    public Product? ManagerProduct(int id);//return a BO product of DO product with id
    public void AddProduct(Product? product);//gets a BO product, check if right and add a DO product 
    public void DeleteProduct(int id);//check in every order that DO product is deleted 
    public void UpdateProduct(Product? product);//get BO product, check if right and updates DO product

    //customer funcs
    //public IEnumerable<ProductItem> GetCatalog();//get product list of DO and and return productItem list of BO
    public Product? CustomerProduct(int id);//return a BO product of DO product with id

}
