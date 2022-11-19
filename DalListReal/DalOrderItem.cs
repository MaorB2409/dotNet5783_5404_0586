using System;
using System.Threading;
using DO;
using DalApi;
namespace Dal;

public class DalOrderItem : IOrderItem
{
    DataSource _ds = DataSource.s_instance;//to access the data 

    public int Add(OrderItem oi)//add OrderItem to a list and return its id
    {
        if (oi.ID == 0)//want to add a new item to the list
        {
            oi.ID = DataSource.Config.NextOrderItemNumber;//set an id number to Product p
            _ds.orderItemList.Add(oi);//add p to the Product list
            return oi.ID;//return the id
        }
        int ind = _ds.orderItemList.FindIndex(x => x.ID == oi.ID && x.IsDeleted == false);//save index of orderItem with matching id if not deleted
        if (ind != -1)//exists already so cant add again
        {
            throw new Exception("Unothorized override");//error
        }
        ind = _ds.orderItemList.FindIndex(x => x.ID == oi.ID && x.IsDeleted == true);//save index of orderItem with matching id if deleted
        if (ind != -1)//already exists but deleted 
        {
            _ds.orderItemList.Add(oi);//add oi to the orderItem list
            return oi.ID;//return the id
        }
        throw new Exception("Unothorized override");//error

    }

    public OrderItem GetById(int id)
    {
        OrderItem res = _ds.orderItemList.Find(x => x.ID == id && x.IsDeleted==false);
        if (res.ID != id || res.IsDeleted == true)
            throw new Exception("The orderItem does not exist\n");
        return res;
    }

    public void Delete(int id)
    {
        int ind = 0;
        foreach (OrderItem orderi in _ds.orderItemList)//gets the index
        {
            if (orderi.ID == id)//if found id in the OrderItem list
                ind = _ds.orderItemList.IndexOf(orderi);//save index of that OrderItem
        }
        OrderItem oi = _ds.orderItemList[ind];//oi is the OrderItem of that placement
        oi.IsDeleted = true;//change flag
        _ds.orderItemList[ind] = oi; //updates "IsDeleted" to true in the OrderItem collection
    }

    public void Update(OrderItem oi)
    {
        bool flag = false;
        foreach (OrderItem it in _ds.orderItemList)//go over OrderItem list
        {
            if (oi.ID == it.ID && it.IsDeleted == false)//if found a matching id
                flag = true;
        }
        if (flag == true)//if found a matching id
        {
            Delete(oi.ID);//delete the existing OrderItem of matching id
            Add(oi);//add the new OrderItem
        }
        else
            throw new Exception("The order you wish to update does not exist");
    }

    public IEnumerable<OrderItem> GetAll()
    {
        List<OrderItem> list = new List<OrderItem> { };
        foreach (OrderItem it in _ds.orderItemList)
        {
            if (it.IsDeleted == false)//if the orderItem exists 
            {
                list.Add(it);
            }
        }
        return list;
    }

    public IEnumerable<OrderItem> ItemsInOrder(int id)   //returns list of products in order number of id
    {
        IEnumerable<OrderItem> orderItems = new List<OrderItem>();
        foreach (OrderItem orderi in _ds.orderItemList)//go over OrderItem list
        {
            if (orderi.ID == id&& orderi.IsDeleted==false)//if found a matching id to the one inputted
                orderItems.Append(orderi);//add to the list
        }//find the order of id
        return orderItems;//return the products
    }

    public OrderItem ItemOfOrder(int id, int productId)//returns specific product from order of id
    {
        OrderItem returnOI = new();
        foreach (OrderItem orderi in _ds.orderItemList)//go over OrderItem list
        {
            if (orderi.OrderID == id && orderi.ProductID == productId)//if found a matching id to the one inputted and product
            { 
                if(orderi.IsDeleted == false)//not deleted
                {
                    returnOI = orderi;//save that one
                }
            }
        }//find the order of id with product
        return returnOI;//return the OrderItem
    }

}