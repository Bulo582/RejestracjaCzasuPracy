using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

namespace RejestracjaCzasuPracy.Classes
{
    public class FileManager 
    {
        static List<Exception> Exceptions = new List<Exception>();
        public static bool lastTimeExist = false;
        public static void MakeFileTxt(bool append, string path, string info, out bool done)
        {
            try
            {
                using (StreamWriter fw = new StreamWriter(path, append))
                {
                    fw.WriteLine(info);
                    done = true;
                }
            }
            catch(Exception ex)
            {
                done = false;
                Exceptions.Add(ex);
            }
        }

        public static void MakeFileTxt(bool append, string path, string info)
        {
            try
            {
                using (StreamWriter fw = new StreamWriter(path, append))
                {
                    fw.WriteLine(info);
                }
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
            }
        }

        public static string OpenFile(out bool done, out string path)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string info = String.Empty;
            openFileDialog.InitialDirectory = App.pathOfFilesFolder;
            if (openFileDialog.ShowDialog() == true)
            {
                path = Path.Combine(openFileDialog.InitialDirectory, openFileDialog.FileName);
                try
                {
                    string currentLine = string.Empty;
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                    {
                        while ((currentLine = sr.ReadLine()) != "--")
                        {
                            info += currentLine + "\n";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exceptions.Add(ex);
                }
            }
            else
                path = String.Empty;
            if (info == String.Empty)
                done = false;
            else
                done = true;
            return info;
        }

        public string OpenFile(string path)
        {
            string info = String.Empty;
            try
            {
                string currentLine = string.Empty;
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((currentLine = sr.ReadLine()) != "--")
                    {
                        info += currentLine + "\n";
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
            }
            return info;
        }

        public static DateTime CheckingLastDate(string filePath)
        {
            string text = string.Empty;
            using (StreamReader sr = new StreamReader(filePath))
                text = sr.ReadToEndAsync().Result;
            
            string[] tab = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = tab.Length-1; i > 0 ; i--)
            {
                if (DateTime.TryParse(tab[i], out DateTime dateTime))
                {
                    return dateTime;
                }
            }
            return new DateTime();
        }

        public static TimeSpan TakeLastEndTime(string filePath)
        {
            string text = string.Empty;
            using (StreamReader sr = new StreamReader(filePath))
                text = sr.ReadToEndAsync().Result;

            string[] tab = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = tab.Length - 1; i > 0; i--)
            {
                if (tab[i].Contains("End"))
                {
                    lastTimeExist = true;
                    int a = tab[i].IndexOf("!-")+2;
                    int b = tab[i].IndexOf("-!");
                    int c = b-a;
                    string value = tab[i].Substring(a,c);
                    string[] tab2 = value.Split(':');
                    return new TimeSpan(int.Parse(tab2[0]), int.Parse(tab2[1]), int.Parse(tab2[2]));
                }
            }
            return new TimeSpan(0,0,0);
        }
    }


}
