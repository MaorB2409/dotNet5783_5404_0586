using BO;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for OrderItemView.xaml
    /// </summary>
    public partial class OrderItemView : Window

    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        PO.OrderItem o = new();
        public OrderItemView(BlApi.IBl? b)
        {
            InitializeComponent();
            bl = b;//new bl
            DataContext = o;

        }

        public OrderItemView(OrderItem orderItem, BlApi.IBl? b)
        {
            InitializeComponent();
            bl = b;//new bl
            o=PL.Tools.CastBoOIToPo(orderItem);//matching po for bo
            DataContext = o;
            tOrderID.IsReadOnly = true;//cant change id in update 
            tProductID.IsReadOnly = true;
            tPrice.IsReadOnly = true;
            tAmount.IsReadOnly = true;
            tProductName.IsReadOnly = true;
            tProductPrice.IsReadOnly = true;
        }
    }
}
