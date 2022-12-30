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
    /// Interaction logic for ProductItemView.xaml
    /// </summary>
    public partial class ProductItemView : Window
    {

            BlApi.IBl? bl = BlApi.Factory.Get();
            private BO.ProductItem p = new BO.ProductItem();
            public ProductItemView(BlApi.IBl? b)//empty ctor
            {
                InitializeComponent();
                bl = b;//new bl
                CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
            }
            public ProductItemView(ProductItem productItem, BlApi.IBl? b)
            {
                InitializeComponent();
                bl = b;//new bl
                CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
                CategoryBox.IsReadOnly = true;
                tid.IsReadOnly = true;//cant change id in update 
                tname.IsReadOnly = true;
                tprice.IsReadOnly = true;
                tinstock.IsReadOnly = true;
                CategoryBox.IsReadOnly = true;
                tAmount.IsReadOnly = true;

            }
        }
}
