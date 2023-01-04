﻿using System;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        PO.Cart myCart = new();
        public RegisterWindow(PO.Cart cart,BlApi.IBl? b)
        {
            InitializeComponent();
            bl = b;
            myCart = cart;
            DataContext= myCart;//set data context

        }
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = 1002;//save the order id
            new EndingWindow(id).ShowDialog();//show ending window 
        }
    }
}