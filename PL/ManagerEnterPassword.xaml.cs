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
    /// Interaction logic for ManagerEnterPassword.xaml
    /// </summary>
    public partial class ManagerEnterPassword : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public ManagerEnterPassword(BlApi.IBl b)
        {
            InitializeComponent();
            bl = b;//new bl
            ManagerPassword.Text = "";
        }

        private void password_previewtextinput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);//only gets numbers for id
        }
        private void EnterPressed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) EnterPassword();
        }

        private void EnterPassword()
        {
            if (ManagerPassword.Text == "1234")
            {
                ManagerPassword.Text = "";
                new ListView(bl!).ShowDialog();
            }
        }

        void clickBackBtn(object sender, RoutedEventArgs e)
        {
            Close();//close this window
            new MainWindow().ShowDialog();
            
        }


    }
}