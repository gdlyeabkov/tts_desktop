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
    /// Логика взаимодействия для RenameProfileDialog.xaml
    /// </summary>
    public partial class RenameProfileDialog : Window
    {

        public Dialogs.DictProfileDialog dialog;
        public string profileName;

        public RenameProfileDialog(Dialogs.DictProfileDialog dialog, string profileName)
        {
            InitializeComponent();

            Init(dialog, profileName);

        }

        public void Init(Dialogs.DictProfileDialog dialog, string profileName)
        {
            this.dialog = dialog;
            this.profileName = profileName;
            dictNameBox.Text = profileName;
        }

        private void OkHandler(object sender, RoutedEventArgs e)
        {
            Ok();
        }

        public void Ok()
        {
            string updatedProfileName = dictNameBox.Text;
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> currentBookmarks = loadedContent.bookmarks;
            Settings currentSettings = loadedContent.settings;
            List<DictProfile> updatedDictProfiles = loadedContent.dictProfiles;
            List<HotKey> currentHotKeys = loadedContent.hotKeys;
            int profileIndex = updatedDictProfiles.FindIndex((DictProfile profile) =>
            {
                string localProfileName = profile.name;
                bool isLocalFound = localProfileName == profileName;
                return isLocalFound;
            });
            bool isFound = profileIndex >= 0;
            if (isFound)
            {
                DictProfile updatedDictProfile = updatedDictProfiles[profileIndex];
                updatedDictProfile.name = updatedProfileName;
                string savedContent = js.Serialize(new SavedContent
                {
                    bookmarks = currentBookmarks,
                    settings = currentSettings,
                    dictProfiles = updatedDictProfiles,
                    hotKeys = currentHotKeys
                });
                File.WriteAllText(saveDataFilePath, savedContent);
                dialog.GetProfiles();
                Cancel();
            }
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
