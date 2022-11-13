using System;
using System.Threading;
using DO;
using DalApi;
namespace Dal;

public class DalOrder:IOrder
{
   // DataSource _ds=DataSource.s_instance;//to access the data 

    public int Add(Order o)//add order to a list and return its id
    {
        if (o.ID != 0 )//if the item already exists 
        {
            int ind = 0;
            foreach (Order order in DataSource.orderList)//go over order list
            {
                if (order.ID == o.ID)//if o exists in list
                    ind = DataSource.orderList.IndexOf(order);//save place of the matching existing order 
            }
            DataSource.orderList[ind] = o;//place o order in that place of ind
            return o.ID;//return the id
        }
        else
        {
            if (o.ID != 0 && DataSource.orderList.ElementAt(o.ID).IsDeleted == false)//the item's ID exists, but it was not deleted from the collection => throw exression
                throw new Exception("Unothorized override");
            else
            {
                o.ID = DataSource.Config.s_nextOrderNumber;//set an id number to order o
                DataSource.orderList.Add(o);//add o to the order list
                return o.ID;//return the id
            }
        }
    }

    public Order GetById(int id)//=>DataSource.orderList.Find(x=>x.GetValueOrDefault().ID==id)?? throw new Exception("order with entered order ID does not exist\n");//get an order by order id
    {
        Order res = DataSource.orderList.Find(x => x.ID == id);
        if (res.ID != id)
            throw new Exception("The order does not exist\n");
        return res;
    }// =>DataSource.orderList.FirstOrDefault()??throw new Exception("missing order ID");//get an order by order id
    
    public void Delete(int id)
    {
        int ind = 0;
        foreach (Order order in DataSource.orderList)//gets the index
        {
            if (order.ID == id)//if found id in the order list
                ind = DataSource.orderList.IndexOf(order);//save index of that order
        }
        Order o = DataSource.orderList[ind];//o is the order of that placement
        o.IsDeleted = true;//change flag
        DataSource.orderList[ind] = o; //updates "IsDeleted" to true in the order collection
    }



    public void Update(Order or)
    {
        bool flag = false;
        foreach (Order it in DataSource.orderList)//go over order list
        {
            if (or.ID == it.ID)//if found a matching id
                flag = true;
        }
        if (flag == true)//if found a matching id
        {
            int ind = 0;
            foreach (Order order in DataSource.orderList)//go over order list
            {
                if (order.ID == or.ID)//if found a matching id to the one inputted
                    ind = DataSource.orderList.IndexOf(order);//save the index 
            }
            Order o = DataSource.orderList[ind];//o is the order in ind index
            Delete(or.ID);//delete the existing order of matching id
            Add(or);//add the new order
        }
        else
            throw new Exception("The order you wish to update does not exist");
    }


    public IEnumerable<Order> GetAll()
    {
        List<Order> list = new List<Order> { };
        foreach (Order it in DataSource.orderList)
        {
            list.Add(it);
        }
        return list;//return the new list of orders
       //return (from Order item in DataSource.orderList select item).ToList();//return the new list of orders
    }
}
