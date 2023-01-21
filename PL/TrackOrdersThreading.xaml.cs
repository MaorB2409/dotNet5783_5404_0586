using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for TrackOrdersThreading.xaml
    /// </summary>
    public partial class TrackOrdersThreading : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        ObservableCollection<PO.OrderForList> ordersForList = new();
        IEnumerable<PO.Order> orders;
        PO.Cart empty = new();
        BackgroundWorker worker;
        bool isWork = false;
        DateTime nowTime = DateTime.Now;//save current time 
        bool inAddingProcess = false;
        public TrackOrdersThreading(BlApi.IBl? Mainbl)
        {
            bl = Mainbl;
            InitializeComponent();
            ordersForList.Clear();
            orders = bl!.Order.GetAllOrderForList().Select(x => bl.Order.GetBoOrder((int)x?.ID!)).OrderBy(x => x.OrderDate).Select(x => PL.Tools.CastBoOrderToPo(x));
            try
            {
                ordersForList = PL.Tools.IEnumerableToObservable(bl!.Order.GetAllOrderForList());
            }
            catch (BO.Exceptions ex)//id is null error on screen
            {
                new ErrorWindow("Admin Order Tracking Window\n", ex.Message).ShowDialog();
            }
            catch (BO.IdNotExistException ex)
            {
                new ErrorWindow("Admin Order Tracking Window\n", ex.Message).ShowDialog();
            }
            catch (BO.IncorrectInput ex)
            {
                new ErrorWindow("Admin Order Tracking Window\n", ex.Message).ShowDialog();
            }
            DataContext = ordersForList;
            #region worker
            worker = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };//create new worker
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            #endregion
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var Worker = sender as BackgroundWorker;

            PO.OrderForList? myOFL = new();
            foreach (PO.Order? Item in orders)
            {
                orders= bl!.Order.GetAllOrderForList().Select(x => bl.Order.GetBoOrder((int)x?.ID!)).OrderBy(x => x.OrderDate).Select(x => PL.Tools.CastBoOrderToPo(x));
                if (worker.CancellationPending == true)//if canceled
                {
                    e.Cancel = true;
                    break;
                }
                switch (Item.Status)
                {
                    case BO.Enums.Status.JustOrdered:
                        if (Item.OrderDate?.AddDays(2) >= nowTime)
                        {
                            bl.Order.ShipUpdate(Item.ID);
                            System.Threading.Thread.Sleep(500);
                        }
                        break;

                    case BO.Enums.Status.Shipped:
                        if(Item.ShipDate?.AddDays(3) >= nowTime)
                        {
                            bl.Order.DeliveredUpdate(Item.ID);
                            System.Threading.Thread.Sleep(500);
                            //play sound?
                        }
                        break;
                }
                
                myOFL = ordersForList.FirstOrDefault(x => x.ID == Item.ID);
                if (myOFL != null)
                {
                    myOFL.Status = (BO.Enums.Status)bl.Order.GetBoOrder(Item.ID).Status;
                }

                System.Threading.Thread.Sleep(2000);
            }

            foreach (PO.Order? Item in orders)
            {
                orders = bl!.Order.GetAllOrderForList().Select(x => bl.Order.GetBoOrder((int)x?.ID!)).OrderBy(x => x.OrderDate).Select(x => PL.Tools.CastBoOrderToPo(x));
                if (worker.CancellationPending == true)//if canceled
                {
                    e.Cancel = true;
                    break;
                }
                switch (Item.Status)
                {
                    case BO.Enums.Status.JustOrdered:
                        if (Item.OrderDate?.AddDays(2) >= nowTime)
                        {
                            bl.Order.ShipUpdate(Item.ID);
                            System.Threading.Thread.Sleep(500);
                        }
                        break;

                    case BO.Enums.Status.Shipped:
                        if (Item.ShipDate?.AddDays(3) >= nowTime)
                        {
                            bl.Order.DeliveredUpdate(Item.ID);
                            System.Threading.Thread.Sleep(500);
                        }
                        break;
                }

                myOFL = ordersForList.FirstOrDefault(x => x.ID == Item.ID);
                if (myOFL != null)
                {
                    myOFL.Status = (BO.Enums.Status)bl.Order.GetBoOrder(Item.ID).Status;
                }

                System.Threading.Thread.Sleep(2000);
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            nowTime.AddHours(6);//add 6 hours to time
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            starttracking.IsEnabled = true;
            stoptracking.IsEnabled = true;
            if (!inAddingProcess)
                MessageBox.Show("tracking has stoped!\none of the following might have happend:\n1) stopped by force\n2) no orders left to deliver", "Simulator", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isWork == false)
            {
                isWork = true;
                starttracking.IsEnabled = false;
                stoptracking.IsEnabled = true;
                worker.RunWorkerAsync("argument");
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (isWork == true)
            {
                isWork = false;
                starttracking.IsEnabled = true;
                worker.CancelAsync();// Cancel the asynchronous operation
            }
        }
        private void Orders_view(object sender, MouseButtonEventArgs e)
        {
            if (ItemGrid.SelectedItem is PO.OrderForList orderForList)
            {
                new OrderView(orderForList.ID, empty, bl!,true).ShowDialog();//view the chosen order details
            }
        }
        void clickOnHomeBtn(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
            Close();//close this window
        }
        void clickBackBtn(object sender, RoutedEventArgs e)
        {
            Close();//close this window
            new ListView(bl).ShowDialog();
            
        }

        private void openCatalog_Click(object sender, RoutedEventArgs e)
        {
            inAddingProcess = true;
            worker.CancelAsync();// Cancel the asynchronous operation

            Close();
            new Catalog(empty, bl!).Show();
        }
    }
}