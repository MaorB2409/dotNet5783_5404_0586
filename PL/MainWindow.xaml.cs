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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToOtherWindow_Click(object sender, RoutedEventArgs e)
        {
            new ListView(bl!).ShowDialog();
            Close();//close this window
        }

        private void TrackOrderWindow_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void NewOrderWindow_Click(object sender, RoutedEventArgs e)
        {
            new Catalog().ShowDialog();
            Close();//close this window

        }

        private void CartWindow_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
