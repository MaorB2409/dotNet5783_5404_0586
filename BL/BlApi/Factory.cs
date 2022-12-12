using BlImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public class Factory
    {
        public static IBl? Get() => new BlImplementation.Bl();
        //public static IBl Get()
        //{
        //    IBl bl = new Bl();
        //    return bl;
        //}

    }
}
