using BO;
using PL.PO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace PL
{
    /// <summary>
    /// Interaction logic for Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        ObservableCollection<PO.ProductItem> productList = new();
        //private PO.ProductItem p = new();
        PO.Cart myCart = new();

      
        public Catalog(PO.Cart cart,BlApi.IBl b)
        {
            InitializeComponent();
            bl = b;//new bl
            myCart = cart;
            productList.Clear();
            try
            {
                productList = PL.Tools.IEnumerableToObservable(bl!.Product.GetCatalog());//save the catalog collection from BO in PO obsv collec
            }
            catch (BO.IdNotExistException ex)
            {
                new ErrorWindow("Catalog Window\n", ex.Message).ShowDialog();
            }
            catalogGrid.DataContext=productList;//set data context of catalog as the product list
            AttributeSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
            AddProductToCart.Visibility = Visibility.Visible;


        }
        private void AttributeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Enums.Category c = (BO.Enums.Category)AttributeSelector.SelectedItem;//save the category picked

            if (c == Enums.Category.NoCategory || AttributeSelector.SelectedItem==null)//if selected to view all products 
            {
                try
                {
                    productList=PL.Tools.IEnumerableToObservable(bl?.Product.GetCatalog()!);//get catalog products from BO
                }
                catch (BO.IdNotExistException ex)
                {
                    new ErrorWindow("Catalog Window\n", ex.Message).ShowDialog();
                }
                AttributeSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//show all of combobox options
                catalogGrid.DataContext = productList;//set data context of catalog as the product list
                return;
            }
            if (c is Enums.Category ca)//if selected a category
            {
                try
                {
                    productList =PL.Tools.IEnumerableToObservable(from p in bl?.Product.GetCatalog()//get all products
                                                                  where p.Category == c
                                                                  select p);//show filtered list
                }
                catch (BO.IdNotExistException ex)
                {
                    new ErrorWindow("Catalog Window\n", ex.Message).ShowDialog();
                }
            }
            catalogGrid.DataContext = productList;//set data context of catalog as the product list
        }
        private void ProductItemView_click(object sender, MouseButtonEventArgs e)
        {
            if (catalogGrid.SelectedItem is PO.ProductItem productItem)
            {
                new ProductItemView(productItem,myCart,bl!).ShowDialog();
            }
        }
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (catalogGrid.SelectedItem is PO.ProductItem productItem)
            {
                try
                {
                    myCart=PL.Tools.CastBoCToPo(bl!.Cart.AddToCart(PL.Tools.CastPoCToBo(myCart), productItem.ID));//add the selected product to cart
                }
                catch (BO.IdNotExistException ex)
                {
                    new ErrorWindow("Cart Window Window", ex.Message).ShowDialog();
                }
                catch (BO.UnfoundException ex)
                {
                    new ErrorWindow("Cart Window Window", ex.Message).ShowDialog();
                }
                catch (BO.Exceptions ex)
                {
                    new ErrorWindow("Cart Window Window", ex.Message).ShowDialog();
                }
                catch (BO.IdExistException ex)
                {
                    new ErrorWindow("Cart Window Window", ex.Message).ShowDialog();
                }
            }//add the product to cart
        }
        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();//go to home window 
            Close();//close this window
        }
        private void CartBtn_Click(object sender, RoutedEventArgs e)
        {
        
            new CartWindow(myCart!, bl!).ShowDialog();//go to cart window 
            Close();//close this window
          

        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

            new MainWindow().ShowDialog();//go to home window 
            Close();//close this window
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility

            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_back.Visibility = Visibility.Collapsed;
                tt_cart.Visibility = Visibility.Collapsed;

            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_cart.Visibility = Visibility.Visible;
                tt_back.Visibility = Visibility.Visible;

            }
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 1;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 0.3;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
