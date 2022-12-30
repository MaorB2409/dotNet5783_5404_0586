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
        BO.Order o = new();
        public OrderView(int id, BlApi.IBl? b)
        {
            InitializeComponent();
            bl = b;//new bl
            try
            {
                o = bl?.Order.GetBoOrder(id)!;//get the order from BO with matching id
            }
            catch (BO.IdNotExistException ex)
            {
                new ErrorWindow("Order View Window\n", ex.Message).ShowDialog();
            }
            catch (BO.Exceptions ex)
            {
                new ErrorWindow("Order View Window\n",ex.Message).ShowDialog();
            }
            DataContext = o;

            orderdate.DisplayDate = (System.DateTime)o.OrderDate!;
            shipdate.DisplayDate = (System.DateTime)o.ShipDate!;
            deliverydate.DisplayDate = (System.DateTime)o.DeliveryDate!;
        }
    }
}
