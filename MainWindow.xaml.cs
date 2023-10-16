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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Занятие_в_аудитории_1_Системное_программирование_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ThreadingButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ThreadingWindow TOkno = new ThreadingWindow();
            TOkno.ShowDialog();
        }

        private void SynchroButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            try
            {
                new SynchroWindow().ShowDialog();
            }
            catch { }
            this.ShowDialog();
        }

        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            TaskWindow TOkno2 = new TaskWindow();
            TOkno2.ShowDialog();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CancelWindow COkno2 = new CancelWindow();
            COkno2.ShowDialog();
        }

        private static Mutex mutex;
        private const String mutexName = "SPNP_MPV_MUTEX";
        private void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mutex = Mutex.OpenExisting(mutexName);
            }
            catch { }
            if (mutex != null)
            {
                if (!mutex.WaitOne(1))
                {
                    string message = "Запущено другой экземпляр окна.";
                    MessageBox.Show(message);
                    return;
                }
                else
                {

                }
            }
            else mutex = new Mutex(true, mutexName);
            this.Hide();
            try
            {
                new ProcessWindow().ShowDialog();
            }
            catch { }
            this.ShowDialog();
            mutex.ReleaseMutex();
        }

        private void ChainingButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new ChainingWindow().ShowDialog();
            this.ShowDialog();
        }

        private void DLLButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new DLLWindow().ShowDialog();
            this.Show();
        }
    }
}
