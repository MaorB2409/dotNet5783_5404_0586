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
        PO.Cart empty = new();
        BackgroundWorker worker;
        DateTime nowTime=DateTime.Now;//save current time 
        public TrackOrdersThreading(BlApi.IBl? Mainbl)
        {
            bl = Mainbl;
            InitializeComponent();
            ordersForList.Clear();
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
            DataContext=ordersForList;
            #region worker
            worker = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };//create new worker
            worker.DoWork += Worker_DoWork;
            //worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            #endregion
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var listOfOrders = (ObservableCollection<PO.OrderForList>)e.Argument;

            //foreach (PO.OrderForList order in listOfOrders)

            for (int i = 1; i < listOfOrders.Count; i++)//go over all orders
            {
                int length = 12;
                int perc = 1;
                if (worker.CancellationPending == true)//if canceled
                {
                    e.Cancel = true;
                    stopwatch.Stop();   
                    e.Result = stopwatch.ElapsedMilliseconds; 
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress
                    nowTime.AddHours(6);//add 6 hours to time
                    System.Threading.Thread.Sleep(2000);//sleep for 2 sec
                    PO.Order matchingOrder = PL.Tools.CastBoOrderToPo(bl.Order.GetBoOrder(listOfOrders[i].ID));//save matching order for curr orderForList
                    if (listOfOrders[i].Status==BO.Enums.Status.JustOrdered && matchingOrder.OrderDate?.AddDays(2) >= nowTime)//if not shipped and passed 2 days from being ordered - ship
                    {
                        bl.Order.ShipUpdate(listOfOrders[i].ID);//update to shipped
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
                    }
                    if (listOfOrders[i].Status == BO.Enums.Status.Shipped && matchingOrder.ShipDate?.AddDays(3) >= nowTime)//if not delivered and passed 3 days from being shipped - deliver
                    {
                        bl.Order.DeliveredUpdate(listOfOrders[i].ID);//update to delivered
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
                    }
                    System.Threading.Thread.Sleep(500);
                    worker.ReportProgress(perc++ * 100 / length);
                }
            }
            e.Result = stopwatch.ElapsedMilliseconds;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;//save the percentage

            DataContext = ordersForList;//reset the screen list
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //worker = null;
            MessageBox.Show("tracking has stoped!\none of the following might have happend:\n1) stopped by force\n2) no orders left to deliver", "Simulator", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy != true)
            {
               
                worker.RunWorkerAsync(ordersForList);// Start the asynchronous operation
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            if (worker.WorkerSupportsCancellation == true)
                // Cancel the asynchronous operation
                worker.CancelAsync();

        }
        private void Orders_view(object sender, MouseButtonEventArgs e)
        {
            if (ItemGrid.SelectedItem is PO.OrderForList orderForList)
            {
                new OrderView(orderForList.ID, empty, bl!).ShowDialog();//view the chosen order details
            }
        }
        void clickOnHomeBtn(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
            Close();//close this window
        }
        void clickBackBtn(object sender, RoutedEventArgs e)
        {
            new ListView(bl).ShowDialog();
            Close();//close this window
        }

        private void openCatalog_Click(object sender, RoutedEventArgs e)
        {
            new Catalog(empty,bl!).Show();
        }
    }
}
