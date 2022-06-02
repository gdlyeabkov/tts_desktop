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
    /// Логика взаимодействия для EditHotKeyDialog.xaml
    /// </summary>
    public partial class EditHotKeyDialog : Window
    {

        public string cmd = "";

        public EditHotKeyDialog(string cmd)
        {
            InitializeComponent();

            Init(cmd);

        }

        public void Init(string cmd)
        {
            this.cmd = cmd;

            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<HotKey> hotKeys = loadedContent.hotKeys;
            
            bool isCreateDoc = cmd == "createDoc";
            bool isOpenDoc = cmd == "openDoc";
            if (isCreateDoc)
            {
                HotKey hotKey = hotKeys[0];
                string shortCut = hotKey.shortcut;
                box.Text = shortCut;
            }
            else if (isOpenDoc)
            {
                HotKey hotKey = hotKeys[1];
                string shortCut = hotKey.shortcut;
                box.Text = shortCut;
            }
        }

        private void HotKeyHandler(object sender, KeyEventArgs e)
        {
            TextBox input = ((TextBox)(sender));
            Key currentKey = e.Key;
            HotKey(input, currentKey);
        }

        public void HotKey(TextBox input, Key key)
        {
            Key leftShiftKey = Key.LeftShift;
            Key rightShiftKey = Key.RightShift;
            Key leftCtrlKey = Key.LeftCtrl;
            Key rightCtrlKey = Key.RightCtrl;
            bool isNotLeftShiftKey = key != leftShiftKey;
            bool isNotRightShiftKey = key != rightShiftKey;
            bool isNotShiftKey = isNotLeftShiftKey && isNotRightShiftKey;
            bool isNotLeftCtrlKey = key != leftCtrlKey;
            bool isNotRightCtrlKey = key != rightCtrlKey;
            bool isNotCtrlKey = isNotLeftCtrlKey && isNotRightCtrlKey;
            bool isNotKeyModifier = isNotShiftKey && isNotCtrlKey;
            if (isNotKeyModifier)
            {
                bool isCtrlEnabled = (Keyboard.Modifiers & ModifierKeys.Control) > 0;
                bool isShiftEnabled = (Keyboard.Modifiers & ModifierKeys.Shift) > 0;
                string rawHotKey = key.ToString();
                if (isShiftEnabled)
                {
                    rawHotKey = "Shift + " + rawHotKey;
                }

                if (isCtrlEnabled)
                {
                    rawHotKey = "Ctrl + " + rawHotKey;
                }
                box.Text = rawHotKey;
            }
        }

        public void CancelHandler(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel()
        {
            this.Close();
        }

        public void OkHandler(object sender, RoutedEventArgs e)
        {
            Ok();
        }

        public void Ok()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> currentBookmarks = loadedContent.bookmarks;
            Settings currentSettings = loadedContent.settings;
            List<DictProfile> currentDictProfiles = loadedContent.dictProfiles;
            List<HotKey> updatedHotKeys = loadedContent.hotKeys;
            string boxContent = box.Text;
            bool isCreateDoc = cmd == "createDoc";
            bool isOpenDoc = cmd == "openDoc";
            int hoyKeyIndex = updatedHotKeys.FindIndex((HotKey hotKey) =>
            {
                string localCmd = hotKey.cmd;
                bool isCurrentCmd = localCmd == cmd;
                return isCurrentCmd;
            });
            bool isFound = hoyKeyIndex >= 0;
            if (isFound)
            {
                HotKey hotKey = updatedHotKeys[hoyKeyIndex];
                hotKey.shortcut = boxContent;
            }
            string savedContent = js.Serialize(new SavedContent
            {
                bookmarks = currentBookmarks,
                settings = currentSettings,
                dictProfiles = currentDictProfiles,
                hotKeys = updatedHotKeys
            });
            File.WriteAllText(saveDataFilePath, savedContent);
            Cancel();
        }

        public void ClearHandler (object sender, RoutedEventArgs e)
        {
            Clear();
        }

        public void Clear ()
        {
            box.Text = "";
        }

        public void SetDefaultHandler(object sender, RoutedEventArgs e)
        {
            SetDefault();
        }

        public void SetDefault()
        {
            string shortCut = "";
            bool isCreateDoc = cmd == "createDoc";
            bool isOpenDoc = cmd == "openDoc";
            if (isCreateDoc)
            {
                shortCut = "Ctrl+N";
            }
            else if (isOpenDoc)
            {
                shortCut = "Ctrl+O";
            }
            box.Text = shortCut;
        }



    }
}
