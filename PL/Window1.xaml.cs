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
        static public IBl bl = BlApi.Factory.Get();
        private BO.Product p = new BO.Product();
        public Window1(IBl Listbl)
        {
            bl = Listbl;
            InitializeComponent();
            CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
        }
       // private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
       //{
       //     if (tid!=null && tid.Text != "")
       //     {
       //         if(int.TryParse(tid.Text, out int val))
       //         {
       //             p.ID=val;
       //         }
       //         //else error
       //     }
       //     if (tprice!=null && tprice.Text != "")
       //     {
       //         if (int.TryParse(tprice.Text, out int val))
       //         {
       //             p.Price = val;
       //         }
       //         //else error
       //     }
       //     if (tinstock!= null && tinstock.Text != "")
       //     {
       //         if (int.TryParse(tinstock.Text, out int val))
       //         {
       //             p.InStock = val;
       //         }
       //         //else error
       //     }
       //     if (tname != null && tname.Text != "")
       //     {
       //         p.Name = tname.Text;
       //     }
       // }

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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            p.Category = (BO.Enums.Category)CategoryBox.SelectedItem;//save the category picked

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            bl.Product.AddProduct(p);//add product to BO
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            bl.Product.UpdateProduct(p);//add product to BO
        }

    }
}
