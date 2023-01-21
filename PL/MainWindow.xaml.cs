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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        PO.Cart myCart=new();
        public MainWindow()
        {
            InitializeComponent();
            bl = BlApi.Factory.Get();
        }

        private void ToOtherWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();//close this window
            new ManagerEnterPassword(bl!).ShowDialog();
            
        }

        private void TrackOrderWindow_Click(object sender, RoutedEventArgs e)
        {
            new OrderIDEnter(myCart,bl!).ShowDialog();//open order id window to view an order and track it
        }

        private void NewOrderWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();//close this window
            new Catalog(myCart,bl!).ShowDialog();
            

        }


    }
}
