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
    /// Логика взаимодействия для CountDownWindow.xaml
    /// </summary>
    public partial class CountDownWindow : Window
    {
        private Mutex mutex;
        public CountDownWindow(Mutex mutex)
        {
            this.mutex = mutex;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(tick);
        }
        private void tick()
        {
            if (!mutex.WaitOne(300))
            {
                Dispatcher.Invoke(() => {
                    progBar1.Value--;
                if (progBar1.Value == 0)
                    {
                        this.DialogResult = false;
                    }
                    else
                    {
                        Task.Run(tick);
                    }
                });
                }
            else
            {
                mutex.ReleaseMutex();
                Dispatcher.Invoke(() => this.DialogResult = true);
            }
        }
    }
}
