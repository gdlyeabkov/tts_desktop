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
    /// Логика взаимодействия для CreateBookmarkDialog.xaml
    /// </summary>
    public partial class CreateBookmarkDialog : Window
    {

        public MainWindow mainWindow;

        public CreateBookmarkDialog (MainWindow mainWindow)
        {
            InitializeComponent();

            Init(mainWindow);

        }

        public void Init (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private void OkHandler(object sender, RoutedEventArgs e)
        {
            Ok();
        }

        public void Ok ()
        {
            string nameBoxContent = nameBox.Text;
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> updatedBookmarks = loadedContent.bookmarks;
            int openedDocControlSelectedIndex = mainWindow.openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = mainWindow.openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            int index = inputBox.SelectionStart;
            Dictionary<String, Object> bookmark = new Dictionary<String, Object>();
            bookmark.Add("name", nameBoxContent);
            bookmark.Add("index", index);
            updatedBookmarks.Add(bookmark);
            
            string savedContent = js.Serialize(new SavedContent
            {
                bookmarks = updatedBookmarks
            });
            File.WriteAllText(saveDataFilePath, savedContent);
            Cancel();
        }

        private void CancelHandler(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel ()
        {
            this.Close();
        }

    }
}
