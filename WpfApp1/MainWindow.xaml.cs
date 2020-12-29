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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Scanner scanner ;
        Elocker elocker;
        public MainWindow()
        {
            InitializeComponent();

            scanner = new Scanner();
            
            elocker = new Elocker();

            int OpenStatus1 = 0;
            int Article1 = 0;
            if (elocker.open("COM1"))
            {
                elocker.QueryBoxStatus(11, out OpenStatus1, out Article1);
                if (OpenStatus1 == 1)
                {
                    Box1.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
                elocker.QueryBoxStatus(12, out OpenStatus1, out Article1);
                if (OpenStatus1 == 1)
                {
                    Box2.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
                elocker.QueryBoxStatus(13, out OpenStatus1, out Article1);
                if (OpenStatus1 == 1)
                {
                    Box3.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
                elocker.QueryBoxStatus(14, out OpenStatus1, out Article1);
                if (OpenStatus1 == 1)
                {
                    Box4.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
                elocker.QueryBoxStatus(15, out OpenStatus1, out Article1);
                if (OpenStatus1 == 1)
                {
                    Box5.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
            }
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (scanner != null)
            {
                scanner.startScanenr(OnReceived);
            }
            else
                MessageBox.Show("fail 0");

        }
        private void OnReceived(string dataInfo)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                scannerInfo.Text = dataInfo;
            }));
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (scanner != null)
            {
                if (scanner.open(comScanner.Text))
                {
                    MessageBox.Show("succeed");
                }
                else
                    MessageBox.Show("fail 2");

            }
            else
                MessageBox.Show("fail ");

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            if (elocker != null)
             {
                if (elocker.openBox(0, Int32.Parse(txtBox.Text)))
                {
                    MessageBox.Show("succeed to open a BOX");
                }
                else
                    MessageBox.Show("fail to open a BOX");
            }
            else
                MessageBox.Show("fail to connect to COM port");
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (elocker != null)
            {
               
                if (elocker.openBox(0, Int32.Parse(Box1.Content.ToString())))
                {
                    MessageBox.Show("succeed");
                }
                else
                    MessageBox.Show("fail ");
            }
        }

      

        private void Box2_Click(object sender, RoutedEventArgs e)
        {
            Button current_box = ((Button)sender);
            current_box.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            //Box2
            // Box2.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 255));
            if (elocker != null)
            {
                if (elocker.openBox(0, Int32.Parse(current_box.Content.ToString())))
                {
                    MessageBox.Show("succeed");
                }
                else
                    MessageBox.Show("fail ");
            }
        }

        private void comLocker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
