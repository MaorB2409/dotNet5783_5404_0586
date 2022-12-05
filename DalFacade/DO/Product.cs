namespace DO;

/// <summary>
/// info of specific item in catalog or for the admin 
/// </summary>

public struct Product
{
    public int ID { get; set; } //product id
    public string Name { get; set; } //product name
    public double Price { get; set; } //product price
    public Enums.Category Category { get; set; } //product category
    public int InStock { get; set; } //how many are in stock
                
    public bool IsDeleted { get; set; } //true if deleted
    
    public override string ToString() => this.ToStringProperty();
 //   public override string ToString() => $@"
	//ID - {ID},
	//Product Name - {Name},
	//Category - {Category},
	//Amount In Stock - {InStock},
 //   ";
}