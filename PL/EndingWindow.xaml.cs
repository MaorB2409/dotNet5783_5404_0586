﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace PL////>ImageSource="/e8faa395ecab57e4f66a0fad35c0d073.jpg"/>
{
    /// <summary>
    /// Interaction logic for EndingWindow.xaml
    /// </summary>
    public partial class EndingWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        PO.Cart myCart = new();
        public EndingWindow(int orderId, PO.Cart cart, BlApi.IBl? b)
        {
            InitializeComponent();
            bl = b;
            myCart = cart;
            orderNum.DataContext = orderId;
        }

        void clickOnHomeBtn(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
            Close();//close this window
        }
        void BackToShop(object sender, RoutedEventArgs e)
        {
            new Catalog(myCart,bl!).ShowDialog();
            Close();//close this window
        }

        private void track_Click(object sender, RoutedEventArgs e)
        {
            new TrackOrdersThreading(bl).ShowDialog();  
            //this.Close();
        }
    }
}
