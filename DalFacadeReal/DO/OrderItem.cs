namespace DO;

/// <summary>
/// short description and info on product for the cart 
/// </summary>
public struct OrderItem
{
    public int ID { get; set; } //general product ID
    public int ProductID { get; set; } //specific product ID
    public int OrderID { get; set; }//specific order ID
    public double Price { get; set; } //product price
    public int Amount { get; set; } //choice of specific amount of this product
    public bool IsDeleted { get; set; }

    public override string ToString() => $@"
	ID - {ID},
	Product ID - {ProductID},
    Order ID - {OrderID},
	Amount In Stock - {Amount},

	Product Price= {Price}
    ";
}