using System;
using System.Threading;
using DO;
using DalApi;
namespace Dal;

public class DalOrder : IOrder
{
    DataSource _ds = DataSource.s_instance;//to access the data 

    public int Add(Order o)//add order to a list and return its id
    {
        if (o.ID == 0)//want to add a new item to the list
        {
            o.ID = DataSource.Config.NextOrderNumber;//set an id number to order p
            _ds.orderList.Add(o);//add p to the order list
            return o.ID;//return the id
        }
        int ind = _ds.orderList.FindIndex(x => x.ID == o.ID && x.IsDeleted == false);//save index of order with matching id if not deleted
        if (ind != -1)//exists already so cant add again
        {
            throw new Exception("Unothorized override");//error
        }
        ind = _ds.orderList.FindIndex(x => x.ID == o.ID && x.IsDeleted == true);//save index of order with matching id if deleted
        if (ind != -1)//already exists but deleted 
        {
            _ds.orderList.Add(o);//add o to the order list
            return o.ID;//return the id
        }
        throw new Exception("Unothorized override");//error
    }

    public Order GetById(int id) { 
        Order res = _ds.orderList.Find(x => x.ID == id && x.IsDeleted == false);
        if (res.ID != id || res.IsDeleted == true)
            throw new Exception("The order does not exist\n");
        return res;
    }

    public void Delete(int id)
    {
        int ind = 0;
        foreach (Order order in _ds.orderList)//gets the index
        {
            if (order.ID == id)//if found id in the order list
                ind = _ds.orderList.IndexOf(order);//save index of that order
        }
        Order o = _ds.orderList[ind];//o is the order of that placement
        o.IsDeleted = true;//change flag
        _ds.orderList[ind] = o; //updates "IsDeleted" to true in the order collection
    }



    public void Update(Order or)
    {
        bool flag = false;
        foreach (Order it in _ds.orderList)//go over order list
        {
            if (or.ID == it.ID && it.IsDeleted == false)//if found a matching id
                flag = true;
        }
        if (flag == true)//if found a matching id
        {
            Delete(or.ID);//delete the existing order of matching id
            Add(or);//add the new order
        }
        else
            throw new Exception("The order you wish to update does not exist");
    }


    public IEnumerable<Order> GetAll()
    {
        List<Order> list = new List<Order> { };
        foreach (Order it in _ds.orderList)
        {
            if (it.IsDeleted == false)//if the orderItem exists 
            {
                list.Add(it);
            }
        }
        return list;
    }
}