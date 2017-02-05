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

namespace MSMoney
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel context;

        public MainWindow()
        {
            InitializeComponent();
            context = (MainViewModel)DataContext;
            context.Init();
            LVList.ItemsSource = context.Elements;
        }

        private void BTNSave_Click(object sender, RoutedEventArgs e)
        {
            if (!context.SaveNewEntry())
                MessageBox.Show("Cast Exception :(");
            else
            {
                TBAmount.Text = "";
                TBName.Text = "";
                TBPrice.Text = "";
                TBName.Focus();
                CollectionViewSource.GetDefaultView(LVList.ItemsSource).Refresh();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            context.SaveData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TBAmount.Text = "";
            TBName.Text = "";
            TBPrice.Text = "";
            TBName.Focus();
        }

        private void TBName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TBPrice.Focus();
        }

        private void TBPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TBAmount.Focus();
            if (e.Key == Key.OemPeriod)
                MessageBox.Show("Periods will be ignored");
        }

        private void TBAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                BTNSave.Focus();
        }
    }
}
