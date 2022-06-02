using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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
    /// Логика взаимодействия для CreateDictDialog.xaml
    /// </summary>
    public partial class CreateDictDialog : Window
    {
        public CreateDictDialog()
        {
            InitializeComponent();
        }

        private void OkHandler(object sender, RoutedEventArgs e)
        {
            Ok();
        }

        public void Ok()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            string cachePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader";
            string dictsFolder = cachePath + @"\dicts\";
            string dictName = dictNameBox.Text;
            string dictPath = dictsFolder + dictName + @".dic";
            using (System.IO.Stream s = File.Open(dictPath, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write("");
                }
            };

            JavaScriptSerializer js = new JavaScriptSerializer();
            Dictionary<String, Object>  dictContent = new Dictionary<String, Object>();
            string savedContent = js.Serialize(dictContent);
            File.WriteAllText(dictPath, savedContent);

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
