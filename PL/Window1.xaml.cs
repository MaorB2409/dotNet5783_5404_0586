﻿using BlApi;
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
        private BO.Product p = new BO.Product();
        public Window1()//add ctor
        {
            InitializeComponent();
            bl = BlApi.Factory.Get();//new bl
            CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
            updateButton.Visibility = Visibility.Collapsed;//update invisible 
        }
        public Window1(ProductForList productForList)//update ctor
        {
            InitializeComponent();
            bl = BlApi.Factory.Get();//new bl
            CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
            addButton.Visibility = Visibility.Collapsed;//add invisible
            updateButton.Visibility = Visibility.Visible;//show update
            tid.Text = productForList.ID.ToString();
            tid.IsReadOnly = true;//cant change id in update 
            tname.Text = productForList.ProductName!.ToString();
            tprice.Text = productForList.Price.ToString();
            tinstock.Text = bl!.Product.ManagerProduct(productForList.ID).InStock.ToString();
            CategoryBox.Text = productForList.Category.ToString();
            
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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl!.Product.AddProduct(p);//add product to BO
            }
            catch (BO.IncorrectInput ex)//IncorrectInput error on the screen 
            {
                new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
               
            }
            catch (BO.IdExistException ex)//IdExistException error on the screen 
            {
                new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
               
            }
            //trigger of a pup op message
            tid.Text = "Enter ID";
            tname.Text = "Enter Name";
            tprice.Text = "Enter Price";
            tinstock.Text = "Enter Amount";//returned previous text
            Close();//close this window


        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl!.Product.UpdateProduct(p);//add product to BO
            }
            catch (BO.IncorrectInput ex)//IncorrectInput error on the screen 
            {
                new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
            }
            catch (BO.IdNotExistException ex)//IdExistException error on the screen 
            {
                new ErrorWindow("Add Product Window\n", ex.Message).ShowDialog();
            }
            tid.Text = "Enter ID";
            tname.Text = "Enter Name";
            tprice.Text = "Enter Price";
            tinstock.Text = "Enter Amount";//returned previous text
            Close();//close this window
        }

        private void tid_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tid != null && tid.Text != "")
            {
                if (int.TryParse(tid.Text, out int val))
                {
                    p.ID = val;
                }
                //else error
            }
        }

        private void tname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tname != null && tname.Text != "")
            {
                p.Name = tname.Text;
            }
        }

        private void tprice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tprice != null && tprice.Text != "")
            {
                if (int.TryParse(tprice.Text, out int val))
                {
                    p.Price = val;
                }
                //else error
            }
        }

        private void tinstock_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tinstock != null && tinstock.Text != "")
            {
                if (int.TryParse(tinstock.Text, out int val))
                {
                    p.InStock = val;
                }
                //else error
            }
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
