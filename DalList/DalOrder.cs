using System;
using System.Threading;
using DO;
using DalApi;
namespace Dal;

public class DalOrder : IOrder
{
    readonly DataSource _ds = DataSource.s_instance;//to access the data 

    public int Add(Order o)//add order to a list and return its id
    {
        if (o.ID == 0)//want to add a new item to the list
        {
            o.ID = DataSource.Config.NextOrderNumber;//set an id number to order p
            _ds.orderList.Add(o);//add p to the order list
            return o.ID;//return the id
        }
        int ind = _ds.orderList.FindIndex(x => x?.ID == o.ID && x?.IsDeleted == false);//save index of order with matching id if not deleted
        if (ind != -1)//exists already so cant add again
        {
            throw new Exceptions("Order exists already so cant add again");//error
        }
        ind = _ds.orderList.FindIndex(x => x?.ID == o.ID && x?.IsDeleted == true);//save index of order with matching id if deleted
        if (ind != -1)//already exists but deleted 
        {
            _ds.orderList.Add(o);//add o to the order list
            return o.ID;//return the id
        }
        throw new Exceptions("The order can not be placed due to technical difficulties");//error
    }

    public Order GetById(int id) { 
        Order? res = _ds.orderList.Find(x => x?.ID == id && x?.IsDeleted == false);
        if (res?.ID != id || res?.IsDeleted == true)
            throw new IdNotExistException("The order that was requested does not exist\n");
        return res ?? throw new Exceptions("The order that was requested does not exist");
    }

    public void Delete(int id)
    {
        int index = _ds.orderList.FindIndex(x => x?.ID == id);

        if (index == -1)//if does not exist
            throw new IdNotExistException("Order does not exist");
        _ds.orderList.RemoveAt(index);//remove from list
    }



    public void Update(Order or)
    {
        int index = _ds.orderList.FindIndex(x => x?.ID == or.ID);

        if (index == -1)//if does not exist
            throw new IdNotExistException("The order you wish to update does not exist");
        _ds.orderList[index] = or;//place new order in place of existing one 
    }


    public IEnumerable<Order?> GetAll(Func<Order?, bool>? filter)
    {
        if (filter == null)//select whole list
        {
            return from v in _ds.orderList
                   where v?.IsDeleted == false
                   select v;
        }
        return from v in _ds.orderList//select with filter
               where v?.IsDeleted == false && filter(v)
               select v;
    }


    public Order GetByFilter(Func<Order?, bool>? filter)
    {
        if(filter == null)
        {
            throw new ArgumentNullException(nameof(filter));//filter is null
        }
        foreach (Order? o in _ds.orderList)
        {
            if (o!=null && o?.IsDeleted == false && filter(o))
            {
                return (Order)o;
            }
        }
        throw new Exceptions("The order that was requested does not exist");
    }
}