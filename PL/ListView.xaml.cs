using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public ListView(BlApi.IBl? Mainbl)
        {
            bl = Mainbl;
            InitializeComponent();
            try
            {
                ItemListview.ItemsSource = bl?.Product.GetProductsForList();//get products for list from BO
                OrderListview.ItemsSource = bl?.Order.GetAllOrderForList();//get orders for list from BO
            }
            catch (BO.Exceptions ex)//id is null error on screen
            {
                new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
            }
            catch(BO.IdNotExistException ex){
                new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
            }
            catch (BO.IncorrectInput ex){
                new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
            }
            AttributeSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
        }

        private void AttributeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Enums.Category c = (BO.Enums.Category)AttributeSelector.SelectedItem;//save the category picked

            if (c == BO.Enums.Category.NoCategory)//if selected to view all products 
            {
                try
                {
                    ItemListview.ItemsSource = bl?.Product.GetProductsForList();//original list with no filter
                }
                catch (BO.Exceptions ex)
                {
                    new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
                }
                AttributeSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//show all of combobox options
                return;
            }
            if (c is BO.Enums.Category ca)
            {
                try
                {
                    ItemListview.ItemsSource = bl?.Product.GetProductsForList().Select(x => x!.Category == ca);//show filtered list
                }
                catch (BO.Exceptions ex)
                {
                    new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
                }
            }

            try
            {
                ItemListview.ItemsSource = from p in bl?.Product.GetProductsForList()//get all products
                                           where p.Category == c
                                          select p;//selected all products of selected category
            }
            catch (BO.Exceptions ex)
            {
                new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
             new Window1(bl!).ShowDialog();
            try
            {
                ItemListview.ItemsSource = bl?.Product.GetProductsForList();//update list view after add
            }
            catch (BO.Exceptions ex)
            {
                new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
            }
        }

        private void ItemListview_updates(object sender, MouseButtonEventArgs e)
        {
            if(ItemListview.SelectedItem is ProductForList productForList)
            {
                new Window1(bl!).ShowDialog();
            }
            try
            {
                ItemListview.ItemsSource = bl?.Product.GetProductsForList();//update list view after add
            }
            catch (BO.Exceptions ex)
            {
                new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
                //id is null error on screen
            }

        }
        private void Orders_updates(object sender, MouseButtonEventArgs e)
        {
            if (OrderListview.SelectedItem is OrderForList orderForList)
            {
                new UpdateOrdersAdmin(bl!).ShowDialog();
            }
            try
            {
                OrderListview.ItemsSource = bl?.Order.GetAllOrderForList();//update order list view 
            }
            catch (BO.Exceptions ex)
            {
                new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
                //id is null error on screen
            }

        }

        void clickOnHomeBtn(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
            Close();//close this window
        }



    }
}
