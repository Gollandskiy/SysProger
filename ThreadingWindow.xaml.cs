using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Занятие_в_аудитории_1_Системное_программирование_
{
    /// <summary>
    /// Логика взаимодействия для ThreadingWindow.xaml
    /// </summary>
    public partial class ThreadingWindow : Window
    {
        public ThreadingWindow()
        {
            InitializeComponent();
        }

        private void StartButton1_Click(object sender, RoutedEventArgs e)
        {
            // Зависание интерфейса
            for (int i = 0; i < 10; i++)
            {
                ProgressBar1.Value = i * 10;
                Thread.Sleep(300);
            }
            ProgressBar1.Value = 100;
        }

        private void StopButton1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
