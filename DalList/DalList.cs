﻿
using DalApi;
namespace Dal;
 public class DalList:IDal
{
    public static IDal Instance { get; }=new DalList();
    public DalList() { }
    public IOrder Order => new DalOrder();
    public IProduct Product => new DalProduct();
    public IOrderItem OrderItem => new DalOrderItem();
}