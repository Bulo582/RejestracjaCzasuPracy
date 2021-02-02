using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace RejestracjaCzasuPracy
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string pathOfFilesFolder = Path.Combine(Environment.CurrentDirectory, "Files");

        protected override void OnStartup(StartupEventArgs e)
        {
            CreateFolderFiles(pathOfFilesFolder);
            base.OnStartup(e);
        }

        public void CreateFolderFiles(string path)
        {
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
