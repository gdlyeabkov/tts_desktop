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
    /// Логика взаимодействия для DictProfileDialog.xaml
    /// </summary>
    public partial class DictProfileDialog : Window
    {

        public MainWindow mainWindow;
        public int selectedProfileIndex = -1;

        public DictProfileDialog(MainWindow mainWindow)
        {
            InitializeComponent();

            Init(mainWindow);

        }

        public void Init (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            GetProfiles();
        }

        public void CancelHandler(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel()
        {
            this.Close();
        }

        public void OpenAddProfileHandler (object sender, RoutedEventArgs e)
        {
            OpenAddProfile();
        }

        public void OpenAddProfile()
        {
            Dialogs.OpenAddProfileDialog dialog = new Dialogs.OpenAddProfileDialog(this);
            dialog.Show();
        }

        public void AddProfileHandler(object sender, RoutedEventArgs e)
        {
            AddProfile();
        }

        public void AddProfile ()
        {
            string profileName = "";
            StackPanel profile = new StackPanel();
            profile.DataContext = profileName;
            TextBlock profileNameLabel = new TextBlock();
            profileNameLabel.Text = profileName;
            profile.Children.Add(profileNameLabel);
            profiles.Children.Add(profile);
        }

        public void GetProfilesHandler(object sender, RoutedEventArgs e)
        {
            GetProfiles();
        }

        public void GetProfiles()
        {
            profiles.Children.Clear();
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<DictProfile> currentDictProfiles = loadedContent.dictProfiles;
            DictProfile dictProfile = new DictProfile();
            foreach (DictProfile currentDictProfile in currentDictProfiles)
            {
                string currentDictProfileName = currentDictProfile.name;
                StackPanel profile = new StackPanel();
                TextBlock profileNameLabel = new TextBlock();
                profileNameLabel.Text = currentDictProfileName;
                profileNameLabel.Margin = new Thickness(15, 5, 15, 5);
                profile.Children.Add(profileNameLabel);
                profiles.Children.Add(profile);
                profile.DataContext = currentDictProfileName;
                profile.MouseLeftButtonUp += SelectProfileHandler;
            }
            UIElementCollection profilesChildren = profiles.Children;
            int profilesChildrenCount = profilesChildren.Count;
            bool isHaveProfiles = profilesChildrenCount >= 1;
            if (isHaveProfiles)
            {
                UIElement rawSelectedProfile = profilesChildren[0];
                StackPanel selectedProfile = ((StackPanel)(rawSelectedProfile));
                SelectProfile(selectedProfile);
            }
        }

        public void RemoveProfileHandler(object sender, RoutedEventArgs e)
        {
            RemoveProfile();
        }

        public void RemoveProfile()
        {
            bool isSelected = selectedProfileIndex >= 0;
            if (isSelected)
            {

                // profiles.Children.RemoveAt(selectedProfileIndex);
                Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
                string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
                string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
                JavaScriptSerializer js = new JavaScriptSerializer();
                string saveDataFileContent = File.ReadAllText(saveDataFilePath);
                SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
                List<Dictionary<String, Object>> currentBookmarks = loadedContent.bookmarks;
                Settings currentSettings = loadedContent.settings;
                List<DictProfile> updatedDictProfiles = loadedContent.dictProfiles;
                updatedDictProfiles.RemoveAt(selectedProfileIndex);
                string savedContent = js.Serialize(new SavedContent
                {
                    bookmarks = currentBookmarks,
                    settings = currentSettings,
                    dictProfiles = updatedDictProfiles
                });
                File.WriteAllText(saveDataFilePath, savedContent);

                selectedProfileIndex = -1;
                GetProfiles();
            }
        }

        public void ShiftProfileHandler(object sender, RoutedEventArgs e)
        {
            ShiftProfile();
        }

        public void ShiftProfile()
        {
            bool isSelected = selectedProfileIndex >= 0;
            if (isSelected)
            {
                UIElementCollection profilesChildren = profiles.Children;
                int profilesChildrenCount = profilesChildren.Count;
                int lastProfileIndex = profilesChildrenCount - 1;
                bool isCanShift = selectedProfileIndex < lastProfileIndex;
                if (isCanShift)
                {
                    selectedProfileIndex++;
                    UIElement rawSelectedProfile = profilesChildren[selectedProfileIndex];
                    StackPanel selectedProfile = ((StackPanel)(rawSelectedProfile));
                    SelectProfile(selectedProfile);
                }
            }
        }

        public void ReverseShiftProfileHandler(object sender, RoutedEventArgs e)
        {
            ReverseShiftProfile();
        }

        public void ReverseShiftProfile()
        {
            bool isSelected = selectedProfileIndex >= 0;
            if (isSelected)
            {
                UIElementCollection profilesChildren = profiles.Children;
                int profilesChildrenCount = profilesChildren.Count;
                bool isCanShift = selectedProfileIndex > 0;
                if (isCanShift)
                {
                    selectedProfileIndex--;
                    UIElement rawSelectedProfile = profilesChildren[selectedProfileIndex];
                    StackPanel selectedProfile = ((StackPanel)(rawSelectedProfile));
                    SelectProfile(selectedProfile);
                }
            }
        }

        public void SelectProfileHandler(object sender, RoutedEventArgs e)
        {
            StackPanel profile = ((StackPanel)(sender));
            SelectProfile(profile);
        }

        public void SelectProfile(StackPanel profile)
        {
            foreach (StackPanel localProfile in profiles.Children)
            {
                localProfile.Background = System.Windows.Media.Brushes.Transparent;
            }
            profile.Background = System.Windows.Media.Brushes.SkyBlue;
            UIElementCollection profilesChildren = profiles.Children;
            selectedProfileIndex = profilesChildren.IndexOf(profile);
        }

        public void RenameProfileHandler(object sender, RoutedEventArgs e)
        {
            RenameProfile();
        }

        public void RenameProfile()
        {
            UIElementCollection profilesChildren = profiles.Children;
            UIElement rawProfile = profilesChildren[selectedProfileIndex];
            StackPanel profile = ((StackPanel)(rawProfile));
            object profileData = profile.DataContext;
            string profileaName = ((string)(profileData));
            Dialogs.RenameProfileDialog dialog = new Dialogs.RenameProfileDialog(this, profileaName);
            dialog.Show();
        }


    }
}
