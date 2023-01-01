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
    /// Interaction logic for OrderIDEnter.xaml
    /// </summary>
    public partial class OrderIDEnter : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public OrderIDEnter(BlApi.IBl b)
        {
            InitializeComponent();
            bl = b;//new bl
            IdEnter.Text = "";
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            try
            {
                id = int.Parse(IdEnter.Text);//save the entered id as a number
            }
            catch (System.FormatException)
            {
                new ErrorWindow("Enter Order ID Window", "Wrong id number entered").ShowDialog();
            }
            new OrderTracking(id,bl!).ShowDialog();//open order tracking window with entered id
            this.Close();//close current window
        }
        private void tid_previewtextinput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);//only gets numbers for id
        }
    }
}
