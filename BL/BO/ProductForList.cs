﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class ProductForList
{
    public int ID { get; set; } //item ID
    public string? ProductName { get; set; }
    public double Price { get; set; }
    public Enums.Category Category { get; set; }
    public override string ToString() => this.ToStringProperty();
}
