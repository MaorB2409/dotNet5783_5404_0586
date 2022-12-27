using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Cart
{
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerAddress { get; set; }
    public List<BO.OrderItem?>? orderItems { get; set; }
    public double Price { get; set; }
    public bool IsDeleted { get; set; }
    public override string ToString() => this.ToStringProperty();
}
