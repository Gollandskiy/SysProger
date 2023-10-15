using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для ProcessWindow.xaml
    /// </summary>
    public partial class ProcessWindow : Window
    {
        private static Mutex mutex;
        private const String mutexName = "SPNP_MUTEX";
        public ProcessWindow()
        {
            //CheckPreviousLunch();
            InitializeComponent();
        }
        private void CheckPreviousLunch()
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
                    throw new ApplicationException(message);
                }
            }
            else mutex = new Mutex(true, mutexName);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Window_Closed(object sender, EventArgs e)
        {
            mutex.ReleaseMutex();
            //mutex.Dispose();
        }
        private void ShowProcesses_Click(object sender, RoutedEventArgs e)
        {
            Process[] processes = Process.GetProcesses();
            //ProcTextBlock.Text = "";
            ProcTreeView.Items.Clear();
            String prevName = "";
            TreeViewItem item = null;
            foreach (Process process in processes.OrderBy(p => p.ProcessName))
            {
                if (prevName != process.ProcessName)
                {
                    prevName = process.ProcessName;
                    item = new TreeViewItem() { Header = prevName };
                    ProcTreeView.Items.Add(item);
                }
                var subItem = new TreeViewItem()
                {
                    Header = String.Format("{0} {1}", process.Id, process.ProcessName),
                    Tag = process
                };
                subItem.Header += String.Format(" Threads: {0}", process.Threads.Count);
                subItem.MouseDoubleClick += TreeViewItem_MouseDoubleClick;
                item.Items.Add(subItem);
                //ProcTextBlock.Text += String.Format("   {0}       {1}\n", process.Id, process.ProcessName);
            }
        }

        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem item)
            {
                String message = "";
                if (item.Tag is Process process)
                {
                    foreach (ProcessThread thread in process.Threads)
                    {
                        message = String.Format("Process ID: {0}\n", process.Id);
                        message += String.Format("CPU Time: {0} seconds\n", process.TotalProcessorTime.TotalSeconds);
                        message += String.Format("Memory Usage: {0} MB\n", (process.WorkingSet64 / 1024) / 1024);
                        message += String.Format("Thread Count: {0}\n", process.Threads.Count);
                    }
                }
                else
                {
                    message = "No process in tag";
                }
                MessageBox.Show(message);
            }
        }
       private Process notepadProcess;
        private void StartNotepad_Click(object sender, RoutedEventArgs e)
        {
            notepadProcess ??= Process.Start("notepad.exe");
        }

        private void StopNotepad_Click(object sender, RoutedEventArgs e)
        {
            notepadProcess.CloseMainWindow();
            notepadProcess.Kill(true);
            notepadProcess.WaitForExit();
            notepadProcess.Dispose();
            notepadProcess = null;
        }

        private void StartEdit_Click(object sender, RoutedEventArgs e)
        {
            string dir = AppContext.BaseDirectory;
            int binPosition = dir.LastIndexOf("bin");
            string projectRoot = dir[0..binPosition];
            //MessageBox.Show(projectRoot);
            //return;
            notepadProcess ??= Process.Start(
                "notepad.exe",$"{projectRoot}ProcessWindow.xaml.cs"
                );
        }
        Process browserProcess;
        private void StartBro_Click(object sender, RoutedEventArgs e)
        {
            string filename = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            if (System.IO.File.Exists(filename))
            {
                browserProcess ??= Process.Start(filename,
               "itstep.org");
            }
            else
            {
                MessageBox.Show("Такого файла не существует!");
            }  
        }

        private void StartCalc_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("calc.exe");
        }
        private Process taskProc;
        private void StartDisp_Click(object sender, RoutedEventArgs e)
        {
            taskProc = new Process();
            taskProc.StartInfo.FileName = "taskmgr.exe";
            taskProc.StartInfo.UseShellExecute = true;
            taskProc.StartInfo.Verb = "runas";
            taskProc.Start();
        }

        private void StartParam_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("control.exe", "desk.cpl");
        }
    }
}
