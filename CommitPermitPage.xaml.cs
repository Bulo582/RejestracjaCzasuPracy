using RejestracjaCzasuPracy.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RejestracjaCzasuPracy
{
    /// <summary>
    /// Logika interakcji dla klasy CommitPermitPage.xaml
    /// </summary>
    public partial class CommitPermitPage : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        string path;
        public CommitPermitPage(string path)
        {
            this.path = path;
            InitializeComponent();
    }

        private void subbmit_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(commit.Text))
            {
                MainWindow.isExitingApp = true;
                string info = $"! Commit: {commit.Text}\n\n";
                FileManager.MakeFileTxt(true, path, info);
                Process.Start("notepad.exe", path);
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                subbmit.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }
    }
}
