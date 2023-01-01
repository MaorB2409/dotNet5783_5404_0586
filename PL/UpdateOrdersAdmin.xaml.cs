using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for UpdateOrdersAdmin.xaml
    /// </summary>
    public partial class UpdateOrdersAdmin : Window
    {
        
        BlApi.IBl? bl = BlApi.Factory.Get();
        private PO.OrderForList o = new();
        public UpdateOrdersAdmin(BlApi.IBl? b)
        {
            InitializeComponent();
            bl = b;//new bl
            DataContext = o;
            statusBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Status));//set combobox values to enums
            updateShip.Visibility = Visibility.Collapsed;//update invisible 
            updateOrder.Visibility = Visibility.Collapsed;//update invisible 
            updateDelivery.Visibility = Visibility.Collapsed;//update invisible 

        }
        public UpdateOrdersAdmin(OrderForList orderForList, BlApi.IBl? b)
        {
            InitializeComponent();
            bl = b;//new bl
            //BO.Order ord = bl!.Order.GetBoOrder(orderForList.ID);//save the matching bo order for orderforlist
            o = new PO.OrderForList() { ID = orderForList.ID, Name = orderForList.Name, TotalPrice = orderForList.TotalPrice, Status = orderForList.Status, Amount = orderForList.Amount };
            DataContext = o;
            statusBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Status));//set combobox values to enums
            updateShip.Visibility = Visibility.Visible;//show update
            updateOrder.Visibility = Visibility.Visible;//show update
            updateDelivery.Visibility = Visibility.Visible;//show update
            tid.IsReadOnly = true;//cant change id in update 
            tname.IsReadOnly = true;
            tamount.IsReadOnly = true;
            tprice.IsReadOnly = true;
        }
        
    }
}
