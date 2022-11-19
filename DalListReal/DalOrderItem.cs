using System;
using System.Threading;
using DO;
using DalApi;
namespace Dal;

public class DalOrderItem : IOrderItem
{
   DataSource _ds=DataSource.s_instance;//to access the data 

    public int Add(OrderItem oi)//add OrderItem to a list and return its id
    { 
        if (oi.ID != 0 && _ds.orderItemList.ElementAt(oi.ID).IsDeleted == true)//if the item already exists and was "deleted" => we came from the 'update' operation
        {
            int ind = 0;
            foreach (OrderItem orderi in _ds.orderItemList)//go over OrderItem list
            {
                if (orderi.ID == oi.ID)//if oi exists in list
                    ind = _ds.orderItemList.IndexOf(orderi);//save place of the matching existing OrderItem 
            }
            _ds.orderItemList[ind] = oi;//place oi OrderItem in that place of ind
            return oi.ID;//return the id
        }
        else
        {
            if (oi.ID != 0 && _ds.orderItemList.ElementAt(oi.ID).IsDeleted == false)//the item's ID exists, but it was not deleted from the collection => throw exression
                throw new Exception("Unothorized override");
            else
            {
                oi.ID = DataSource.Config.s_nextOrderItemNumber;//set an id number to OrderItem p
                _ds.orderItemList?.Add(oi);//add oi to the OrderItem list
                return oi.ID;//return the id
            }
        } 
    }

    public OrderItem GetById(int id)
    {
        OrderItem res = _ds.orderItemList.Find(x => x.ID == id);
        if (res.ID != id)
            throw new Exception("The orderItem does not exist\n");
        return res;
    } //=>DataSource.orderItemList.FirstOrDefault()??throw new Exception("missing order item ID");//get an OrderItem by its id
   
    public void Delete(int id)
    {
        int ind = 0;
        foreach (OrderItem orderi in _ds.orderItemList)//gets the index
        {
            if (orderi.ID == id)//if found id in the OrderItem list
                ind = _ds.orderItemList.IndexOf(orderi);//save index of that OrderItem
        }
        OrderItem oi = _ds.orderItemList[ind];//oi is the OrderItem of that placement
        _ds.orderItemList[ind] = oi; //updates "IsDeleted" to true in the OrderItem collection
    }

    public void Update(OrderItem oi)
    {
        bool flag = false;
        foreach (OrderItem it in _ds.orderItemList)//go over OrderItem list
        {
            if (oi.ID == it.ID)//if found a matching id
                flag = true;
        }
        if (flag == true)//if found a matching id
        {
            int ind = 0;
            foreach (OrderItem orderi in _ds.orderItemList)//go over OrderItem list
            {
                if (orderi.ID == oi.ID)//if found a matching id to the one inputted
                    ind = _ds.orderItemList.IndexOf(orderi);//save the index 
            }
            OrderItem ord = _ds.orderItemList[ind];//ord is the OrderItem in ind index
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
            list.Add(it);
        }
        return list;//return the new list of Products
        //return (from OrderItem item in DataSource.orderItemList select item).ToList();//return the new list of OrderItems
    }

    public IEnumerable<OrderItem> ItemsInOrder(int id)   //returns list of products in order number of id
    {
        IEnumerable<OrderItem> orderItems = new List<OrderItem>();
        foreach (OrderItem orderi in _ds.orderItemList)//go over OrderItem list
            {
                if (orderi.ID == id)//if found a matching id to the one inputted
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
                returnOI=orderi;//save that one
        }//find the order of id with product
        return returnOI;//return the OrderItem
    }

}
