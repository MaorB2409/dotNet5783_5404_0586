using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    internal class Tools
    {
        PO.Product CastBoProductToPo(BO.Product p)
        {
            PO.Product poProduct = new PO.Product();
            poProduct.Name = p.Name;
            poProduct.Price = p.Price;
            poProduct.ID=p.ID;
            poProduct.Price=p.Price;
            poProduct.InStock = p.InStock;
            poProduct.Category = p.Category;
            return poProduct;
        }

    }
}
