﻿using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
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
        static public IBl bl = BlApi.Factory.Get();
        public ListView(IBl Mainbl)
        {
            bl = Mainbl;
            InitializeComponent();
            ItemListview.ItemsSource = bl.Product.GetProductsForList();
            AttributeSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
        }

        private void AttributeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Enums.Category c = (BO.Enums.Category)AttributeSelector.SelectedItem;//save the category picked

            if (c == BO.Enums.Category.NoCategory)//if selected to view all products 
            {
                ItemListview.ItemsSource = bl.Product.GetProductsForList();//original list with no filter
                AttributeSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//show all of combobox options
                return;
            }
            if (c is BO.Enums.Category ca)
                ItemListview.ItemsSource = bl.Product.GetProductsForList().Select(x => x.Category == ca);//show filtered list



            ItemListview.ItemsSource = from p in bl.Product.GetProductsForList()//get all products
                                       where p.Category==c
                                       select p;//selected all products of selected category
            AttributeSelector.SelectedIndex = -1;//clear the selected item in combobox 
            //AttributeSelector.IsDropDownOpen = false;
            AttributeSelector.Items.Add(BO.Enums.Category.NoCategory);//only show no category 
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window1 w1 = new Window1(bl);
            w1.Show();
            w1.updateButton.Visibility = Visibility.Collapsed;//update invisible 
        }

        private void ItemListview_updates(object sender, MouseButtonEventArgs e)
        {
            Window1 w1 = new Window1(bl);
            w1.Show();
            w1.addButton.Visibility = Visibility.Collapsed;//add invisible
            w1.updateButton.Visibility = Visibility.Visible;//show update
        }

        
    }
}