using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class OrderTracking

{

    public int ID { set; get; }

    public BO.Enums.Status Status { set; get; }

    public List<Tuple<DateTime, string>>? Tracking { set; get; }
    public override string ToString() => this.ToStringProperty();

}

