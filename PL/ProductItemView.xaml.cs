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
            public ProductItemView()//empty ctor
            {
                InitializeComponent();
                bl = BlApi.Factory.Get();//new bl
                CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
            }
            public ProductItemView(ProductItem productItem)
            {
                InitializeComponent();
                bl = BlApi.Factory.Get();//new bl
                CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));//set combobox values to enums
                tid.Text = productItem.ID.ToString();
                tid.IsReadOnly = true;//cant change id in update 
                tname.Text = productItem.ProductName!.ToString();
                tname.IsReadOnly = true;
                tprice.Text = productItem.Price.ToString();
                tprice.IsReadOnly = true;
                tinstock.Text = bl!.Product.ManagerProduct(productItem.ID).InStock.ToString();
                tinstock.IsReadOnly = true;
                CategoryBox.Text = productItem.Category.ToString();
                CategoryBox.IsReadOnly = true;
                tAmount.Text = productItem.Amount.ToString();
                tAmount.IsReadOnly = true;

            }
        }
}
