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
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        public CartWindow(BlApi.IBl b)
        {
            InitializeComponent();
            bl = b;
        }

        void clickOnHomeBtn(object sender, RoutedEventArgs e)
        {
            new Catalog(bl!).ShowDialog();
            Close();//close this window
        }
    }
}
