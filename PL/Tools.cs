using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;

static class Tools
{
    public static PO.Product CastBoProductToPo(BO.Product p)
    {
        PO.Product poProduct = new PO.Product();
        poProduct.Name = p.Name;
        poProduct.Price = p.Price;
        poProduct.ID=p.ID;
        poProduct.Price=p.Price;
        poProduct.InStock = p.InStock;
        poProduct.Category = p.Category;
        return poProduct;
    }
    public static BO.Product CastPoProductToBo(PO.Product p)
    {
        BO.Product prod = new();
        prod.Name = p.Name;
        prod.Price = p.Price;
        prod.ID = p.ID;
        prod.Price = p.Price;
        prod.InStock = p.InStock;
        prod.Category = p.Category;
        return prod;
    }
    public static BO.Order CastPoOrderToBo(PO.Order p)
    {
        BO.Order ord = new();
        ord.CostumerAddress = p.CostumerAddress;
        ord.CostumerName= p.CostumerName;
        ord.CostumerEmail = p.CostumerEmail;
        ord.DeliveryDate = p.DeliveryDate;
        ord.OrderDate = p.OrderDate;
        ord.ShipDate = p.ShipDate;
        ord.ID = p.ID;
        ord.Status=p.Status;
        ord.orderItems=p.OrderItems;
        ord.TotalPrice=p.TotalPrice;
        return ord;
    }
    public static PO.Order CastBoOrderToPo(BO.Order p)
    {
        PO.Order ord = new();
        ord.CostumerAddress = p.CostumerAddress;
        ord.CostumerName = p.CostumerName;
        ord.CostumerEmail = p.CostumerEmail;
        ord.DeliveryDate = p.DeliveryDate;
        ord.OrderDate = p.OrderDate;
        ord.ShipDate = p.ShipDate;
        ord.ID = p.ID;
        ord.Status = p.Status;
        ord.OrderItems = p.orderItems;
        ord.TotalPrice = p.TotalPrice;
        return ord;
    }
    public static PO.OrderForList CastBoOrderFLToPo(BO.OrderForList p)
    {
        PO.OrderForList ord = new();
        ord.Status = p.Status;
        ord.ID = p.ID;
        ord.TotalPrice = p.TotalPrice;
        ord.Amount=p.Amount;
        ord.Name = p.Name;
        return ord;
    }
    public static BO.OrderForList CastPoOrderFLToBo(PO.OrderForList p)
    {
        BO.OrderForList ord = new();
        ord.Status = p.Status;
        ord.ID = p.ID;
        ord.TotalPrice = p.TotalPrice;
        ord.Amount = p.Amount;
        ord.Name = p.Name;
        return ord;
    }
    public static BO.ProductItem CastPoOrderPIToBo(PO.ProductItem p)
    {
        BO.ProductItem prod = new();
        prod.Amount = p.Amount;
        prod.ProductName=p.ProductName;
        prod.Price=p.Price;
        prod.ID=p.ID;
        prod.InStock=p.InStock;
        prod.Category=p.Category;
        prod.Price=p.Price;
        return prod;
    }
    public static PO.ProductItem CastBoOrderPIToPo(BO.ProductItem p)
    {
        PO.ProductItem prod = new();
        prod.Amount = p.Amount;
        prod.ProductName = p.ProductName;
        prod.Price = p.Price;
        prod.ID = p.ID;
        prod.InStock = p.InStock;
        prod.Category = p.Category;
        prod.Price = p.Price;
        return prod;
    }
}
