using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal;

static internal class XmlTools
{
    static string suffixPath = @"..\xml\";

        #region XElement
        #region load to file
        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(suffixPath + filePath))
                {
                    return XElement.Load(suffixPath + filePath);
                }
                else
                {
                    XElement rootElem = new XElement(filePath);
                    if (filePath == @"config.xml") { }
                    //rootElem.Add(new XElement("droneRunningNum", 1)); //להוריד
                    rootElem.Save(suffixPath + filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DalApi.LoadingException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion
        #region save to file
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(suffixPath + filePath);
            }
            catch (Exception ex)
            {
                throw new DalApi.LoadingException(suffixPath + filePath, $"fail to create xml file: {suffixPath + filePath}", ex);
            }
        }

        #endregion
        #endregion

        #region XmlSerializer
        //save a complete list in a specific file- throw exception in case of problems..
        //for the using with XMLSerializer..
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(suffixPath + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DalApi.LoadingException(suffixPath + filePath, $"fail to create xml file: {suffixPath + filePath}", ex);
            }
        }

        //load a complete list from a specific file- throw exception in case of problems..
        //for the using with XMLSerializer..
        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
            if (File.Exists(suffixPath + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(suffixPath + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file)!;
                    file.Close();
                    return list!;
                }
            else
            {
                return new List<T>();
            }
        }
            catch (Exception ex)
            {
                throw new DalApi.LoadingException(suffixPath + filePath, $"fail to load xml file: {suffixPath + filePath}", ex);
            }
        }



    public static T? ToEnumerable<T> (this XElement element, string name) where T : struct, Enum => 
        Enum.TryParse<T>((string?)element.Element(name), out var result) ? (T?)result : null;

    public static DateTime? ToDateTimeNullable (this XElement element, string name) =>
        DateTime.TryParse((string?)element.Element(name), out var result) ? (DateTime?)result : null;

    public static double? ToDoubleNullable(this XElement element, string name) =>
        double.TryParse((string?)element.Element(name), out var result) ? (double?)result : null;

    public static int? ToInt(this XElement element, string name) =>
      int.TryParse((string?)element.Element(name), out var result) ? (int?)result : null;
    public static bool? ToBoolNullable(this XElement element, string name) =>
        bool.TryParse((string?)element.Element(name), out var result) ? (bool?)result : null;

    public static int? ToIntlNullable(this XElement element, string name) =>
        int.TryParse((string?)element.Element(name), out var result) ? (int?)result : null;

    public static int? ToInt(this XElement element, string name) =>
      int.TryParse((string?)element.Element(name), out var result) ? (int?)result : null;


    public static DO.Enums.Category ToCategory(string name)
    {
        if (name == "Office")
        {
            return DO.Enums.Category.Office;
        }
        if (name == "Outdoors")
        {
            return DO.Enums.Category.Outdoors;
        }
        if (name == "Bedroom")
        {
            return DO.Enums.Category.Bedroom;
        }
        if (name == "Kitchen")
        {
            return DO.Enums.Category.Kitchen;
        }
        if (name == "LivingRoom")
        {
            return DO.Enums.Category.LivingRoom;
        }
        else
        {
            return DO.Enums.Category.DiningRoom;
        }
    }


    #endregion
}
