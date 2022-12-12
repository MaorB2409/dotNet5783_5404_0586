using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

sealed class Bl : IBl //changed from sealed public class
{
    public ICart Cart => new Cart();
    public IOrder Order => new Order();
    public IProduct Product => new Product();
    
}
