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
using System.Windows.Shapes;
using System.Threading;

namespace Занятие_в_аудитории_1_Системное_программирование_
{
    /// <summary>
    /// Логика взаимодействия для TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public TaskWindow()
        {
            InitializeComponent();
        }

        private void DemoButton1_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(demo1);
            task.Start();
            Task task2 = Task.Run(demo1);
        }
        private void demo1()
        {
            Dispatcher.Invoke(() => LogTextBlock.Text += "Demo1 Starts\n");
            Thread.Sleep(1000);
            Dispatcher.Invoke(() => LogTextBlock.Text += "Demo1 Finishes\n");

        }
        private async Task<String> demo2()
        {
            LogTextBlock.Text += "demo2 starts\n";
            await Task.Delay(1000);
            return "Done";
        }

        private async void DemoButton2_Click(object sender, RoutedEventArgs e)
        {
            //var res = demo2();
            //String str = res.Result;
            //Task<String> task = demo2();
            //String str = await task;
            //LogTextBlock.Text += $"demo 2 result: {str}\n";
            //LogTextBlock.Text += $"demo2-1 result: {await demo2()} \n";
            //LogTextBlock.Text += $"demo2-2 result: {await demo2()} \n";
            Task<String> task1 = demo2();
            Task<String> task2 = demo2();
            String res1 = $"demo2-1 result: {await task1}\n";
            LogTextBlock.Text += res1;
            res1 = $"demo2-2 result: {await task2}\n";
            LogTextBlock.Text += res1;
        }

       
        private static Random r = new Random();
        private async void Start_Click(object sender, RoutedEventArgs e)
        {
                await AddProgressBar(new ProgressWork { ProgressBar = ProgBar1, Delay = r.Next(100, 300) });
                await AddProgressBar(new ProgressWork { ProgressBar = ProgBar2, Delay = r.Next(100, 300) });
        }

        private async void Start2_Click(object sender, RoutedEventArgs e)
        {
            AddProgressBar(new ProgressWork { ProgressBar = ProgBar1, Delay = r.Next(100, 500) });
            AddProgressBar(new ProgressWork { ProgressBar = ProgBar2, Delay = r.Next(100, 500) });
        }
        private async Task AddProgressBar(ProgressWork progressWork)
        {
            for (int i = 0; i <= 10; i++)
            {
                progressWork.ProgressBar.Value = i * 10;
                await Task.Delay(progressWork.Delay);
            }
        }
        private class ProgressWork
        {
            public ProgressBar ProgressBar { get; set; } = null!;
            public int Delay { get; set; }
        }

    }
}
