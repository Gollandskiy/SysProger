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
    /// Логика взаимодействия для CancelWindow.xaml
    /// </summary>
    public partial class CancelWindow : Window
    {
        private CancellationTokenSource _cancelTokSour;
        private int activeTaskCount;
        private readonly object countLocker = new();
        public CancelWindow()
        {
            InitializeComponent();
            _cancelTokSour = null;
        }

        private void StopBtn1_Click(object sender, RoutedEventArgs e)
        {
            _cancelTokSour?.Cancel();
        }

        private async void StartBtn1_Click(object sender, RoutedEventArgs e)
        {
            //await RunProgressVai(ProgressBar10);
            //await RunProgressVai(ProgressBar11, 4);
            //await RunProgressVai(ProgressBar12, 2);
            //await Task.Run(() =>RunProgress(ProgressBar10));
            //await Task.Run(() => RunProgress(ProgressBar11, 4));
            //await Task.Run(() => RunProgress(ProgressBar12, 2));
             _cancelTokSour = new CancellationTokenSource();
            // RunProgressCancel(ProgressBar10, _cancelTokSour.Token);
            // RunProgressCancel(ProgressBar11, _cancelTokSour.Token, 4);
            // RunProgressCancel(ProgressBar12, _cancelTokSour.Token, 2);
            // await Task.WhenAll(RunProgressVai(ProgressBar10),
            //     RunProgressVai(ProgressBar11, 4),
            //     RunProgressVai(ProgressBar12, 2));
            // MessageBox.Show("Done!");
            RunProgressCancel(ProgressBar10,_cancelTokSour.Token);
            RunProgressCancel(ProgressBar11, _cancelTokSour.Token,4);
            RunProgressCancel(ProgressBar12, _cancelTokSour.Token,2);

        }
        private async void RunProgress(ProgressBar progressBar,CancellationToken token, int time = 3)
        {
            for (int i = 0; i < 10; i++)
            {
                Dispatcher.Invoke(() => progressBar.Value += 10);
                await Task.Delay(1000 * time / 10);
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }
        private async Task RunProgressVai(ProgressBar progressBar,CancellationToken token, int time = 3)
        {
            progressBar.Value = 0;
            for (int i = 0; i < 10; i++)
            {
                progressBar.Value += 10;
                await Task.Delay(1000 * time / 10);
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }
        private async void RunProgressCancel(ProgressBar progressBar, CancellationToken cToken, int time = 3)
        {
            progressBar.Value = 0;
            lock(countLocker)
            activeTaskCount++;
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    progressBar.Value += 10;
                    await Task.Delay(1000 * time / 10);
                    cToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                if (progressBar.Value < 100)
                {
                    progressBar.Foreground = Brushes.Tomato;
                    while (progressBar.Value > 0)
                    {
                        progressBar.Value--;
                        await Task.Delay(5);
                    }
                }
                return;
            }
            finally
            {
                progressBar.Foreground = Brushes.ForestGreen;
                bool isLast;
                lock (countLocker)
                {
                    activeTaskCount--;
                    isLast = activeTaskCount == 0;
                }
                if (isLast)
                {
                    MessageBox.Show("Done!");
                }
            }
        }

        private async void StartBtn2_Click(object sender, RoutedEventArgs e)
        {
           _cancelTokSour = new CancellationTokenSource();
           await RunProgressVai(ProgressBar13, _cancelTokSour.Token);
           await RunProgressVai(ProgressBar14, _cancelTokSour.Token,2);
           await RunProgressVai(ProgressBar15, _cancelTokSour.Token,4);

        }

        private void StopBtn2_Click(object sender, RoutedEventArgs e)
        {
            _cancelTokSour?.Cancel();
        }

        private async void StartBtn3_Click(object sender, RoutedEventArgs e)
        {
            _cancelTokSour = new CancellationTokenSource();
            await Task.Run(() => RunProgress(ProgressBar16, _cancelTokSour.Token));
            await Task.Run(() => RunProgress(ProgressBar17, _cancelTokSour.Token, 4));
            await Task.Run(() => RunProgress(ProgressBar18 , _cancelTokSour.Token, 2));
        }

        private void StopBtn3_Click(object sender, RoutedEventArgs e)
        {
            _cancelTokSour.Cancel();
        }
    }
}
