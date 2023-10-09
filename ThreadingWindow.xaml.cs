using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
        private static Mutex? mutex;
        private static string mutexName = "TW_MUTEX";
        public ThreadingWindow()
        {
            CheckLunch();
            InitializeComponent();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            mutex?.ReleaseMutex();
        }
        private void CheckLunch()
        {
            try { mutex = Mutex.OpenExisting(mutexName); } catch { }

            if (mutex is null)
            {
                mutex = new Mutex(true, mutexName);
            }
            else if (!mutex.WaitOne(1))
            {
                MessageBox.Show("Есть уже запущенное окно!");
                throw new ApplicationException();
            }
        }

        private void StartButton1_Click(object sender, RoutedEventArgs e)
        {
            // Зависание интерфейса
            new Thread(IncrementProgress).Start();
        }

        private void StopButton1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartButton2_Click(object sender, RoutedEventArgs e)
        {
            new Thread(IncrementProgress).Start();
        }

        private void StopButton2_Click(object sender, RoutedEventArgs e)
        {

        }
        private void IncrementProgress()
        {
            for (int i = 0; i < 10; i++)
            {
                ProgressBar2.Value = i * 10;
                Thread.Sleep(300);
            }
            ProgressBar2.Value = 100;
        }
        private bool isStopped3 { get; set; }
        private void StartButton3_Click(object sender, RoutedEventArgs e)
        {
            new Thread(IncrementProgress3).Start();
            isStopped3 = false;
        }

        private void StopButton3_Click(object sender, RoutedEventArgs e)
        {
            isStopped3 = true;
        }
        private void IncrementProgress3()
        {
            for (int i = 0; i <= 10 && !isStopped3; i++)
            {
                this.Dispatcher.Invoke(() => ProgressBar3.Value = i * 10);
                Thread.Sleep(300);
            }
        }
        private bool isStopped4 { get; set; }
        private Thread thread4;
        private void StartButton4_Click(object sender, RoutedEventArgs e)
        {
            if (thread4 == null)
            {
                isStopped4 = false;
                thread4 = new Thread(IncrementProgress4);
                thread4.Start();
                StartButton4.IsEnabled = false;
            }
        }

        private void StopButton4_Click(object sender, RoutedEventArgs e)
        {
            stopHadle();
        }
        private void stopHadle()
        {
            isStopped4 = true;
            thread4 = null;
            StartButton4.IsEnabled = true;
        }
        private void IncrementProgress4()
        {
            for (int i = 0; i <= 10 && !isStopped4; i++)
            {
                this.Dispatcher.Invoke(() => ProgressBar4.Value = i * 10);
                Thread.Sleep(300);
            }
            //thread4 = null;
            //this.Dispatcher.Invoke(() => StartButton4.IsEnabled = true);
            this.Dispatcher.Invoke(stopHadle);
        }

        private Thread thread5;
        CancellationTokenSource cts;
        private void StartButton5_Click(object sender, RoutedEventArgs e)
        {
            int time = Convert.ToInt32(TimeTextBox.Text);
            thread5 = new Thread(IncrementProgress5);
            cts = new();
            thread5.Start(new ThreadData5
            {
                RabotaTime = time,
                CancelToken = cts.Token
            });
            //thread5.Join();

        }

        private void StopButton5_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }
        private void IncrementProgress5(object param)
        {
            if (param is ThreadData5 data)
            {
                for (int i = 0; i <= 10; i++)
                {
                    this.Dispatcher.Invoke(() => ProgressBar5.Value = i * 10);
                    Thread.Sleep(100 * data.RabotaTime);
                    if (data.CancelToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Thread 5 error(Invalid Argument)");
            }
            //thread4 = null;
            //this.Dispatcher.Invoke(() => StartButton4.IsEnabled = true);
            //this.Dispatcher.Invoke(stopHadle);
        }
        class ThreadData5
        {
            public int RabotaTime { get; set; }
            public CancellationToken CancelToken { get; set; }
        }

        Thread thread61, thread62, thread63;
        CancellationTokenSource cts2;
        private void StartButton6_Click(object sender, RoutedEventArgs e)
        {
            int time1 = Convert.ToInt32(TimeTextBox1.Text);
            int time2 = Convert.ToInt32(TimeTextBox2.Text);
            int time3 = Convert.ToInt32(TimeTextBox3.Text);
            thread61 = new Thread(() => IncrementProgress6(time1, ProgressBar6));
            thread62 = new Thread(() => IncrementProgress6(time2, ProgressBar61));
            thread63 = new Thread(() => IncrementProgress6(time3, ProgressBar62));
            cts2 = new CancellationTokenSource();
            thread61.Start();
            thread62.Start();
            thread63.Start();
            StartButton6.IsEnabled = false;
            
        }
        private void StopButton6_Click(object sender, RoutedEventArgs e)
        {
            cts2.Cancel();
            StartButton6.IsEnabled = true;
        }
        private void IncrementProgress6(int time, ProgressBar progressBar)
        {
            for (int i = 0; i <= 10; i++)
            {
                this.Dispatcher.Invoke(() => progressBar.Value = i * 10);

                Thread.Sleep(100 * time);

                if (cts2.Token.IsCancellationRequested)
                {
                    break;
                }
            }
        }
    }
 }
