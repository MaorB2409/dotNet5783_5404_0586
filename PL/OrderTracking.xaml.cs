using BO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for OrderTracking.xaml
    /// </summary>
    public partial class OrderTracking : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        BO.OrderTracking orderTracking = new();
        public OrderTracking(BlApi.IBl? b)//empty ctor
        {
            InitializeComponent();
            bl = b;//new bl
            DataContext = orderTracking;


        }
        public OrderTracking(int id, BlApi.IBl? b)
        {
            InitializeComponent();
            bl = b;//new bl

            try
            {
                orderTracking = bl?.Order.GetOrderTracking(id)!;
            }
            catch (BO.IdNotExistException ex)
            {
                new ErrorWindow("Order Tracking Window\n", ex.Message).ShowDialog();
            }
            DataContext = orderTracking;
            tid.Text = orderTracking.ID.ToString();
            tid.IsReadOnly = true;//cant change id in update 
            StatusBox.Text = orderTracking.Status.ToString();
            StatusBox.IsReadOnly = true;

        }

        private void OrderDetails_Click(object sender, RoutedEventArgs e)
        {
            new OrderView(orderTracking.ID,bl!).ShowDialog();
            Close();//close this window
        }
    }
}
