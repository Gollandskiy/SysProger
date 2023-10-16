using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Занятие_в_аудитории_1_Системное_программирование_
{
    /// <summary>
    /// Логика взаимодействия для DLLWindow.xaml
    /// </summary>
    public partial class DLLWindow : Window
    {
        [DllImport("User32.dll")]
        public static extern int MessageBoxA(IntPtr hVnd, string lpText,
            string lpCaption, uint uType);
        public DLLWindow()
        {
            InitializeComponent();
        }

        private void AlertButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxA(IntPtr.Zero, "Уведомление", "Заголовок", 0x40);
        }
        [DllImport("Kernel32.dll", EntryPoint = "Beep")]
        public static extern bool Sound(uint frequency, uint duration);

        public delegate void ThreadMethod();
        [DllImport("Kernel32.dll", EntryPoint = "CreateThread")]
        public static extern IntPtr NewThread(IntPtr lpThreadAttributes,
                                              uint dwStackSize,
                                              ThreadMethod lpStartAddress,
                                              IntPtr lpParameter,
                                              uint dwCreationFlags,
                                              IntPtr lpThreadId);
        public void ErrorMessage()
        {
            MessageBoxA(IntPtr.Zero, "Уведомление об ошибке", 
                "Место возникновения", 0x14);
            methodHandle.Free();
        }
        GCHandle methodHandle;
        private void ThreadButton_Click(object sender, RoutedEventArgs e)
        {
            var method = new ThreadMethod(ErrorMessage);
            methodHandle = GCHandle.Alloc(method);
            NewThread(IntPtr.Zero, 0, method, IntPtr.Zero, 0, IntPtr.Zero);
        }
        private void SoundButton_Click(object sender, RoutedEventArgs e)
        {
            Sound(440, 300);
        }

        private void SoundButton2_Click(object sender, RoutedEventArgs e)
        {
            Sound(540, 300);
        }

        private void SoundButton3_Click(object sender, RoutedEventArgs e)
        {
            Sound(640, 300);
        }

        private void SoundButton4_Click(object sender, RoutedEventArgs e)
        {
            Sound(740, 300);
        }
    }
}