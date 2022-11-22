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

internal class Cart:ICart
{
    private IDal DOList = new DalList();//to access DO info
    public BO.Cart AddToCart(BO.Cart? myCart, int id)
    {
        BO.OrderItem orderItem = new BO.OrderItem();//create new orderitem
        orderItem = myCart.orderItems.Find(x => x.ProductID == id);//save the orderitem with id
        if(orderItem == null)//if no matching orderitem in cart
        {
            
        }

    }
    public BO.Cart Update(BO.Cart? myCart, int id, int newAmount){
        BO.OrderItem orderItem = new BO.OrderItem();//create new orderitem
        orderItem = myCart.orderItems.Find(x => x.ProductID == id);//save the orderitem with id
        int index = myCart.orderItems.FindIndex(x => x.ProductID == id);//save index of the orderitem with id
        if (newAmount > orderItem?.Amount)//if need to increase existing amount
        {
            myCart.Price += orderItem?.Price * (orderItem.Amount + newAmount);//increase price
            myCart.orderItems[index].Amount = newAmount;//change amount of orderitem
            return myCart;
        }
        if (newAmount < orderItem?.Amount)//if need to deccrease existing amount
        {
            myCart.Price -= orderItem?.Price * (orderItem.Amount - newAmount);//decrease price
            myCart.orderItems.       //[index].Amount = newAmount;//change amount of orderitem
            return myCart
        }

    }
    public IEnumerable<OrderItem> MakeOrder(BO.Cart? myCart, string CustomerName, string CustomerEmail, string CustomerAddress)
    {

    }
}
