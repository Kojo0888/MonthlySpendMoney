using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MSMoney.Model;
using System.Windows.Input;

namespace MSMoney
{
    public class MainViewModel
    {
        public Entry Current { set; get; } = new Entry();



        public void ShowMessageWindow()
        {
            MessageBox.Show("hello worlds!!!!");
        }
    }
}
