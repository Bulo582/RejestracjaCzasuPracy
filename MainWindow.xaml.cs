using RejestracjaCzasuPracy.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RejestracjaCzasuPracy
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        bool fileLoaded { get { if (path == String.Empty) return false; else return true; } }
        public static string path = String.Empty;
        //
        static DispatcherTimer dispatcherTimer = new DispatcherTimer();
        static Stopwatch stopWatch = new Stopwatch();
        //
        public static TimeManager timeManager;
        //
        protected static string currentTime;
        public string CurrentTime { get { return currentTime; } set { currentTime = value; OnPropertyChanged("CurrentTime"); }}

      

        protected static bool firstStart = false;
        public static bool isExitingApp = false;
        private string mainInfo = "Wczytaj plik lub stwórz nowy"; 
        public string MainInfo { get => mainInfo;  set { mainInfo = value; OnPropertyChanged("MainInfo");}}

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            dispatcherTimer.Tick += new EventHandler(dt_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            btn_Start.IsEnabled = false;
            btn_Break.IsEnabled = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!isExitingApp)
            {
                Finish(true);
            }
            base.OnClosed(e);
        }

        void dt_Tick(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                TimeSpan ts = stopWatch.Elapsed;
                CurrentTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Hours, ts.Minutes, ts.Seconds);

            }
        }

        #region btn
        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            
            if (!stopWatch.IsRunning)
            {
                if (fileLoaded)
                {
                    firstStart = true;
                    btn_Break.IsEnabled = true;
                    btn_Start.IsEnabled = false;
                    stopWatch.Start();
                    dispatcherTimer.Start();
                    timeManager.StartRunner();
                }
                else
                {
                    MessageBox.Show("Wczytaj plik txt z czasem pracy.");
                }
            }
            else
                MessageBox.Show("Stoper pracuję!");

        }

        private void btn_Break_Click(object sender, RoutedEventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                btn_Break.IsEnabled = false;
                btn_Start.IsEnabled = true;
                stopWatch.Stop();
                timeManager.StopRunner();
            }
            else
                MessageBox.Show("Stoper jest zatrzymany");
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            Finish(false);
        }

        public static void Finish(bool isExitButton)
        {
            if (isExitButton)
            {
                isExitingApp = true;
                if (path != string.Empty && firstStart)
                {
                    if (stopWatch.IsRunning)
                    {
                        stopWatch.Stop();
                        dispatcherTimer.Stop();
                    }
                    MessageBox.Show($"Łączny czas = {timeManager.End(stopWatch.Elapsed).ToString(@"hh\:mm\:ss")}");
                    CommitPermitPage p = new CommitPermitPage(path);
                    p.Show();
                }
                else
                {
                    Process.Start("notepad.exe", path);
                    Application.Current.Shutdown();
                }
            }
            else
            {
                if (path != string.Empty && firstStart)
                {
                    if (stopWatch.IsRunning)
                    {
                        stopWatch.Stop();
                        dispatcherTimer.Stop();
                    }
                    MessageBox.Show($"Łączny czas = {timeManager.End(stopWatch.Elapsed).ToString(@"hh\:mm\:ss")}");
                    CommitPermitPage p = new CommitPermitPage(path);
                    p.Show();
                }
                else
                {
                    isExitingApp = true;
                    Process.Start("notepad.exe", path);
                    Application.Current.Shutdown();
                }
            }
        }

        #endregion

        #region mi
        private void mi_new_Click(object sender, RoutedEventArgs e)
        {
            NewFilePage p = new NewFilePage();
            p.Show();
            p.DataContext = this;
        }

        private void mi_load_Click(object sender, RoutedEventArgs e)
        {
            
            string info = FileManager.OpenFile(out bool done, out string _path);
            path = _path;
            if (done)
            {
                lbl_info.Content = info;
                timeManager = new TimeManager();
                btn_Start.IsEnabled = true;
            }
        }
        #endregion

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (timeManager != null)
                {
                    if (timeManager.B_Start)
                    {
                        btn_Break.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }
                    else if (timeManager.B_Stop)
                    {
                        btn_Start.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }
                    else
                    {
                        btn_Start.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }
                }
            }
            if(e.Key == Key.Escape)
            {
                btn_Stop.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
            if(e.Key == Key.F1)
            {
                mi_new.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
            }
            if(e.Key == Key.F2)
            {
                mi_load.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
            }
        }
    }
}
