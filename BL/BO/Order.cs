﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Order
{
    public int ID { get; set; } //item ID
    public string? CostumerName { get; set; }
    public string? CostumerEmail { get; set; }
    public string? CostumerAddress { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ShipDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public bool IsDeleted { get; set; }
    public Enums.Status Status { get; set; }
    public double TotalPrice { get; set; }
    public List<BO.OrderItem?>? orderItems { get; set; }


    public override string ToString() => this.ToStringProperty();
 //   public override string ToString() => $@"
 //   item ID is - {ID},
	//Costumer Name is - {CostumerName}, 
	//CostumerEmail is - {CostumerEmail},
 //   Costumer Adrees is - {CostumerAddress},
 //   Order Date is - {OrderDate},
 //   Shipping Date is - {ShipDate},
 //   Delivery Date is - {DeliveryDate}
	//";

}
