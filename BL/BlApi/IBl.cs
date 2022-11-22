using BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IBl
{
    public IOrder Order { get; }
    public IProduct Product { get; }
    //public IOrderItem OrderItem { get; }
    public ICart Cart { get; }

   // public IProductItem ProductItem { get; }
   // public IProductForList ProductForList { get; }
   // public IOrderForList OrderForList { get; }
   // public IOrderTracking OrderTracking { get; }





}
