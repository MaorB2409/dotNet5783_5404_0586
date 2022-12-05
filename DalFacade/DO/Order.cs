namespace DO;

/// <summary>
/// the order info includes-
/// order id, costumer info, different time stamps, total price, and order status
/// </summary>
public struct Order
{
    public int ID { get; set; } //item ID
    public string CostumerName { get; set; }
    public string CostumerEmail { get; set; }
    public string CostumerAddress { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ShipDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public bool IsDeleted { get; set; }
    public override string ToString() => this.ToStringProperty();
    //public override string ToString() => $@"
    //   item ID is - {ID},
    //Costumer Name is - {CostumerName}, 
    //CostumerEmail is - {CostumerEmail},
    //   Costumer Address is - {CostumerAddress},
    //   Order Date is - {OrderDate},
    //   Shipping Date is - {ShipDate},
    //   Delivery Date is - {DeliveryDate}
    //";

}
