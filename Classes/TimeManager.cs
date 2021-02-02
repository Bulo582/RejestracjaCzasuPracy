using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;
using System.Windows;

namespace RejestracjaCzasuPracy.Classes
{
    public class TimeManager : MainWindow
    {


        private bool b_Stop = false;
        private bool b_Start = false;
        DateTime today;

        int _sec;
        public int sec { get { return _sec; } set { this._sec = value;} }
        int _min;
        public int min { get { return _min; } set { this._min = value;} }
        int _hour;
        public int hour { get { return _hour; } set { this._hour = value;} }

        public bool B_Stop { get => b_Stop; set { b_Stop = value; b_Start = !value; } }
        public bool B_Start { get => b_Start; set { b_Start = value; b_Stop = !value; } }

        // take time from currentTime
        public TimeManager()
        {
            today = DateTime.Today;
        }

        public void StartRunner()
        {
            new Thread(Start).Start();
        }

        public void StopRunner()
        {
            new Thread(Stop).Start();
        }

        private void Start()
        {
            if (currentTime == null)
            {
                while (true)
                {
                    Thread.Sleep(50);
                    if (currentTime != null)
                        break;
                }
            }
            string date = $"\t{today.Date.ToShortDateString()}\n";
            string timeInfo = $"Start {TakeHour()}:{TakeMin()}:{TakeSec()}\t {DateTime.Now.ToLongTimeString()}";
            DateTime lastDate = FileManager.CheckingLastDate(path);
            B_Start = true;
            if (lastDate.ToShortDateString() == DateTime.Today.ToShortDateString())
            {
                FileManager.MakeFileTxt(true, path, timeInfo);
            }
            else
            {
                FileManager.MakeFileTxt(true, path, date, out _);
                FileManager.MakeFileTxt(true, path, timeInfo);
            }
            
        }

        private void Stop()
        {
            if (currentTime == null)
            {
                MessageBox.Show("Najpierw uruchom stoper.");
            }
            else
            {
                B_Stop = true;
                string timeInfo = $"Stop {TakeHour()}:{TakeMin()}:{TakeSec()}\t {DateTime.Now.ToLongTimeString()}";
                FileManager.MakeFileTxt(true, path, timeInfo);
            }
        }

        public TimeSpan End(TimeSpan ts)
        {
            if (currentTime == null)
            {
                MessageBox.Show("Najpierw uruchom stoper.");
                return new TimeSpan();
            }
            else
            {
                TimeSpan sum;
                TimeSpan lastTime = FileManager.TakeLastEndTime(path);
                if (FileManager.lastTimeExist)
                {
                    sum = lastTime + ts;
                }
                else
                {
                    sum = ts;
                }

                string _timeInfo = $"Stop {TakeHour()}:{TakeMin()}:{TakeSec()}\t {DateTime.Now.ToLongTimeString()}";
                string timeInfo = $"End !-{sum.Hours}:{sum.Minutes}:{sum.Seconds}-!\t {DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}";
                if(B_Start)
                    FileManager.MakeFileTxt(true, path, _timeInfo);
                FileManager.MakeFileTxt(true, path, timeInfo);

                return sum;
            }
        }
        public int TakeSec()
        {
            string[] tab = currentTime.Split(':');
            return int.Parse(tab[2]);
        }
        public int TakeMin()
        {
            string[] tab = currentTime.Split(':');
            return int.Parse(tab[1]);
        }
        public int TakeHour()
        {
            string[] tab = currentTime.Split(':');
            return int.Parse(tab[1]);
        }

    }
}
