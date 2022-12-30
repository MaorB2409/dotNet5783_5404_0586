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
        private BO.Order p = new BO.Order();
        public UpdateOrdersAdmin()
        {
            InitializeComponent();
            bl = BlApi.Factory.Get();//new bl
            statusBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Status));//set combobox values to enums
            updateButton.Visibility = Visibility.Collapsed;//update invisible 
        }
        public UpdateOrdersAdmin(OrderForList orderForList)
        {
            InitializeComponent();
            bl = BlApi.Factory.Get();//new bl
            DataContext = orderForList;

            statusBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Status));//set combobox values to enums
            updateButton.Visibility = Visibility.Visible;//show update
            tid.IsReadOnly = true;//cant change id in update 
            
        }
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
