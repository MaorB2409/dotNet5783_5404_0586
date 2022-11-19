using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class BoProduct
{
    public int ID { get; set; } //product id
    public string Name { get; set; } //product name
    public double Price { get; set; } //product price
    public Enums.Category Category { get; set; } //product category
    public int InStock { get; set; } //how many are in stock

    public bool IsDeleted { get; set; } //true if deleted

    public object GetValueOrDefault()
    {
        throw new NotImplementedException();
    }

    public override string ToString() => $@"
	ID - {ID},
	Product Name - {Name},
	Category - {Category},
	Amount In Stock - {InStock},
    ";
}
