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
    public IBoOrder BoOrder { get; }
    public IBoProduct BoProduct { get; }
    public IBoOrderItem BoOrderItem { get; }
}
