using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PLConverter
{


    public class StatusToBackgroundColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var o = (PL.PO.OrderForList)value;
            if (o.Status.ToString() == "Shipped")
            {
                return new SolidColorBrush(Colors.LightGreen);

            }
            else if(o.Status.ToString() == "Just Ordered")
            {
                return new SolidColorBrush(Colors.LightSeaGreen);
            }
            else //Recieved
            {
                return new SolidColorBrush(Colors.Green);   
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class FalseToTrueConverterDataGrid : IValueConverter
    {
        //convert from source property type to target property type
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue && parameter is DataGrid && !(parameter as DataGrid).IsGrouping)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //convert from target property type to source property type
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue && parameter is DataGrid && !(parameter as DataGrid).IsGrouping)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class FalseToTrueConverter : IValueConverter
    {
        //convert from source property type to target property type
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue)
            {
                return false; //Visibility.Collapsed;
            }
            else
            {
                return true;
            }
        }

        //convert from target property type to source property type
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue)
            {
                return false; //Visibility.Collapsed;
            }
            else
            {
                return true;
            }
        }
    }
    public class NotVisibilityToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibilityValue = (Visibility)value;
            if (visibilityValue == Visibility.Hidden)
            {
                return Visibility.Visible; //Visibility.Collapsed;
            }
            else
            {
                return Visibility.Hidden;
            }
        }


        //convert from target property type to source property type
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibilityValue = (Visibility)value;
            if (visibilityValue == Visibility.Hidden)
            {
                return Visibility.Visible; //Visibility.Collapsed;
            }
            else
            {
                return Visibility.Hidden;
            }
        }
    }
    public class BoolToVisibilityConverter : IValueConverter
    {
        //convert from source property type to target property type
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue)
            {
                return Visibility.Visible; //Visibility.Collapsed;
            }
            else
            {
                return Visibility.Hidden;
            }
        }


        //convert from target property type to source property type
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibilityValue = (Visibility)value;
            if (visibilityValue is Visibility.Visible)
            {
                return true; //Visibility.Collapsed;
            }
            else
            {
                return false;
            }
        }
    }


    public class ValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isenable = true;
            if (string.IsNullOrEmpty(value.ToString()))
            {
                isenable = false;
            }
            return isenable;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class BooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            foreach (object value in values)
            {
                if ((value is string) && (string)value == "")
                {
                    return false;
                }
            }
            return true;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("BooleanAndConverter is a OneWay converter.");
        }
    }


    public class StatusToProgressBarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var o = (PL.PO.OrderForList)value;
            //ProgressBar progressBar = new ProgressBar();
            Random random = new Random();
            if(!(value is BO.Enums.Status))
            {
                throw new Exception("Wrong Creation!");
            }
            var status = (BO.Enums.Status)value;
            if(status.ToString()== "Just Ordered")
            {
                return random.Next(30);
            }
            else if(status.ToString()== "Shipped")
            {
                return (int)random.Next(30, 80);
            }
            return 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
        //var progressBar = new ProgressBar();
        //if (status == BO.Enums.Status.JustOrdered)
        //{
        //    progressBar.Value = 0;
        //}
        //if (status == BO.Enums.Status.Shipped)
        //{
        //    progressBar.Value = 50;
        //}
        //if (status == BO.Enums.Status.Recieved)
        //{
        //    progressBar.Value = 100;
        //}
        //return progressBar;
    }

}
