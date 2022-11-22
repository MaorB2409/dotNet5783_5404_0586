using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class OrderForList
{
    public int ID { get; set; } //item ID
    public string? Name { get; set; }
    public Enums.Status? Status {get; set;}
    public int? Amount { get; set; }
    public int? TotalPrice { get; set; }

    public override string ToString() => this.ToStringProperty();
}
