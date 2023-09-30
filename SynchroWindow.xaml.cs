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
    /// Логика взаимодействия для SynchroWindow.xaml
    /// </summary>
    public partial class SynchroWindow : Window
    {
        private double sum;
        private int threadCount;
        private object sumLocker = new();
        private bool lastThreadProcessed = false;

        public SynchroWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            sum = 100;
            LogTextBlock.Text = String.Empty;
            threadCount = 12;
            lastThreadProcessed = false;
            for (int i = 0; i < threadCount; i++)
            {
                new Thread(AddPercent).Start(new MonthData { Month = i + 1 });
            }
        }

        private void AddPercent(object? data)
        {
            var monthData = data as MonthData;
            double localSum;
            lock (sumLocker)
            {
                localSum = sum = sum * 1.1;
            }

            Random random = new Random();
            double randomPercent = random.NextDouble() * 20;

            monthData.Perc = randomPercent;

            Dispatcher.Invoke(() =>
            {
                LogTextBlock.Text += $"{monthData?.Month}. {localSum} ({monthData?.Perc:F2}%)\n";
            });

            if (Interlocked.Decrement(ref threadCount) == 0 && !lastThreadProcessed)
            {
                Thread.Sleep(1);
                lastThreadProcessed = true;
                Dispatcher.Invoke(() => LogTextBlock.Text += $"---------\nresult = {sum}");
            }
        }

        class MonthData
        {
            public int Month { get; set; }
            public double Perc { get; set; }
        }

        private void AddPercent4()
        {
            Thread.Sleep(200);
            lock (sumLocker)
            {
                sum = sum * 1.1;
            }
                Dispatcher.Invoke(() => { LogTextBlock.Text += $"{sum}\n"; });
        }
        private void AddPercent3()
        {
            lock (sumLocker)
            {
                double localSum = sum;
                Thread.Sleep(200);
                localSum *= 1.1;
                sum = localSum;
                Dispatcher.Invoke(() => { LogTextBlock.Text += $"{sum}\n"; });
            }
            }
            private void AddPercent2()
        {
            Thread.Sleep(200);
            double localSum = sum;
            localSum *= 1.1;
            sum = localSum;
            Dispatcher.Invoke(() => { LogTextBlock.Text += $"{sum}\n"; });
        }
        private void AddPercent1()
        {
            double localSum = sum;
            Thread.Sleep(200);
            localSum *= 1.1;
            sum = localSum;
            Dispatcher.Invoke(() => { LogTextBlock.Text += $"{sum}\n"; });
        }
    }
}
