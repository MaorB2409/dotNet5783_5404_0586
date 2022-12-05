using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class OrderForList
{
    public int ID { get; set; } //order ID
    public string Name { get; set; }
    public Enums.Status Status {get; set;}
    public int Amount { get; set; }
    public double TotalPrice { get; set; }

    public override string ToString() => this.ToStringProperty();
}
