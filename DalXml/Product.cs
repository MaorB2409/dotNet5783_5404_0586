﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;

internal class Product : IProduct
{
    string productPath = @"Product.xml";
    string configPath = @"Config.xml";

    public static DO.Product? GetProduct(XElement p) =>
    p?.ToInt("ID") is null ? null : new DO.Product()
    {
        ID = p.ToInt("ID") ?? 0,
        Name = (string?)(p.Element("Name")?.Value),
        Category = XmlTools.ToCategory(p.Element("Category").Value),
        InStock = p?.ToInt("InStock") ?? 0,
        Price = p?.ToDoubleNullable("Price") ?? 0
    };
    //public static DO.Product? GetProduct(XElement p) =>
    //p?.ToInt("ID") is null ? null : new DO.Product()
    //{
    //    ID = p.ToInt("ID") ?? 0,
    //    Name = (string?)(p.Element("Name")?.Value),
    //    Category = XmlTools.ToCategory(p.Element("Category").Value),
    //    InStock = p?.ToInt("InStock") ?? 0,
    //    Price = p?.ToDoubleNullable("Price") ?? 0,
    //};

    public int Add(DO.Product item)
    {
        XElement productRoot = XmlTools.LoadListFromXMLElement(productPath); //get all the elements from the file

        //check if the customer exists in th file
        var customerFromFile = (from prod in productRoot.Elements()
                                where (prod.Element("ID").Value == item.ID.ToString())
                                select prod).FirstOrDefault()??null;

        //throw an exception
        if (customerFromFile != null)
            throw new DalApi.IdExistException("the product already exists");

        //get running product ID number
        List<RunningNumber> runningList = XmlTools.LoadListFromXMLSerializer<RunningNumber>(configPath);

        var runningNum = (from number in runningList
                          where (number.typeOfnumber == "Product ID running number")
                          select number).FirstOrDefault();
        runningList.Remove(runningNum);//remove the saved number from list
        runningNum.numberSaved++;//add one to the saved number
        runningList.Add(runningNum);//add the number back to list
        int temp = (int)runningNum.numberSaved;//save the running number

        //add the customer to the root element
        productRoot.Add(
            new XElement("Product",
            new XElement("ID", temp),
            new XElement("Name", item.Name),
            new XElement("Price", item.Price),
            new XElement("Category", item.Category),
            new XElement("InStock", item.InStock)));

        //save the root in the file
        XmlTools.SaveListToXMLElement(productRoot, productPath);
        return temp;
    }

    public void Delete(int id)
    {
        List<DO.Product?> productList = XmlTools.LoadListFromXMLSerializer<DO.Product?>(productPath);

        DO.Product temp = (from item in productList
                           where item != null && item?.ID == id
                           select (DO.Product)item).FirstOrDefault();

        if (temp.ID.Equals(default(Order)))
            throw new DalApi.IdNotExistException("the product does not exist");

        productList.Remove(temp);

        XmlTools.SaveListToXMLSerializer(productList, productPath);
    }


   

    public IEnumerable<DO.Product?> GetAll(Func<DO.Product?, bool>? filter = null)
       =>
       filter is null
       ? XmlTools.LoadListFromXMLElement(productPath).Elements().Select(p => GetProduct(p))
       : XmlTools.LoadListFromXMLElement(productPath).Elements().Select(p => GetProduct(p)).Where(filter);



    

        //XElement productRoot = XmlTools.LoadListFromXMLElement (productPath);

        //return (from product in productRoot
        //        where filter(product)
        //        select product).ToList();
    

    public DO.Product GetByFilter(Func<DO.Product?, bool>? filter)
    {
        List<DO.Product?> productList = GetAll().ToList();

        return (from item in productList
                where filter(item)
                select (DO.Product)item).FirstOrDefault();
    }

    public DO.Product GetById(int id)
    {
        List<DO.Product?> productList = GetAll().ToList();

        return (from item in productList
                where item != null && item?.ID == id
                select (DO.Product)item).FirstOrDefault();
        throw new DalApi.IdNotExistException("the product requested does not exist");
    }

    public void Update(DO.Product item)
    {
        DO.Product? temp = GetById(item.ID);//get the product requested to update 
        List<DO.Product?> productList = GetAll().ToList();//get all product from ile
        productList.Remove(temp);//remove the existing product
        productList.Add(item);//add the updated product

        XmlTools.SaveListToXMLSerializer(productList, productPath);//save back into file    }
    }
}
