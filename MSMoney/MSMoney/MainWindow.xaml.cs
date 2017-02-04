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
        }

        private void BTNSave_Click(object sender, RoutedEventArgs e)
        {
            context.SaveNewEntry();
            TBAmount.Text = "";
            TBName.Text = "";
            TBPrice.Text = "";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            context.SaveData();
        }
    }
}
