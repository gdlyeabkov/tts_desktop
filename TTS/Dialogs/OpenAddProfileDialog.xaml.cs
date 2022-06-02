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
    /// Логика взаимодействия для OpenAddProfileDialog.xaml
    /// </summary>
    public partial class OpenAddProfileDialog : Window
    {

        public Dialogs.DictProfileDialog dialog;

        public OpenAddProfileDialog(Dialogs.DictProfileDialog dialog)
        {
            InitializeComponent();

            Init(dialog);

        }

        public void Init (Dialogs.DictProfileDialog dialog)
        {
            this.dialog = dialog;
        }

        private void OkHandler(object sender, RoutedEventArgs e)
        {
            Ok();
        }

        public void Ok()
        {
            string profileName = dictNameBox.Text;
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> currentBookmarks = loadedContent.bookmarks;
            Settings currentSettings = loadedContent.settings;
            List<DictProfile> updatedDictProfiles = loadedContent.dictProfiles;
            DictProfile dictProfile = new DictProfile();
            dictProfile.name = profileName;
            List<DictProfileItem>  profileItems = new List<DictProfileItem>();
            foreach (StackPanel dict in dialog.mainWindow.dicts.Children)
            {
                object dictData = dict.DataContext;
                string dictPath = ((string)(dictData));
                UIElementCollection dictChildren = dict.Children;
                UIElement rawCheckBox = dictChildren[0];
                CheckBox checkBox = ((CheckBox)(rawCheckBox));
                object rawIsChecked = checkBox.IsChecked;
                bool isChecked = ((bool)(rawIsChecked));
                bool isDefault = dictPath == dialog.mainWindow.defaultDictPath;
                DictProfileItem profileItem = new DictProfileItem();
                profileItem.path = dictPath;
                profileItem.isChecked = isChecked;
                profileItem.isDefault = isDefault;
                profileItems.Add(profileItem);
            }
            dictProfile.items = profileItems;
            updatedDictProfiles.Add(dictProfile);
            string savedContent = js.Serialize(new SavedContent
            {
                bookmarks = currentBookmarks,
                settings = currentSettings,
                dictProfiles = updatedDictProfiles
            });
            File.WriteAllText(saveDataFilePath, savedContent);
            dialog.GetProfiles();
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
