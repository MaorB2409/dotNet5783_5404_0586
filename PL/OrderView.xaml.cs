using BO;
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
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        PO.Order o = new();
        public OrderView(int id, BlApi.IBl? b)
        {
            InitializeComponent();
            bl = b;//new bl
            try
            {
                BO.Order ord = bl?.Order.GetBoOrder(id)!;//get the order from BO with matching id
            }
            catch (BO.IdNotExistException ex)
            {
                new ErrorWindow("Order View Window\n", ex.Message).ShowDialog();
            }
            catch (BO.Exceptions ex)
            {
                new ErrorWindow("Order View Window\n",ex.Message).ShowDialog();
            }
            o = new PO.Order();
            //o = BoToPoOrder(ord);//convert to PO order 
            DataContext = o;


        }

        void clickBackBtn(object sender, RoutedEventArgs e)
        {
            new OrderIDEnter(bl!).ShowDialog();
            Close();//close this window
        }
    }
}
