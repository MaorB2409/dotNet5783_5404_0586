using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
       

        BlApi.IBl? bl = BlApi.Factory.Get();
        private BO.ProductItem p = new BO.ProductItem();
        public Catalog()//empty ctor
        {
            InitializeComponent();
            bl = BlApi.Factory.Get();//new bl
            CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
            //updateButton.Visibility = Visibility.Collapsed;//update invisible 
        }
        public Catalog(ProductItem productItem)
        {
            InitializeComponent();
            bl = BlApi.Factory.Get();//new bl
            CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
            //addButton.Visibility = Visibility.Collapsed;//add invisible
            //updateButton.Visibility = Visibility.Visible;//show update
            tid.Text = productItem.ID.ToString();
            tid.IsReadOnly = true;//cant change id in update 
            tname.Text = productItem.ProductName!.ToString();
            tprice.Text = productItem.Price.ToString();
            tinstock.Text = bl!.Product.ManagerProduct(productItem.ID).InStock.ToString();
            CategoryBox.Text = productItem.Category.ToString();
            tAmount.Text = productItem.Amount.ToString();

        }
        private void tid_previewtextinput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);//only gets numbers for id
        }

        private void tinstock_previewtextinput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);//only gets numbers for instock
        }

        private void tprice_previewtextinput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);//only gets numbers for price
        }
        private void tname_previewtextinput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^a-z]+").IsMatch(e.Text);//only get letters 
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            p.Category = (BO.Enums.Category)CategoryBox.SelectedItem;//save the category picked

        }

        //private void addButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        bl!.Product.AddProduct(p);//add product to BO
        //    }
        //    catch (BO.IncorrectInput ex)//IncorrectInput error on the screen 
        //    {
        //        new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
        //        //Console.WriteLine("Add Product Window\n");
        //        //Console.WriteLine(ex.Message);
        //        //Console.WriteLine("your input is incorrect\n");
        //        //Console.WriteLine(ex.InnerException?.ToString());
        //    }
        //    catch (BO.IdExistException ex)//IdExistException error on the screen 
        //    {
        //        new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
        //        //Console.WriteLine("Add Product Window\n");
        //        //Console.WriteLine(ex.Message);
        //        //Console.WriteLine("the ID you requested to add already exists\n");
        //        //Console.WriteLine(ex.InnerException?.ToString());
        //    }
        //    //trigger of a pup op message
        //    tid.Text = "Enter ID";
        //    tname.Text = "Enter Name";
        //    tprice.Text = "Enter Price";
        //    tinstock.Text = "Enter Amount";//returned previous text
        //    Close();//close this window


        //}

        //private void updateButton_Click(object sender, RoutedEventArgs e)
        //{
            //try
            //{
            //    bl!.Product.UpdateProduct(p);//add product to BO
            //}
            //catch (BO.IncorrectInput ex)//IncorrectInput error on the screen 
            //{
            //    new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
            //    //Console.WriteLine("Add Product Window\n");
            //    //Console.WriteLine(ex.Message);
            //    //Console.WriteLine("your input is incorrect\n");
            //    //Console.WriteLine(ex.InnerException?.ToString());
            //}
            //catch (BO.IdNotExistException ex)//IdExistException error on the screen 
            //{
            //    new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
            //    //Console.WriteLine("Add Product Window\n");
            //    //Console.WriteLine(ex.Message);
            //    //Console.WriteLine("the ID you requested to update does not exists\n");
            //    //Console.WriteLine(ex.InnerException?.ToString());
            //}
            //tid.Text = "Enter ID";
            //tname.Text = "Enter Name";
            //tprice.Text = "Enter Price";
            //tinstock.Text = "Enter Amount";//returned previous text
            //Close();//close this window
        //}


        private void tid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tid.Text == "Enter ID")
            {
                tid.Clear();//clear the default text
            }
        }

        private void tname_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            tname.Clear();
        }

        private void tprice_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            tprice.Clear();
        }

        private void tinstock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            tinstock.Clear();
        }

       
    }
}
