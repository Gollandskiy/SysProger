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
            SynchroWindow SOkno = new SynchroWindow();
            SOkno.ShowDialog();
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
    }
}
