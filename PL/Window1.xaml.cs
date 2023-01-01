using BlApi;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        PO.Product p = new();
        public Window1(BlApi.IBl? b)//add ctor
        {
            InitializeComponent();
            bl = b;//new bl
            DataContext = p;
            CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
            updateButton.Visibility = Visibility.Collapsed;//update invisible 
            tid.IsEnabled = false;//cant decide an id for a product to add
        }
        public Window1(ProductForList productForList, BlApi.IBl? b)//update ctor
        {
            InitializeComponent();
            bl = b;//new bl
            BO.Product prod = bl!.Product.ManagerProduct(productForList.ID);//save the matching product or product for list
            p = PL.Tools.CastBoProductToPo(prod);//save matching PO product
            DataContext = p;//set product as data context

            CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
            addButton.Visibility = Visibility.Collapsed;//add invisible
            updateButton.Visibility = Visibility.Visible;//show update
            tid.IsReadOnly = true;//cant change id of a product to update                                            
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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Product BoProd = PL.Tools.CastPoProductToBo(p);//matching BO prod
                bl!.Product.AddProduct(BoProd);//add product to BO
            }
            catch (BO.IncorrectInput ex)//IncorrectInput error on the screen 
            {
                new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
               
            }
            catch (BO.IdExistException ex)//IdExistException error on the screen 
            {
                new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
               
            }
            
            Close();//close this window


        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Product BoProd = PL.Tools.CastPoProductToBo(p);//matching BO prod
                bl!.Product.UpdateProduct(BoProd);//add product to BO
            }
            catch (BO.IncorrectInput ex)//IncorrectInput error on the screen 
            {
                new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
            }
            catch (BO.IdNotExistException ex)//IdExistException error on the screen 
            {
                new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
            }
            
            Close();//close this window
        }

       
        private void tid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(tid.Text=="Enter ID")
            {
                tid.Clear();//clear the default text
            }
        }

        private void tname_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tname.Text == "Enter Name")
            {
                tname.Clear();//clear the default text
            }
        }

        private void tprice_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tprice.Text == "Enter Price")
            {
                tprice.Clear();//clear the default text
            }
        }

        private void tinstock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tinstock.Text == "Enter Amount")
            {
                tinstock.Clear();//clear the default text
            }
        }

      

    }
}
