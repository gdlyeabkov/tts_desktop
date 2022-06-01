using System;
using System.Collections.Generic;
using System.IO;
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

namespace TTS.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для RenameDiactDialog.xaml
    /// </summary>
    public partial class RenameDiactDialog : Window
    {

        public string dictPath = "";

        public RenameDiactDialog(string dictPath)
        {
            InitializeComponent();

            Init(dictPath);

        }

        public void Init (string dictPath)
        {
            this.dictPath = dictPath;
            string dictName = System.IO.Path.GetFileNameWithoutExtension(dictPath);
            dictNameBox.Text = dictName;
        }


        private void OkHandler(object sender, RoutedEventArgs e)
        {
            Ok();
        }

        public void Ok()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string cachePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader";
            string dictsFolder = cachePath + @"\dicts\";
            string dictName = dictNameBox.Text;
            string updatedDictPath = dictsFolder + dictName + @".dic";
            File.Move(dictPath, updatedDictPath);
            Cancel();
        }

        private void CancelHandler(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel()
        {
            this.Close();
        }

    }
}
