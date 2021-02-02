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
using System.IO;
using RejestracjaCzasuPracy.Classes;

namespace RejestracjaCzasuPracy
{
    /// <summary>
    /// Logika interakcji dla klasy NewFilePage.xaml
    /// </summary>
    public partial class NewFilePage : Window
    {
        public NewFilePage()
        {
            InitializeComponent();
        }

        private void btn_create_Click(object sender, RoutedEventArgs e)
        {
            Create();
        }


        public void Create()
        {
            if (!String.IsNullOrEmpty(tbox_name.Text) && !String.IsNullOrEmpty(tbox_path.Text))
            {
                string path = System.IO.Path.Combine(App.pathOfFilesFolder, $"{tbox_name.Text}_{tbox_path.Text}.rcp");
                string info = $"path: {tbox_path.Text}\nname: {tbox_name.Text}\n\n--";

                if (File.Exists(path))
                    MessageBox.Show("Taki plik już istnieje");
                else
                {
                    FileManager.MakeFileTxt(false, path, info, out bool done);
                    if (done)
                    {
                        MessageBox.Show("Plik został stworzony");
                        MainWindow mw = (MainWindow)this.DataContext;
                        mw.lbl_info.Content = info;
                        mw.btn_Start.IsEnabled = true;
                        MainWindow.timeManager = new TimeManager();
                        MainWindow.path = path;
                        Close();
                    }
                    else
                        MessageBox.Show("Nie udało się stworzyć pliku");
                }
            }
            else
                MessageBox.Show("Wypełnij pola Path oraz Name");
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Create();
        }
    }
}
