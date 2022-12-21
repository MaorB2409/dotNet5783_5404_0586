using BlApi;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

sealed internal class Bl : IBl 
{
    public ICart Cart => new Cart();
    public BlApi.IOrder Order => new Order();
    public BlApi.IProduct Product => new Product();
    
}
//namespace BL
//{
//    sealed class BL : IBL
//    {
//        static readonly IBL instance = new BL();
//        public static IBL Instance { get => instance; }

//        internal IDal dal = DalFactory.GetDal();
//        BL() { }

//    }
//}