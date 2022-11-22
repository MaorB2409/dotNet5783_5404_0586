using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Cart
{
    public string? CostumerName { get; set; }
    public string? CostumerEmail { get; set; }
    public string? CostumerAddress { get; set; }
    public List<OrderItem?> orderItems { get; set; }
    public double? Price { get; set; }
    public bool? IsDeleted { get; set; }
    public override string ToString() => this.ToStringProperty();
}
