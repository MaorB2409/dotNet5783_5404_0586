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
        public Catalog(BlApi.IBl b)
        {
            InitializeComponent();
            bl = b;//new bl
            AttributeSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
            try
            {
                ItemListview.ItemsSource = bl?.Product.GetCatalog();//get catalog products from BO
            }
            catch (BO.IdNotExistException ex)
            {
                new ErrorWindow("Catalog Window\n", ex.Message).ShowDialog();
            }

        }
        private void AttributeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Enums.Category c = (BO.Enums.Category)AttributeSelector.SelectedItem;//save the category picked

            if (c == BO.Enums.Category.NoCategory)//if selected to view all products 
            {
                try
                {
                    ItemListview.ItemsSource = bl?.Product.GetCatalog();//original list with no filter
                }
                catch (BO.IdNotExistException ex)
                {
                    new ErrorWindow("Catalog Window\n", ex.Message).ShowDialog();
                }
                AttributeSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//show all of combobox options
                return;
            }
            if (c is BO.Enums.Category ca)
            {
                try
                {
                    ItemListview.ItemsSource = bl?.Product.GetCatalog().Select(x => x!.Category == ca);//show filtered list
                }
                catch (BO.IdNotExistException ex)
                {
                    new ErrorWindow("Catalog Window\n", ex.Message).ShowDialog();
                }
            }

            try
            {
                ItemListview.ItemsSource = from p in bl?.Product.GetCatalog()//get all products
                                           where p.Category == c
                                           select p;//selected all products of selected category
            }
            catch (BO.IdNotExistException ex)
            {
                new ErrorWindow("Catalog Window\n", ex.Message).ShowDialog();
            }
        }
        private void ItemListview_click(object sender, MouseButtonEventArgs e)
        {
            if (ItemListview.SelectedItem is ProductItem productItem)
            {
                new ProductItemView(bl!).ShowDialog();
            }
            try
            {
                ItemListview.ItemsSource = bl?.Product.GetCatalog();//update list view after add
            }
            catch (BO.IdNotExistException ex)
            {
                new ErrorWindow("Catalog Window\n", ex.Message).ShowDialog();
                //id is null error on screen
            }

        }


        void clickOnCart(object sender, RoutedEventArgs e)
        {
            new CartWindow(bl!).ShowDialog();
            Close();//close this window
        }
    }
}
