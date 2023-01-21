using BO;
using PL.PO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
        PO.OrderForList ords = new();
        ObservableCollection<PO.ProductForList> productsForList = new();
        ObservableCollection<PO.OrderForList> ordersForList = new();
        public ListView(BlApi.IBl? Mainbl)
        {
            bl = Mainbl;
            InitializeComponent();
            productsForList.Clear();
            ordersForList.Clear();
            try
            {
                productsForList = PL.Tools.IEnumerableToObservable(bl!.Product.GetProductsForList());
                ordersForList = PL.Tools.IEnumerableToObservable(bl!.Order.GetAllOrderForList());
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
            ProductItemGrid.DataContext = productsForList;
            ItemGrid.DataContext = ordersForList;
        }



        private void AttributeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Enums.Category c = (BO.Enums.Category)AttributeSelector.SelectedItem;//save the category picked

            if (c == BO.Enums.Category.NoCategory)//if selected to view all products 
            {
                try
                {

                    productsForList = PL.Tools.IEnumerableToObservable(bl?.Product.GetProductsForList()!);//get catalog products from BO

                    //ItemListview.ItemsSource = bl?.Product.GetProductsForList();//original list with no filter
                }
                catch (BO.Exceptions ex)
                {
                    new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
                }
                AttributeSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//show all of combobox options
                ProductItemGrid.DataContext = productsForList;

                return;
            }
            if (c is BO.Enums.Category ca)
            {
                try
                {
                    //ItemListview.ItemsSource = bl?.Product.GetProductsForList().Select(x => x!.Category == ca);//show filtered list

                    productsForList = PL.Tools.IEnumerableToObservable(from p in bl?.Product.GetProductsForList()//get all products
                                                                   where p.Category == c
                                                                   select p);//show filtered list
                }
                catch (BO.Exceptions ex)
                {
                    new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
                }
                ProductItemGrid.DataContext = productsForList;

            }

            try
            {
                productsForList = PL.Tools.IEnumerableToObservable(from p in bl?.Product.GetProductsForList()//get all products
                                                                   where p.Category == c
                                                                   select p);
            }
            catch (BO.Exceptions ex)
            {
                new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
            }
            ProductItemGrid.DataContext = productsForList;

        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            new Window1(bl!).ShowDialog();
            try
            {
                productsForList = PL.Tools.IEnumerableToObservable(bl?.Product.GetProductsForList()!);
            }
            catch (BO.Exceptions ex)
            {
                new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
            }
            ProductItemGrid.DataContext = productsForList;
        }

        private void ProductItemGrid_updates(object sender, MouseButtonEventArgs e)
        {
            if (ProductItemGrid.SelectedItem is PO.ProductForList productForList)
            {
               new Window1(productForList, bl!).ShowDialog();
            }
            try
            {
                productsForList = PL.Tools.IEnumerableToObservable(bl?.Product.GetProductsForList()!);
            }
            catch (BO.Exceptions ex)
            {
                new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
                //id is null error on screen
            }
            ProductItemGrid.DataContext = productsForList;
        }
        private void Orders_updates(object sender, MouseButtonEventArgs e)
        {
            if (ItemGrid.SelectedItem is PO.OrderForList orderForList)
            {
               new UpdateOrdersAdmin(orderForList, bl!).ShowDialog();
            }
            try
            {
                ordersForList = PL.Tools.IEnumerableToObservable(bl?.Order.GetAllOrderForList()!);
            }
            catch (BO.Exceptions ex)
            {
                new ErrorWindow("List View Window\n", ex.Message).ShowDialog();
                //id is null error on screen
            }
            ItemGrid.DataContext = ordersForList;
        }



        void clickOnHomeBtn(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
            Close();//close this window
        }



        #region group by status
        
        private void GroupByStatus_Click(object sender, RoutedEventArgs e)
        {
            RemoveGroupings_Click(sender, e);//remove prev grouping
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ItemGrid.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            SortDescription sortDscription = new SortDescription("Status", ListSortDirection.Ascending);
            view.GroupDescriptions.Add(groupDescription);
            view.SortDescriptions.Add(sortDscription);
            GroupByStatus.IsEnabled = false;
        }

        private void RemoveGroupings_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ordersForList);
            view.GroupDescriptions.Clear();
            view.SortDescriptions.Clear();
            GroupBack.IsEnabled=false;
        }
        #endregion
        private void track_Click(object sender, RoutedEventArgs e)
        {
            new TrackOrdersThreading(bl).ShowDialog();
        }
    }
}
