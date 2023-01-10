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
        int ind = _ds.orderItemList.FindIndex(x => x?.ProductID == oi.ID && x?.IsDeleted == false);//save index of orderItem with matching id if not deleted
        if (ind != -1)//exists already so cant add again
        {
            oi.Amount++;//add one more of the order item
            Update(oi);//send to update
            return oi.ID;
            //throw new IdExistException("The order item exists already so can't add again");//error
        }
        //ind = _ds.orderItemList.FindIndex(x => x?.ProductID == oi.ID && x?.IsDeleted == true);//save index of orderItem with matching id if deleted
        //if (ind != -1)//already exists but deleted 
        //{
        //    _ds.orderItemList.Add(oi);//add oi to the orderItem list
        //    return oi.ID;//return the id
        //}
        throw new Exceptions("The order item can not be added due to technical difficulties");//error

    }

    public OrderItem GetById(int id)
    {
        OrderItem? res = _ds.orderItemList.Find(x => x?.OrderID == id && x?.IsDeleted == false);//find a priduct with same id and exists
        if (res?.ID != id || res?.IsDeleted == true)//if not found
            throw new Exceptions("The OrderItem requested does not exist\n");
        return res ?? throw new Exceptions("The OrderItem requested does not exist\n");
        
    }

    public void Delete(int id)
    {
        int index = _ds.orderItemList.FindIndex(x => x?.ID == id);

        if (index == -1)//if does not exist
            throw new IdNotExistException("Order item you wish to remove does not exist");
        _ds.orderItemList.RemoveAt(index);//remove from list
    }

    public void Update(OrderItem oi)
    {
        int index = _ds.orderItemList.FindIndex(x => x?.ProductID == oi.ID);

        if (index == -1)//if does not exist
            throw new IdNotExistException("The order item you wish to update does not exist");
        _ds.orderItemList[index] = oi;//place new order item in place of existing one 
    }

    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool>? filter)
    {
        if (filter == null)//select whole list
        {
            return from v in _ds.orderItemList
                   where v?.IsDeleted == false
                   select v;
        }
        return from v in _ds.orderItemList//select with filter
               where v?.IsDeleted == false && filter(v)
               select v;

    }

    public IEnumerable<OrderItem?> ItemsInOrder(int id)   //returns list of products in order number of id
    {
        return from v in _ds.orderItemList//go over OrderItem list
               where v?.OrderID == id && v?.IsDeleted== false//if found a matching id to the one inputted
               select v;
   
    }

    public OrderItem ItemOfOrder(int id, int productId)//returns specific product from order of id
    {
        OrderItem returnOI = new();
        returnOI = (DO.OrderItem)_ds.orderItemList.FirstOrDefault(x => x != null && x?.IsDeleted == false && x?.OrderID == id && x?.ProductID == productId)!;
        if (returnOI.ID != id)
        {
            throw new DalApi.IdNotExistException("the requested order item does not exist");
        }
        return returnOI;//return the OrderItem
    }



    public OrderItem GetByFilter(Func<OrderItem?, bool>? filter)
    {
        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter));//filter is null
        }
        OrderItem? o = _ds.orderItemList.FirstOrDefault(x => x != null && x?.IsDeleted == false && filter(x));
        if (o != null)
        {
            return (OrderItem)o;
        }
      
        //foreach (OrderItem? o in _ds.orderItemList)
        //{
        //    if (o!=null && o?.IsDeleted == false && filter(o))
        //    {
        //        return (OrderItem)o;
        //    }
        //}
        throw new Exceptions("The order item requested does not exist\n");
    }
}