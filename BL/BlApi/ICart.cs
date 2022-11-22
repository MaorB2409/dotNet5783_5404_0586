using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlImplementation;
using BO;

namespace BlApi;

public interface ICart
{
    public void AddToCart(BO.Cart? myCart, int id); //check if product is in cart, if not add it from DO product if in stock
    public BO.Cart Update(BO.Cart? myCart, int id, int newAmount); //update the cart to have more or less Products and the total price
    public IEnumerable<OrderItem> MakeOrder(BO.Cart? myCart, string CustomerName, string CustomerEmail, string CustomerAddress);//approve the items in the cart and start making the actual order  

}
