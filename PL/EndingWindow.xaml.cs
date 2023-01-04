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

namespace PL////>ImageSource="/e8faa395ecab57e4f66a0fad35c0d073.jpg"/>
{
    /// <summary>
    /// Interaction logic for EndingWindow.xaml
    /// </summary>
    public partial class EndingWindow : Window
    {
        public EndingWindow()
        {
            InitializeComponent();
        }

        void clickOnHomeBtn(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
            Close();//close this window
        }
    }
}
