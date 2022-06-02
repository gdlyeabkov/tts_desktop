﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Speech.Synthesis;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using NAudio.Wave;
using System.IO;
using System.Windows.Threading;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Diagnostics;
using NAudio.CoreAudioApi;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows.Shell;

namespace TTS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public bool isAppInit = false;
        public SpeechSynthesizer speechSynthesizer = null;
        public List<string> openedDocHistory;
        public int bookmarkIndex = -1;
        public Dictionary<String, Object> speechTimerData;
        public int selectedAudioDevice = -1;
        public string lastCopiedText = "";
        public System.Windows.Forms.NotifyIcon nIcon = null;
        public int selectedDictIndex = -1;
        public string selectedDictProfileName = "";
        public string defaultDictPath = "";


        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
        private IntPtr _ClipboardViewerNext;

        public MainWindow()
        {

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");

            InitializeComponent();

        }


        public void SetDefaultDictHandler (object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            object menuItemBoxData = menuItem.DataContext;
            string dictName = ((string)(menuItemBoxData));
            SetDefaultDict(dictName);
        }

        public void SetDefaultDict (string dictName)
        {
            /*defaultDictPath = dictName;
            UIElementCollection dictsChildren = dicts.Children;
            foreach (StackPanel dictChild in dictsChildren)
            {
                object dictChildData = dictChild.DataContext;
                string path = ((string)(dictChildData));
                bool isFound = path == dictName;
                if (isFound)
                {
                    UIElementCollection dictChildChildren = dictChild.Children;
                    object rawCheckBox = dictChildChildren[0];
                    CheckBox checkBox = ((CheckBox)(rawCheckBox));
                    checkBox.FontWeight = FontWeights.ExtraBlack;
                    break;
                }
            }*/

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
                bool isLocalFound = localProfileName == selectedDictProfileName;
                return isLocalFound;
            });
            bool isFound = profileIndex >= 0;
            if (isFound)
            {
                DictProfile updatedDictProfile = updatedDictProfiles[profileIndex];
                List<DictProfileItem> profileItems = updatedDictProfile.items;
                int profileItemIndex = profileItems.FindIndex((DictProfileItem profileItem) =>
                {
                    string localProfilePath = profileItem.path;
                    bool isLocalFound = localProfilePath == dictName;
                    return isLocalFound;
                });
                isFound = profileItemIndex >= 0;
                if (isFound)
                {

                    foreach (DictProfileItem profileItem in profileItems)
                    {
                        profileItem.isDefault = false;
                    }

                    DictProfileItem currentDictProfileItem = profileItems[profileItemIndex];
                    currentDictProfileItem.isDefault = true;
                    string savedContent = js.Serialize(new SavedContent
                    {
                        bookmarks = currentBookmarks,
                        settings = currentSettings,
                        dictProfiles = updatedDictProfiles,
                        hotKeys = currentHotKeys
                    });
                    File.WriteAllText(saveDataFilePath, savedContent);

                    GetDicts();

                }
            }

        }

        private void ToggleSpeedSliderHandler (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = ((Slider)(sender));
            ToggleSpeedSlider(slider);
        }

        public void ToggleSpeedSlider (Slider slider)
        {
            if (isAppInit)
            {
                double speedSliderValue = slider.Value;
                double roundedSpeedSliderValue = ((int)(speedSliderValue));
                string rawRoundedSpeedSliderValue = roundedSpeedSliderValue.ToString();

                int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                ItemCollection openedDocControlItems = openedDocControl.Items;
                object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
                TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
                object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
                Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
                openedDocControlSelectedItemContent.speedValueLabel.Text = rawRoundedSpeedSliderValue;
            }
        }

        private void TogglePitchSliderHandler(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = ((Slider)(sender));
            TogglePitchSlider(slider);
        }

        public void TogglePitchSlider (Slider slider)
        {
            if (isAppInit)
            {
                double pitchSliderValue = slider.Value;
                double roundedPitchSliderValue = ((int)(pitchSliderValue));
                string rawRoundedPitchSliderValue = roundedPitchSliderValue.ToString();
                int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                ItemCollection openedDocControlItems = openedDocControl.Items;
                object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
                TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
                object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
                Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
                TextBlock pitchValueLabel = openedDocControlSelectedItemContent.pitchValueLabel;
                pitchValueLabel.Text = rawRoundedPitchSliderValue;

            }
        }

        private void ToggleVolumeSliderHandler (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = ((Slider)(sender));
            ToggleVolumeSlider(slider);
        }

        public void ToggleVolumeSlider (Slider slider)
        {
            if (isAppInit)
            {
                double volumeSliderValue = slider.Value;
                double roundedVolumeSliderValue = ((int)(volumeSliderValue));
                string rawRoundedVolumeSliderValue = roundedVolumeSliderValue.ToString();
                int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                ItemCollection openedDocControlItems = openedDocControl.Items;
                object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
                TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
                object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
                Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
                TextBlock volumeValueLabel = openedDocControlSelectedItemContent.volumeValueLabel;
                volumeValueLabel.Text = rawRoundedVolumeSliderValue;

            }
        }

        private void ToggleDetectBufferHandler(object sender, RoutedEventArgs e)
        {
            ToggleDetectBuffer();
        }

        public void ToggleDetectBuffer ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> currentBookmarks = loadedContent.bookmarks;
            Settings updatedSettings = loadedContent.settings;
            List<DictProfile> currentDictProfiles = loadedContent.dictProfiles;
            List<HotKey> currentHotKeys = loadedContent.hotKeys;
            bool IsChecked = detectBufferMenuItem.IsChecked;
            updatedSettings.buffer.isEnabled = IsChecked;
            string savedContent = js.Serialize(new SavedContent
            {
                bookmarks = currentBookmarks,
                settings = updatedSettings,
                dictProfiles = currentDictProfiles,
                hotKeys = currentHotKeys
            });
            File.WriteAllText(saveDataFilePath, savedContent);
        }

        private void InitHandler (object sender, RoutedEventArgs e)
        {
            Init();
        }

        void Init ()
        {
            InitVars();
            GetVoices();
            InitCache();
            CreateDoc();
            InitTray();
            InitBufferSettings();
            InitGeneralSettings();
            GetDictProfiles();
        }

        public void InitGeneralSettings ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            GeneralSettings generalSettings = currentSettings.general;
            string startupAction = generalSettings.startupAction;
            bool isOpenLastDoc = startupAction == "openLastDoc";
            bool isOpenDocAndSpeak = startupAction == "openDocAndSpeak";
            bool isCreateNewDoc = startupAction == "createNewDoc";
            if (isOpenLastDoc)
            {

            }
            else if (isOpenDocAndSpeak)
            {
                OpenDoc();
            }
            else if (isCreateNewDoc)
            {
                CreateDoc();
            }
        }

        public void InitBufferSettings ()
        {
            ClipboardMonitor detector = new ClipboardMonitor(this);
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            BufferSettings bufferSettings = currentSettings.buffer;
            bool isDetectBufferEnabled = bufferSettings.isEnabled;
            detectBufferMenuItem.IsChecked = isDetectBufferEnabled;
        }

        private void DetectCopyActionHandler (object sender, EventArgs e)
        {
            DetectCopyAction();
        }

        public void DetectCopyAction ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            BufferSettings bufferSettings = currentSettings.buffer;
            bool isDetectBufferEnabled = bufferSettings.isEnabled;
            if (isDetectBufferEnabled)
            {
                string bufferAction = bufferSettings.action;
                bool isIgnoreCopiedTextInBufferIfTextNotChanged = bufferSettings.ignoreCopiedTextInBufferIfTextNotChanged;
                bool isNotIgnoreCopiedTextInBufferIfTextNotChanged = !isIgnoreCopiedTextInBufferIfTextNotChanged;
                bool isShowNotifications = bufferSettings.showAlertTextOperationMsgs;
                bool IsBufferSpeakActionChecked = bufferAction == "speak";
                bool IsBufferCreateDocActionChecked = bufferAction == "createDoc";
                bool IsBufferAddTextToCurrentDocActionChecked = bufferAction == "addTextToCurrentDoc";
                bool IsBufferReplaceTextToCurrentDocActionChecked = bufferAction == "replaceTextToCurrentDoc";
                bool IsBufferCreateDocAndSpeakActionChecked = bufferAction == "createDocAndSpeak";
                bool IsBufferAddTextToDocAndSpeakActionChecked = bufferAction == "addTextToCurrentDocAndSpeak";
                bool IsBufferReplaceTextToCurrentDocAndSpeakActionChecked = bufferAction == "replaceTextToCurrentDocAndSpeak";
                string copiedText = Clipboard.GetText();
                bool isNotLastCopiedText = lastCopiedText != copiedText;
                bool isCanCopy = isNotIgnoreCopiedTextInBufferIfTextNotChanged || isNotLastCopiedText;
                if (isCanCopy)
                {
                    if (IsBufferSpeakActionChecked)
                    {
                        Speak();
                        if (isShowNotifications)
                        {
                            new ToastContentBuilder()
                                .AddArgument("action", "viewConversation")
                                .AddArgument("conversationId", 9813)
                                .AddText("Читаю вслух")
                                .AddText("TTS читает вслух!")
                                .Show();
                        }
                    }
                    else if (IsBufferCreateDocActionChecked)
                    {
                        CreateDoc();
                        if (isShowNotifications)
                        {
                            new ToastContentBuilder()
                                .AddArgument("action", "viewConversation")
                                .AddArgument("conversationId", 9813)
                                .AddText("Создаю документ")
                                .AddText("TTS создал документ!")
                                .Show();
                        }
                    }
                    else if (IsBufferAddTextToCurrentDocActionChecked)
                    {
                        int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                        ItemCollection openedDocControlItems = openedDocControl.Items;
                        object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
                        TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
                        object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
                        Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
                        TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
                        string inputBoxContent = inputBox.Text;
                        inputBoxContent += copiedText;
                        inputBox.Text = inputBoxContent;
                        if (isShowNotifications)
                        {
                            new ToastContentBuilder()
                                .AddArgument("action", "viewConversation")
                                .AddArgument("conversationId", 9813)
                                .AddText("Добавляю содержимое из буфера обмена")
                                .AddText("TTS добавил содержимое из буфера обмена!")
                                .Show();
                        }
                    }
                    else if (IsBufferReplaceTextToCurrentDocActionChecked)
                    {
                        int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                        ItemCollection openedDocControlItems = openedDocControl.Items;
                        object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
                        TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
                        object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
                        Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
                        TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
                        inputBox.Text = copiedText;
                        if (isShowNotifications)
                        {
                            new ToastContentBuilder()
                                .AddArgument("action", "viewConversation")
                                .AddArgument("conversationId", 9813)
                                .AddText("Заменяю текст на содержимое из буфера обмена")
                                .AddText("TTS заменил текст на содержимое из буфера обмена!")
                                .Show();
                        }
                    }
                    else if (IsBufferCreateDocAndSpeakActionChecked)
                    {
                        CreateDoc();
                        Speak();
                        if (isShowNotifications)
                        {
                            new ToastContentBuilder()
                                .AddArgument("action", "viewConversation")
                                .AddArgument("conversationId", 9813)
                                .AddText("Создаю документ и читаю вслух")
                                .AddText("TTS создал документ и прочитал вслух!")
                                .Show();
                        }
                    }
                    else if (IsBufferAddTextToDocAndSpeakActionChecked)
                    {
                        int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                        ItemCollection openedDocControlItems = openedDocControl.Items;
                        object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
                        TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
                        object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
                        Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
                        TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
                        string inputBoxContent = inputBox.Text;
                        inputBoxContent += copiedText;
                        inputBox.Text = inputBoxContent;
                        Speak();
                        if (isShowNotifications)
                        {
                            new ToastContentBuilder()
                                .AddArgument("action", "viewConversation")
                                .AddArgument("conversationId", 9813)
                                .AddText("Добавляю содержимое из буфера обмена и читаю вслух")
                                .AddText("TTS добавил текст из буфера обмена и прочитал вслух!")
                                .Show();
                        }
                    }
                    else if (IsBufferReplaceTextToCurrentDocAndSpeakActionChecked)
                    {
                        int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                        ItemCollection openedDocControlItems = openedDocControl.Items;
                        object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
                        TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
                        object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
                        Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
                        TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
                        inputBox.Text = copiedText;
                        Speak();
                        if (isShowNotifications)
                        {
                            new ToastContentBuilder()
                                .AddArgument("action", "viewConversation")
                                .AddArgument("conversationId", 9813)
                                .AddText("Заменяю текст на содержимое из буфера обмена и читаю вслух")
                                .AddText("TTS заменил текст на содержимое из буфера обмена и прочитал вслух!")
                                .Show();
                        }
                    }
                }

                string bufferedText = Clipboard.GetText();
                int minCountCharsInCopiedText = bufferSettings.minCountCharsInCopiedText;
                bool isMinCountCharsInCopiedTextEnabled = minCountCharsInCopiedText >= 1;
                if (isMinCountCharsInCopiedTextEnabled)
                {
                    bufferedText = Clipboard.GetText();
                    int bufferedTextLength = bufferedText.Length;
                    bool isNotCopy = bufferedTextLength < minCountCharsInCopiedText;
                    if (isNotCopy)
                    {
                        bufferedText = "";
                    }
                }

                lastCopiedText = bufferedText;
            }
        }

        public void InitVars ()
        {
            isAppInit = true;
            speechSynthesizer = new SpeechSynthesizer();

            // speechSynthesizer.AddLexicon(new Uri(@"C:\Users\ПК\AppData\Local\OfficeWare\SpeechReader\lexicon.xml"), "application/xml");

            speechSynthesizer.SpeakCompleted += SpeakCompletedHandler;
            openedDocHistory = new List<string>();
            speechTimerData = new Dictionary<String, Object>();
            speechTimerData.Add("isEnabled", false);
            speechTimerData.Add("isPlaySound", false);
            speechTimerData.Add("isShowAttention", false);
            speechTimerData.Add("action", "quit");
        }

        public void ShowRuleListHandler (object sender, RoutedEventArgs e)
        {
            ShowRuleList();
        }

        public void ShowRuleList ()
        {
            Dialogs.RuleListDialog dialog = new Dialogs.RuleListDialog();
            dialog.Show();
        }

        public void GetDicts ()
        {

            bool isProfileSelected = selectedDictProfileName != "";

            selectedDictIndex = -1;
            editDictMenuItem.IsEnabled = false;
            editDictMenuItem.DataContext = "";
            editDictInNotepadMenuItem.IsEnabled = false;
            editDictInNotepadMenuItem.DataContext = "";
            useDefaultDictMenuItem.IsEnabled = false;
            useDefaultDictMenuItem.DataContext = "";
            renameDictMenuItem.IsEnabled = false;
            renameDictMenuItem.DataContext = "";
            removeDictMenuItem.IsEnabled = false;
            removeDictMenuItem.DataContext = "";
            editDictBtn.IsEnabled = false;

            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string dictsFolder = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\dicts";
            dicts.Children.Clear();
            string[] dictFiles = Directory.GetFileSystemEntries(dictsFolder);
            foreach (string dictFile in dictFiles)
            {
                FileInfo fileInfo = new FileInfo(dictFile);
                string fileName = fileInfo.Name;
                StackPanel dict = new StackPanel();
                dict.Background = System.Windows.Media.Brushes.Transparent;
                dict.DataContext = dictFile;
                CheckBox dictCheckBox = new CheckBox();
                dictCheckBox.Width = 75;
                dictCheckBox.HorizontalAlignment = HorizontalAlignment.Left;

                if (isProfileSelected)
                {
                    string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string saveDataFileContent = File.ReadAllText(saveDataFilePath);
                    SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
                    List<DictProfile> currentDictProfiles = loadedContent.dictProfiles;
                    int profileIndex = currentDictProfiles.FindIndex((DictProfile profile) =>
                    {
                        string localProfileName = profile.name;
                        bool isLocalFound = localProfileName == selectedDictProfileName;
                        return isLocalFound;
                    });
                    bool isFound = profileIndex >= 0;
                    if (isFound)
                    {
                        DictProfile currentDictProfile = currentDictProfiles[profileIndex];
                        List<DictProfileItem> profileItems = currentDictProfile.items;
                        int profileItemIndex = profileItems.FindIndex((DictProfileItem profileItem) =>
                        {
                            string localProfilePath = profileItem.path;
                            bool isLocalFound = localProfilePath == dictFile;
                            return isLocalFound;
                        });
                        isFound = profileItemIndex >= 0;
                        if (isFound)
                        {
                            DictProfileItem currentDictProfileItem = profileItems[profileItemIndex];
                            bool isChecked = currentDictProfileItem.isChecked;
                            dictCheckBox.IsChecked = isChecked;
                            bool isDefault = currentDictProfileItem.isDefault;
                            if (isDefault)
                            {
                                dictCheckBox.FontWeight = FontWeights.ExtraBlack;
                            }
                        }
                    }
                }
                
                dictCheckBox.Content = fileName;
                dictCheckBox.DataContext = dictFile;
                dictCheckBox.Margin = new Thickness(15, 5, 15, 5);
                dict.Children.Add(dictCheckBox);
                dicts.Children.Add(dict);
                ContextMenu dictContextMenu = new ContextMenu();
                MenuItem dictContextMenuItem = new MenuItem();
                dictContextMenuItem.Header = "Переименовать";
                dictContextMenuItem.DataContext = dictFile;
                dictContextMenuItem.Click += RenameDictHandler;
                dictContextMenu.Items.Add(dictContextMenuItem);
                dictContextMenuItem = new MenuItem();
                dictContextMenuItem.Header = "Редактировать в блокноте";
                dictContextMenuItem.DataContext = dictFile;
                dictContextMenuItem.Click += EditDictInNotepadHandler;
                dictContextMenu.Items.Add(dictContextMenuItem);
                dictContextMenuItem = new MenuItem();
                dictContextMenuItem.Header = "Удалить";
                dictContextMenuItem.DataContext = dictFile;
                dictContextMenuItem.Click += RemoveDictHandler;
                dictContextMenu.Items.Add(dictContextMenuItem);
                // dict.ContextMenu = dictContextMenu;
                dict.ContextMenu = null;
                dict.MouseLeftButtonUp += SelectDictHandler;
            }

        }

        public void GetDictProfiles ()
        {
            
            ContextMenu dictsContextMenu = dicts.ContextMenu;
            ItemCollection dictsContextMenuItems = dictsContextMenu.Items;
            int dictsContextMenuItemsCount = dictsContextMenuItems.Count;
            int lastDictsContextMenuItemIndex = dictsContextMenuItemsCount - 1;
            for (int i = lastDictsContextMenuItemIndex; i > 11; i--)
            {
                dictsContextMenuItems.RemoveAt(i);
            }
            
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> currentBookmarks = loadedContent.bookmarks;
            Settings currentSettings = loadedContent.settings;
            List<DictProfile> currentDictProfiles = loadedContent.dictProfiles;
            List<HotKey> currentHotKeys = loadedContent.hotKeys;
            foreach (DictProfile currentDictProfile in currentDictProfiles)
            {
                string currentDictProfileName = currentDictProfile.name;
                MenuItem dictsContextMenuItem = new MenuItem();
                dictsContextMenuItem.Header = currentDictProfileName;
                dictsContextMenuItem.DataContext = currentDictProfileName;
                dictsContextMenuItem.Click += SelectDictProfileHandler;
                dicts.ContextMenu.Items.Add(dictsContextMenuItem);
            }
        }

        public void SelectDictProfileHandler (object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            object menuItemData = menuItem.DataContext;
            string profileName = ((string)(menuItemData));
            SelectDictProfile(profileName);
        }

        public void SelectDictProfile (string profileName)
        {
            selectedDictProfileName = profileName;
            GetDicts();
        }

        public void SelectDictHandler (object sender, RoutedEventArgs e)
        {
            // CheckBox checkBox = ((CheckBox)(sender));
            StackPanel checkBox = ((StackPanel)(sender));
            int selectedDictIndex = dicts.Children.IndexOf(checkBox);
            SelectDict(selectedDictIndex);
        }

        public void SelectDict(int dictIndex)
        {
            foreach (StackPanel dict in dicts.Children)
            {
                dict.Background = System.Windows.Media.Brushes.Transparent;
            }
            UIElementCollection dictsChildren = dicts.Children;
            UIElement rawSelectedDict = dictsChildren[dictIndex];
            // CheckBox selectedDict = ((CheckBox)(rawSelectedDict));
            StackPanel selectedDict = ((StackPanel)(rawSelectedDict));
            object selectedDictData = selectedDict.DataContext;
            string dictPath = ((string)(selectedDictData));
            selectedDict.Background = System.Windows.Media.Brushes.SkyBlue;
            selectedDictIndex = dictIndex;
            editDictMenuItem.IsEnabled = true;
            editDictMenuItem.DataContext = dictPath;
            editDictInNotepadMenuItem.IsEnabled = true;
            editDictInNotepadMenuItem.DataContext = dictPath;
            useDefaultDictMenuItem.IsEnabled = true;
            useDefaultDictMenuItem.DataContext = dictPath;
            renameDictMenuItem.IsEnabled = true;
            renameDictMenuItem.DataContext = dictPath;
            removeDictMenuItem.IsEnabled = true;
            removeDictMenuItem.DataContext = dictPath;
            editDictBtn.IsEnabled = true;
        }

        public void InitCache ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            string cachePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader";
            bool isCacheFolderExists = Directory.Exists(cachePath);
            bool isCacheFolderNotExists = !isCacheFolderExists;
            if (isCacheFolderNotExists)
            {
                Directory.CreateDirectory(cachePath);

                string dictsFolder = cachePath + @"\dicts";
                Directory.CreateDirectory(dictsFolder);

                using (System.IO.Stream s = File.Open(saveDataFilePath, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write("");
                    }
                };
                JavaScriptSerializer js = new JavaScriptSerializer();
                string savedContent = js.Serialize(new SavedContent()
                {
                    bookmarks = new List<Dictionary<String, Object>>() { },
                    settings = new Settings()
                    {
                        buffer = new BufferSettings()
                        {
                            isEnabled = false,
                            action = "speak",
                            ignoreTextInSoftware = false,
                            showAlertTextOperationMsgs = false,
                            ignoreCopiedTextInBufferIfTextNotChanged = false,
                            minCountCharsInCopiedText = 0
                        },
                        text = new TextSettings()
                        {
                            isOpen = false,
                            isRemoveExcessSpaces = true,
                            isRemoveNewLineChars = true,
                            isRemoveAllEmptyLines = false,
                            isReplaceManyEmptyLinesToEmptyLine = false,
                            isRemoveSpacesBeforeSemicolon = false,
                            isNotOfferSaveChangedText = false
                        },
                        general = new GeneralSettings()
                        {
                            beginReadSpeakWith = "cursorPosition",
                            beginWriteToAudioFileWith = "textStart",
                            isLetters = false,
                            isWords = false,
                            isParagraphs = false,
                            startupAction = "openLastDoc",
                            isShowSmallFloatWindow = false,
                            isTransparentSmallFloatWindow = false
                        },
                        view = new ViewSettings()
                        {
                            isShowIcons = true,
                            isShowFullPathToDoc = false,
                            isShowPercentOfWorkInTaskBar = false,
                            isHideAppInTrayWhenMinimize = true,
                            isAlwaysShowIconInTray = true
                        }
                    },
                    dictProfiles = new List<DictProfile>() { },
                    hotKeys = new List<HotKey>() {
                        new HotKey()
                        {
                            cmd = "createDoc",
                            shortcut = "Ctrl+N"
                        },
                        new HotKey()
                        {
                            cmd = "openDoc",
                            shortcut = "Ctrl+O"
                        }
                    }
                });
                File.WriteAllText(saveDataFilePath, savedContent);
            }

        }

        public void OpenDictProfileHandler (object sender, RoutedEventArgs e)
        {
            OpenDictProfile();
        }

        public void OpenDictProfile ()
        {
            Dialogs.DictProfileDialog dialog = new Dialogs.DictProfileDialog(this);
            dialog.Closed += GetDictProfilesHandler;
            dialog.Show();
        }

        public void GetDictProfilesHandler (object sender, EventArgs e)
        {
            GetDictProfiles();
        }

        public void SpeakCompletedHandler (object sender, SpeakCompletedEventArgs e)
        {
            SpeakCompleted();
        }

        public void SpeakCompleted ()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    speechSynthesizer.SetOutputToDefaultAudioDevice();
                    bool isEnabled = ((bool)(speechTimerData["isEnabled"]));
                    if (isEnabled)
                    {
                        bool isPlaySound = ((bool)(speechTimerData["isPlaySound"]));
                        bool isShowAttention = ((bool)(speechTimerData["isShowAttention"]));
                        if (isPlaySound)
                        {
                            mainAudio.Source = new Uri(@"C:\wpf_projects\TTS\TTS\Sounds\notification.wav");
                            mainAudio.Play();
                        }
                        if (isShowAttention)
                        {

                        }
                        Cancel();
                        Dictionary<String, Object> updatedSpeechTimerData = new Dictionary<String, Object>();
                        updatedSpeechTimerData.Add("isEnabled", false);
                        updatedSpeechTimerData.Add("isPlaySound", false);
                        updatedSpeechTimerData.Add("isShowAttention", false);
                        updatedSpeechTimerData.Add("action", "quit");
                        speechTimerData = updatedSpeechTimerData;
                    }
                });
            }
            catch (Exception)
            {

            }
        }

        public void SpeakBufferHandler (object sender, RoutedEventArgs e)
        {
            SpeakBuffer();
        }

        public void SpeakBuffer ()
        {

            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            Slider speedSlider = openedDocControlSelectedItemContent.speedSlider;
            double speedSliderValue = speedSlider.Value;
            int roundedSpeedSliderValue = ((int)(speedSliderValue));
            speechSynthesizer.Rate = roundedSpeedSliderValue;

            Slider pitchSlider = openedDocControlSelectedItemContent.pitchSlider;
            double pitchSliderValue = pitchSlider.Value;
            int roundedPitchSliderValue = ((int)(pitchSliderValue));

            Slider volumeSlider = openedDocControlSelectedItemContent.volumeSlider;
            double volumeSliderValue = volumeSlider.Value;
            
            int roundedVolumeSliderValue = ((int)(volumeSliderValue));
            speechSynthesizer.Volume = roundedVolumeSliderValue;
            string copiedText = Clipboard.GetText();
            ResetPause();

            PromptBuilder builder = new PromptBuilder();
            // builder.Culture = CultureInfo.CreateSpecificCulture("ru-RU");
            builder.Culture = CultureInfo.CreateSpecificCulture("en-US");
            builder.StartVoice(builder.Culture);
            builder.StartSentence();
            PromptEmphasis emphasis = PromptEmphasis.Moderate;
            bool isStrong = roundedPitchSliderValue < 0;
            bool isMiddle = roundedPitchSliderValue == 0;
            bool isHigh = roundedPitchSliderValue > 0;
            string pitch = "default";
            if (isStrong)
            {
                emphasis = PromptEmphasis.Strong;
                pitch = "default";
            }
            else if (isMiddle)
            {
                emphasis = PromptEmphasis.Moderate;
                pitch = "default";
            }
            else if (isHigh)
            {
                emphasis = PromptEmphasis.Reduced;
                pitch = "default";
            }
            builder.StartStyle(new PromptStyle() { Emphasis = emphasis });

            // builder.AppendSsmlMarkup("<say-as interpret-as = \"red\"> green </say-as>");
            // builder.AppendSsmlMarkup("<say-as interpret-as=\"red\">green</say-as>");
            // builder.AppendTextWithAlias("green", "red");

            // builder.AppendText(copiedText);
            builder.AppendSsmlMarkup("<prosody pitch=\"" + pitch + "\">" + copiedText + "</prosody>");

            builder.EndStyle();
            builder.EndSentence();
            builder.EndVoice();

            speechSynthesizer.SpeakAsync(builder);            
            // speechSynthesizer.SpeakSsmlAsync("<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\"><phoneme ph=\"" + "red" + "\">" + "green" + "</phoneme></speak>");


            stopSpeechBtn.IsEnabled = true;
        }

        public void ResetPause ()
        {
            SynthesizerState speechSynthesizerState = speechSynthesizer.State;
            SynthesizerState pausedState = SynthesizerState.Paused;
            bool isPaused = speechSynthesizerState == pausedState;
            if (isPaused)
            {
                speechSynthesizer.SpeakAsyncCancelAll();
                speechSynthesizer.Resume();
            }
        }

        public void SpeakSelectionHandler (object sender, RoutedEventArgs e)
        {
            SpeakSelection();
        }

        public void SpeakSelection ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            Slider speedSlider = openedDocControlSelectedItemContent.speedSlider;
            double speedSliderValue = speedSlider.Value;
            int roundedSpeedSliderValue = ((int)(speedSliderValue));
            speechSynthesizer.Rate = roundedSpeedSliderValue;

            Slider pitchSlider = openedDocControlSelectedItemContent.pitchSlider;
            double pitchSliderValue = pitchSlider.Value;
            int roundedPitchSliderValue = ((int)(pitchSliderValue));

            Slider volumeSlider = openedDocControlSelectedItemContent.volumeSlider;
            double volumeSliderValue = volumeSlider.Value;
            int roundedVolumeSliderValue = ((int)(volumeSliderValue));
            speechSynthesizer.Volume = roundedVolumeSliderValue;
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxSelectedContent = inputBox.SelectedText;
            ResetPause();

            PromptBuilder builder = new PromptBuilder();
            builder.Culture = CultureInfo.CreateSpecificCulture("ru-RU");
            builder.StartVoice(builder.Culture);
            builder.StartSentence();
            PromptEmphasis emphasis = PromptEmphasis.Moderate;
            bool isStrong = roundedPitchSliderValue < 0;
            bool isMiddle = roundedPitchSliderValue == 0;
            bool isHigh = roundedPitchSliderValue > 0;
            if (isStrong)
            {
                emphasis = PromptEmphasis.Strong;
            }
            else if (isMiddle)
            {
                emphasis = PromptEmphasis.Moderate;
            }
            else if (isHigh)
            {
                emphasis = PromptEmphasis.Reduced;
            }
            builder.StartStyle(new PromptStyle() { Emphasis = emphasis });
            builder.AppendText(inputBoxSelectedContent);
            builder.EndStyle();
            builder.EndSentence();
            builder.EndVoice();
            speechSynthesizer.SpeakAsync(builder);
            stopSpeechBtn.IsEnabled = true;
        }

        public void SpeakHandler (object sender, RoutedEventArgs e)
        {
            Speak();
        }

        public void Speak ()
        {

            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            GeneralSettings generalSettings = currentSettings.general;
            string beginReadSpeakWith = generalSettings.beginReadSpeakWith;
            bool isCursorPosition = beginReadSpeakWith == "cursorPosition";
            bool isTextStart = beginReadSpeakWith == "textStart";
            bool isParagraphStart = beginReadSpeakWith == "paragraphStart";

            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            Slider speedSlider = openedDocControlSelectedItemContent.speedSlider;
            double speedSliderValue = speedSlider.Value;
            int roundedSpeedSliderValue = ((int)(speedSliderValue));
            speechSynthesizer.Rate = roundedSpeedSliderValue;
            
            Slider pitchSlider = openedDocControlSelectedItemContent.pitchSlider;
            double pitchSliderValue = pitchSlider.Value;
            int roundedPitchSliderValue = ((int)(pitchSliderValue));

            Slider volumeSlider = openedDocControlSelectedItemContent.volumeSlider;
            double volumeSliderValue = volumeSlider.Value;
            int roundedVolumeSliderValue = ((int)(volumeSliderValue));
            speechSynthesizer.Volume = roundedVolumeSliderValue;
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            
            string inputBoxContent = inputBox.Text;
            if (isCursorPosition)
            {
                int charIndex = inputBox.SelectionStart;
                int inputBoxContentLength = inputBoxContent.Length;
                int leftLength = inputBoxContentLength - charIndex;
                inputBoxContent = inputBox.Text.Substring(charIndex, leftLength);
            }
            else if (isTextStart)
            {
                inputBoxContent = inputBox.Text;
            }
            else if (isParagraphStart)
            {
                inputBoxContent = inputBox.Text;
            }

            ResetPause();

            PromptBuilder builder = new PromptBuilder();
            
            builder.Culture = CultureInfo.CreateSpecificCulture("ru-RU");
            // builder.Culture = CultureInfo.CreateSpecificCulture("en-US");
            
            builder.StartVoice(builder.Culture);
            builder.StartSentence();
            PromptEmphasis emphasis = PromptEmphasis.Moderate;
            bool isStrong = roundedPitchSliderValue < 0;
            bool isMiddle = roundedPitchSliderValue == 0;
            bool isHigh = roundedPitchSliderValue > 0;
            string pitch = "default";
            if (isStrong)
            {
                emphasis = PromptEmphasis.Strong;
                // pitch = "x-low";
                pitch = "-50st";
            }
            else if (isMiddle)
            {
                emphasis = PromptEmphasis.Moderate;
                // pitch = "default";
                pitch = "0st";
            }
            else if (isHigh)
            {
                emphasis = PromptEmphasis.Reduced;
                // pitch = "x-high";
                pitch = "50st";
            }
            builder.StartStyle(new PromptStyle() { Emphasis = emphasis });

            // builder.AppendSsmlMarkup("<say-as interpret-as = \"red\"> green </say-as>");
            // builder.AppendSsmlMarkup("<say-as interpret-as=\"red\">green</say-as>");
            // builder.AppendTextWithAlias("green", "red");

            // builder.AppendText(inputBoxContent);
            builder.AppendSsmlMarkup("<prosody pitch=\"" + pitch + "\">" + inputBoxContent + "</prosody>");
            
            builder.EndStyle();
            builder.EndSentence();
            builder.EndVoice();


            speechSynthesizer.SpeakAsync(builder);
            // speechSynthesizer.SpeakAsync(inputBoxContent);
            // speechSynthesizer.SpeakSsmlAsync("<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\"><phoneme ph=\"" + "red" + "\">" + "green" + "</phoneme></speak>");
            // speechSynthesizer.SpeakSsmlAsync("<phoneme alphabet=\"x-microsoft-ups\" ph=\"J AA M AX\"> hello </phoneme>");
            // speechSynthesizer.SpeakSsmlAsync("<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\"><sub alias=\"World Wide Web Consortium\">W3C</sub></speak>");
            // speechSynthesizer.SpeakSsmlAsync("<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\"><prosody pitch=\"" + pitch + "\">" + inputBoxContent + "</prosody></speak>");
            
            stopSpeechBtn.IsEnabled = true;
        }

        public void CreateDocHandler (object sender, RoutedEventArgs e)
        {
            CreateDoc();
        }

        public void CreateDoc (string path = "")
        {

            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            ViewSettings viewSettings = currentSettings.view;
            bool isShowFullPathToDoc = viewSettings.isShowFullPathToDoc;

            UIElementCollection totalOpenedDocs = openedDocs.Children;
            int countOpenedDocs = totalOpenedDocs.Count;
            int documentNumber = countOpenedDocs + 1;
            string rawDocumentNumber = documentNumber.ToString();
            Button openedDoc = new Button();
            StackPanel openedDocContent = new StackPanel();
            openedDocContent.Orientation = Orientation.Horizontal;
            PackIcon openedDocContentIcon = new PackIcon();
            openedDocContentIcon.Kind = PackIconKind.FileDocument;
            openedDocContentIcon.Margin = new Thickness(4, 2, 4, 2);
            openedDocContent.Children.Add(openedDocContentIcon);
            TextBlock openedDocContentLabel = new TextBlock();
            string openedDocContentLabelContent = "Документ" + rawDocumentNumber;
            openedDocContentLabel.Text = openedDocContentLabelContent;
            openedDocContentLabel.Margin = new Thickness(4, 2, 4, 2);
            openedDocContent.Children.Add(openedDocContentLabel);
            openedDoc.Content = openedDocContent;
            openedDocs.Children.Add(openedDoc);
            TabItem docTab = new TabItem();
            docTab.Visibility = Visibility.Collapsed;
            Controls.OpenedDocControl docTabContent = new Controls.OpenedDocControl();
            docTabContent.DataContext = this;
            docTab.Content = docTabContent;
            openedDocControl.Items.Add(docTab);

            bool isPathExists = path != "";
            if (isPathExists)
            {
                /*
                if (isShowFullPathToDoc)
                {
                    openedDoc.DataContext = path;
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(path);
                    string fileName = fileInfo.Name;
                    openedDoc.DataContext = fileName;
                }
                */
                FileInfo fileInfo = new FileInfo(path);
                string fileName = fileInfo.Name;
                openedDoc.DataContext = fileName;
            }
            else
            {
                openedDoc.DataContext = openedDocContentLabelContent;
            }

            openedDoc.Click += SelectOpenedDocHandler;
            SelectOpenedDoc(openedDoc);

            TextBox inputBox = docTabContent.inputBox;
            CommandManager.AddPreviewCanExecuteHandler(inputBox, _canExecute);

        }

        private void _canExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            BufferSettings bufferSettings = currentSettings.buffer;
            bool isDetectBufferEnabled = bufferSettings.isEnabled;
            if (isDetectBufferEnabled)
            {
                bool isIgnoreTextInSoftware = bufferSettings.ignoreTextInSoftware;
                if (isIgnoreTextInSoftware)
                {
                    e.CanExecute = false;
                    e.Handled = true;
                }
                else
                {
                    e.CanExecute = true;
                    e.Handled = false;
                }
            }
            else
            {
                e.CanExecute = true;
                e.Handled = false;
            }
        }

        public void SelectOpenedDocHandler (object sender, RoutedEventArgs e)
        {
            Button btn = ((Button)(sender));
            SelectOpenedDoc(btn);
        }

        public void SelectOpenedDoc (Button selectedOpenedDoc)
        {

            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            ViewSettings viewSettings = currentSettings.view;
            bool isShowFullPathToDoc = viewSettings.isShowFullPathToDoc;

            UIElementCollection totalOpenedDocs = openedDocs.Children;
            foreach (Button openedDoc in totalOpenedDocs)
            {
                openedDoc.BorderThickness = new Thickness(1);
                openedDoc.Background = System.Windows.Media.Brushes.LightGray;
            }
            selectedOpenedDoc.BorderThickness = new Thickness(0);
            selectedOpenedDoc.Background = System.Windows.Media.Brushes.Transparent;
            int index = openedDocs.Children.IndexOf(selectedOpenedDoc);
            openedDocControl.SelectedIndex = index;

            object rawSelectedOpenedDocData = selectedOpenedDoc.DataContext;
            
            string selectedOpenedDocData = ((string)(rawSelectedOpenedDocData));
            
            string titleContent = "TTS - [" + selectedOpenedDocData + "]";
            if (isShowFullPathToDoc)
            {
                titleContent = "TTS - [" + selectedOpenedDocData + "]";
            }
            else
            {
                FileInfo fileInfo = new FileInfo(selectedOpenedDocData);
                string fileName = fileInfo.Name;
                titleContent = "TTS - [" + fileName + "]";
            }

            this.Title = titleContent;
        
        }

        public void SelectAllInputHandler (object sender, RoutedEventArgs e)
        {
            SelectAllInput();
        }

        public void SelectAllInput ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            inputBox.SelectAll();
        }

        public void InsertTextHandler (object sender, RoutedEventArgs e)
        {
            InsertText();
        }

        public void InsertText ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            int startSelectionIndex = inputBox.SelectionStart;
            string inputBoxContent = inputBox.Text;
            string copiedText = Clipboard.GetText();
            int copiedTextLength = copiedText.Length;
            inputBoxContent = inputBoxContent.Insert(startSelectionIndex, copiedText);
            inputBox.Text = inputBoxContent;
            inputBox.SelectionStart = startSelectionIndex + copiedTextLength;
        }

        public void TogglFullScreenHandler (object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            TogglFullScreen(menuItem);
        }

        public void TogglFullScreen (MenuItem menuItem)
        {
            bool isChecked = menuItem.IsChecked;
            if (isChecked)
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.SingleBorderWindow; 
            }
        }

        public void IncreaseFontSizeHandler (object sender, RoutedEventArgs e)
        {
            IncreaseFontSize();
        }

        public void IncreaseFontSize ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            inputBox.FontSize++;
        }

        public void DecreaseFontSizeHandler (object sender, RoutedEventArgs e)
        {
            DecreaseFontSize();
        }

        public void DecreaseFontSize ()
        {
            
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            double fontSize = inputBox.FontSize;
            bool isCanDecrease = fontSize > 8;
            if (isCanDecrease)
            {
                inputBox.FontSize--;
            }
        }

        public void IncreaseSpeedHandler (object sender, RoutedEventArgs e)
        {
            IncreaseSpeed();
        }

        public void IncreaseSpeed ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            Slider speedSlider = openedDocControlSelectedItemContent.speedSlider;
            speedSlider.Value--;
        }

        public void DecreaseSpeedHandler (object sender, RoutedEventArgs e)
        {
            DecreaseSpeed();
        }

        public void DecreaseSpeed()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            Slider speedSlider = openedDocControlSelectedItemContent.speedSlider; 
            speedSlider.Value--;
        }

        public void IncreasePitchHandler (object sender, RoutedEventArgs e)
        {
            IncreasePitch();
        }

        public void IncreasePitch ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            Slider pitchSlider = openedDocControlSelectedItemContent.pitchSlider;
            pitchSlider.Value++;
        }

        public void DecreasePitchHandler(object sender, RoutedEventArgs e)
        {
            DecreasePitch();
        }

        public void DecreasePitch ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            Slider pitchSlider = openedDocControlSelectedItemContent.pitchSlider;
            pitchSlider.Value--;
        }

        public void IncreaseVolumeHandler(object sender, RoutedEventArgs e)
        {
            IncreaseVolume();
        }

        public void IncreaseVolume ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            Slider volumeSlider = openedDocControlSelectedItemContent.volumeSlider; 
            volumeSlider.Value++;
        }

        public void DecreaseVolumeHandler (object sender, RoutedEventArgs e)
        {
            DecreaseVolume();
        }

        public void DecreaseVolume ()
        {

            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            Slider volumeSlider = openedDocControlSelectedItemContent.volumeSlider;
            volumeSlider.Value--;
        }

        public void GoToNextLineHandler (object sender, RoutedEventArgs e)
        {
            GoToNextLine();
        }

        public void GoToNextLine ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            int selectedStartIndex = inputBox.SelectionStart;
            int lineIndex = inputBox.GetLineIndexFromCharacterIndex(selectedStartIndex);
            int countLines = inputBox.LineCount;
            int lastLineIndex = countLines - 1;
            bool isNotLastLine = lineIndex < lastLineIndex;
            string inputBoxContent = inputBox.Text;
            int inputBoxContentLength = inputBoxContent.Length;
            int index = inputBoxContentLength - 1;
            if (isNotLastLine)
            {
                int nextLineIndex = lineIndex + 1;
                index = inputBox.GetCharacterIndexFromLineIndex(nextLineIndex);
            }
            inputBox.Select(index, 0);
        }

        public void GoToPreviousLineHandler (object sender, RoutedEventArgs e)
        {
            GoToPreviousLine();
        }

        public void GoToPreviousLine()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            int selectedStartIndex = inputBox.SelectionStart;
            int lineIndex = inputBox.GetLineIndexFromCharacterIndex(selectedStartIndex);
            bool isNotFirstLine = lineIndex >= 1;
            int index = 0;
            if (isNotFirstLine)
            {
                int previousLineIndex = lineIndex - 1;
                index = inputBox.GetCharacterIndexFromLineIndex(previousLineIndex);
            }
            inputBox.Select(index, 0);
        }

        public void StopHandler (object sender, RoutedEventArgs e)
        {
            Stop();
        }

        public void Stop ()
        {
            speechSynthesizer.Pause();
            stopSpeechBtn.IsEnabled = false;
        }

        public void TranslateHandler (object sender, RoutedEventArgs e)
        {
            Translate();
        }

        public void Translate ()
        {
            Dialogs.TranslateDialog dialog = new Dialogs.TranslateDialog(this);
            dialog.Show();
        }

        public void CloseDocHandler (object sender, RoutedEventArgs e)
        {
            CloseDoc();
        }

        public void CloseDoc ()
        {
            int selectedIndex = openedDocControl.SelectedIndex;
            openedDocControl.Items.RemoveAt(selectedIndex);
            openedDocs.Children.RemoveAt(selectedIndex);
            UIElementCollection openedDocsChildren = openedDocs.Children;
            int openedDocsChildrenCount = openedDocsChildren.Count;
            bool isNotDocs = openedDocsChildrenCount <= 0;
            if (isNotDocs)
            {
                CreateDoc();
            }
            else
            {
                UIElement rawOpenedDoc = openedDocsChildren[0];
                Button openedDoc = ((Button)(rawOpenedDoc));
                SelectOpenedDoc(openedDoc);
            }
        }

        public void CloseAllDocsHandler (object sender, RoutedEventArgs e)
        {
            CloseAllDocs();
        }

        public void CloseAllDocs ()
        {
            openedDocControl.Items.Clear();
            openedDocs.Children.Clear();
            CreateDoc();
        }

        public void CloseAllExceptCurrentDocHandler (object sender, RoutedEventArgs e)
        {
            CloseAllExceptCurrentDoc();
        }

        public void CloseAllExceptCurrentDoc ()
        {
            int selectedIndex = openedDocControl.SelectedIndex;
            UIElementCollection openedDocsChildren = openedDocs.Children;
            int openedDocsChildrenCount = openedDocsChildren.Count;
            List<int> closedDocs = new List<int>();
            for (int i = openedDocsChildrenCount - 1; i >= 0;  i--)
            {
                bool isCurrentDoc = i == selectedIndex;
                bool isCanClose = !isCurrentDoc;
                if (isCanClose)
                {
                    closedDocs.Add(i);
                }
            }
            foreach (int closedDoc in closedDocs)
            {
                openedDocControl.Items.RemoveAt(closedDoc);
                openedDocs.Children.RemoveAt(closedDoc);
            }
        }

        public void CancelHandler (object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel ()
        {
            this.Close();
        }

        public void SelectOutputDeviceHandler (object sender, RoutedEventArgs e)
        {
            SelectOutputDevice();
        }

        public void SelectOutputDevice ()
        {
            Dialogs.SelectOutputDevieDialog dialog = new Dialogs.SelectOutputDevieDialog(this);
            dialog.Show();
        }

        public void GetVoices ()
        {
            var voices = speechSynthesizer.GetInstalledVoices();
            foreach (var voice in voices)
            {
                VoiceInfo voiceInfo = voice.VoiceInfo;
                string voiceInfoName = voiceInfo.Name;
                MenuItem voiceMenuItem = new MenuItem();
                voiceMenuItem.Header = voiceInfoName;
                voicesMenuItem.Items.Add(voiceMenuItem);
                voicesMenuItem.DataContext = voiceInfoName;
                voicesMenuItem.MouseLeftButtonUp += SetVoiceHandler;
            }
        }

        public void SetVoiceHandler (object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            object menuItemData = menuItem.DataContext;
            string voiceInfoName = ((string)(menuItemData));
            SetVoice(voiceInfoName);
        }

        public void SetVoice (string voiceInfoName)
        {
            speechSynthesizer.SelectVoice(voiceInfoName);
        }

        public void SaveAudioFileHandler (object sender, RoutedEventArgs e)
        {
            SaveAudioFile();
        }

        public void SaveAudioFile ()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Документ";
            sfd.DefaultExt = ".wav";
            sfd.Filter = "Аудиофайлы (.wav)|*.wav";
            bool? res = sfd.ShowDialog();
            bool isSave = ((bool)(res));
            if (isSave)
            {

                Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
                string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
                string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
                JavaScriptSerializer js = new JavaScriptSerializer();
                string saveDataFileContent = File.ReadAllText(saveDataFilePath);
                SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
                Settings currentSettings = loadedContent.settings;
                GeneralSettings generalSettings = currentSettings.general;
                string beginWriteToAudioFileWith = generalSettings.beginWriteToAudioFileWith;
                bool isCursorPosition = beginWriteToAudioFileWith == "cursorPosition";
                bool isTextStart = beginWriteToAudioFileWith == "textStart";
                bool isParagraphStart = beginWriteToAudioFileWith == "paragraphStart";

                string path = sfd.FileName;
                speechSynthesizer.SetOutputToWaveFile(path);
                int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                ItemCollection openedDocControlItems = openedDocControl.Items;
                object rawOpenedDocControlItem = openedDocControlItems[openedDocControlSelectedIndex];
                TabItem openedDocControlItem = ((TabItem)(rawOpenedDocControlItem));
                object rawOpenedDocControlItemContent = openedDocControlItem.Content;
                Controls.OpenedDocControl openedDocControlItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlItemContent));
                TextBox inputBox = openedDocControlItemContent.inputBox;
                
                string inputBoxContent = inputBox.Text;
                if (isCursorPosition)
                {
                    int charIndex = inputBox.SelectionStart;
                    int inputBoxContentLength = inputBoxContent.Length;
                    int leftLength = inputBoxContentLength - charIndex;
                    inputBoxContent = inputBox.Text.Substring(charIndex, leftLength);
                }
                else if (isTextStart)
                {
                    inputBoxContent = inputBox.Text;
                }
                else if (isParagraphStart)
                {
                    inputBoxContent = inputBox.Text;
                }

                speechSynthesizer.SpeakAsync(inputBoxContent);
                bool isExit = exitAfterSaveAudioMenuItem.IsChecked;
                if (isExit)
                {
                    Cancel();
                }
            }
        }

        public void OpenFontAndColorsHandler (object sender, RoutedEventArgs e)
        {
            OpenFontAndColors();
        }

        public void OpenFontAndColors ()
        {
            Dialogs.FontAndColors dialog = new Dialogs.FontAndColors(this);
            dialog.Closed += ApplyFontAndColorHandler;
            dialog.Show();
        }

        public void ApplyFontAndColorHandler (object sender, EventArgs e)
        {
            Dialogs.FontAndColors dialog = ((Dialogs.FontAndColors)(sender));
            ApplyFontAndColor(dialog);
        }

        public void ApplyFontAndColor (Dialogs.FontAndColors dialog)
        {
            BrushConverter converter = new BrushConverter();
            Xceed.Wpf.Toolkit.ColorPicker textColorPicker = dialog.textColorPicker;
            Color? possibleColor = textColorPicker.SelectedColor;
            bool isColorExists = possibleColor != null;
            Brush textBrush = System.Windows.Media.Brushes.Black;
            if (isColorExists)
            {
                Color selectedColor = possibleColor.Value;
                string rawSelectedColor = selectedColor.ToString();
                object rawBrush = converter.ConvertFromString(rawSelectedColor);
                textBrush = ((Brush)(rawBrush));
            }
            Xceed.Wpf.Toolkit.ColorPicker backgroundColorPicker = dialog.backgroundColorPicker;
            possibleColor = backgroundColorPicker.SelectedColor;
            isColorExists = possibleColor != null;
            Brush backgroundBrush = System.Windows.Media.Brushes.Transparent;
            if (isColorExists)
            {
                Color selectedColor = possibleColor.Value;
                string rawSelectedColor = selectedColor.ToString();
                object rawBrush = converter.ConvertFromString(rawSelectedColor);
                backgroundBrush = ((Brush)(rawBrush));
            }
            Xceed.Wpf.Toolkit.ColorPicker glowColorPicker = dialog.glowColorPicker;
            possibleColor = glowColorPicker.SelectedColor;
            isColorExists = possibleColor != null;
            Brush glowBrush = System.Windows.Media.Brushes.Transparent;
            if (isColorExists)
            {
                Color selectedColor = possibleColor.Value;
                string rawSelectedColor = selectedColor.ToString();
                object rawBrush = converter.ConvertFromString(rawSelectedColor);
                glowBrush = ((Brush)(rawBrush));
            }
            Xceed.Wpf.Toolkit.ColorPicker selectionColorPicker = dialog.selectionColorPicker;
            possibleColor = selectionColorPicker.SelectedColor;
            isColorExists = possibleColor != null;
            Brush selectionBrush = System.Windows.Media.Brushes.SkyBlue;
            if (isColorExists)
            {

                Color selectedColor = possibleColor.Value;
                string rawSelectedColor = selectedColor.ToString();
                object rawBrush = converter.ConvertFromString(rawSelectedColor);
                selectionBrush = ((Brush)(rawBrush));
            }
            Xceed.Wpf.Toolkit.ColorPicker selectionTextColorPicker = dialog.selectionTextColorPicker;
            possibleColor = selectionTextColorPicker.SelectedColor;
            isColorExists = possibleColor != null;
            Brush selectionTextBrush = System.Windows.Media.Brushes.SkyBlue;
            if (isColorExists)
            {
                Color selectedColor = possibleColor.Value;
                string rawSelectedColor = selectedColor.ToString();
                object rawBrush = converter.ConvertFromString(rawSelectedColor);
                selectionTextBrush = ((Brush)(rawBrush));
            }
            TextBlock exampleLabel = dialog.textColorLabel;
            double exampleLabelFontSize = exampleLabel.FontSize;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            foreach (TabItem openedDocControlItem in openedDocControlItems)
            {
                object rawOpenedDocControlItemContent = openedDocControlItem.Content;
                Controls.OpenedDocControl openedDocControlItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlItemContent));
                TextBox inputBox = openedDocControlItemContent.inputBox;
                inputBox.Foreground = textBrush;
                inputBox.Background = backgroundBrush;
                inputBox.SelectionBrush = selectionBrush;
                inputBox.FontSize = exampleLabelFontSize;
            }
        }

        public void OpenDocHandler (object sender, RoutedEventArgs e)
        {
            OpenDoc();
        }

        public void OpenDoc ()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "Документы (.txt)|*.txt";
            bool? res = ofd.ShowDialog();
            bool isOpen = ((bool)(res));
            if (isOpen)
            {
                Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
                string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
                string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
                JavaScriptSerializer js = new JavaScriptSerializer();
                string saveDataFileContent = File.ReadAllText(saveDataFilePath);
                SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
                Settings currentSettings = loadedContent.settings;
                TextSettings textSettings = currentSettings.text;
                ViewSettings viewSettings = currentSettings.view;
                bool isShowPercentOfWorkInTaskBar = viewSettings.isShowPercentOfWorkInTaskBar;
                bool isFormat = textSettings.isOpen;
                TaskbarItemProgressState normalProgressState = TaskbarItemProgressState.Normal;
                TaskbarItemProgressState indeterminateProgressState = TaskbarItemProgressState.Indeterminate;
                if (isShowPercentOfWorkInTaskBar)
                {
                    this.TaskbarItemInfo.ProgressState = indeterminateProgressState;
                }
                string path = ofd.FileName;
                InsertDoc(path);
                if (isFormat)
                {
                    DispatcherTimer delay = new DispatcherTimer();
                    delay.Interval = TimeSpan.FromSeconds(0.5);
                    delay.Tick += delegate
                    {
                        FormatText();
                    };
                    delay.Start();
                }
                if (isShowPercentOfWorkInTaskBar)
                {
                    DispatcherTimer progressDelay = new DispatcherTimer();
                    progressDelay.Interval = TimeSpan.FromSeconds(1.0);
                    progressDelay.Tick += delegate
                    {
                        this.TaskbarItemInfo.ProgressState = normalProgressState;
                    };
                    progressDelay.Start();
                }
            }
        }

        public void ClearOpenDocHistoryHandler (object sender, RoutedEventArgs e)
        {
            ClearOpenDocHistory();
        }

        public void ClearOpenDocHistory ()
        {
            openedDocHistory.Clear();
        }

        private void RefreshOpenDocHistoryHandler (object sender, RoutedEventArgs e)
        {
            RefreshOpenDocHistory();
        }

        public void RefreshOpenDocHistory ()
        {
            ItemCollection reOpenMenuItemItems = reOpenMenuItem.Items;
            int reOpenMenuItemItemsCount = reOpenMenuItemItems.Count;
            for (int i = reOpenMenuItemItemsCount - 1; i > 0; i--)
            {
                reOpenMenuItem.Items.RemoveAt(i);
            }
            foreach (string openedDocHistoryItem in openedDocHistory)
            {
                string fileName = System.IO.Path.GetFileName(openedDocHistoryItem);
                MenuItem menuItem = new MenuItem();
                menuItem.Header = fileName;
                menuItem.DataContext = openedDocHistoryItem;
                menuItem.Click += OpenRecentDocHandler;
                reOpenMenuItem.Items.Add(menuItem);
            }
        }

        public void OpenRecentDocHandler (object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            object menuItemData = menuItem.DataContext;
            string path = ((string)(menuItemData));
            OpenRecentDoc(path);
        }

        public void OpenRecentDoc (string path)
        {
            InsertDoc(path);
        }

        public void InsertDoc (string path)
        {
            CreateDoc(path);
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlItem = ((TabItem)(rawOpenedDocControlItem));
            object rawOpenedDocControlItemContent = openedDocControlItem.Content;
            Controls.OpenedDocControl openedDocControlItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlItemContent));
            TextBox inputBox = openedDocControlItemContent.inputBox;
            string content = File.ReadAllText(path);
            inputBox.Text = content;
            string fileName = System.IO.Path.GetFileName(path);
            UIElementCollection openedDocsChildren = openedDocs.Children;
            UIElement rawOpenedDoc = openedDocsChildren[openedDocControlSelectedIndex];
            Button openedDoc = ((Button)(rawOpenedDoc));
            object rawOpenedDocContent = openedDoc.Content;
            StackPanel openedDocContent = ((StackPanel)(rawOpenedDocContent));
            UIElementCollection openedDocContentChildren = openedDocContent.Children;
            UIElement rawOpenedDocContentLabel = openedDocContentChildren[1];
            TextBlock openedDocContentLabel = ((TextBlock)(rawOpenedDocContentLabel));
            openedDocContentLabel.Text = fileName;
            openedDocHistory.Add(path);

            openedDoc.DataContext = path;

        }

        public void SaveDocHandler (object sender, RoutedEventArgs e)
        {
            SaveDoc();
        }

        public void SaveDoc ()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".txt";
            sfd.Filter = "Текстовые (.txt)|*.txt";
            bool? res = sfd.ShowDialog();
            bool isSave = ((bool)(res));
            if (isSave)
            {

                Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
                string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
                string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
                JavaScriptSerializer js = new JavaScriptSerializer();
                string saveDataFileContent = File.ReadAllText(saveDataFilePath);
                SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
                Settings currentSettings = loadedContent.settings;
                ViewSettings viewSettings = currentSettings.view;
                bool isShowPercentOfWorkInTaskBar = viewSettings.isShowPercentOfWorkInTaskBar;

                TaskbarItemProgressState normalProgressState = TaskbarItemProgressState.Normal;
                TaskbarItemProgressState indeterminateProgressState = TaskbarItemProgressState.Indeterminate;
                if (isShowPercentOfWorkInTaskBar)
                {
                    this.TaskbarItemInfo.ProgressState = indeterminateProgressState;
                }
                string path = sfd.FileName;
                int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                ItemCollection openedDocControlItems = openedDocControl.Items;
                object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
                TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
                object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
                Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
                Slider speedSlider = openedDocControlSelectedItemContent.speedSlider;
                double speedSliderValue = speedSlider.Value;
                int roundedSpeedSliderValue = ((int)(speedSliderValue));
                speechSynthesizer.Rate = roundedSpeedSliderValue;
                Slider volumeSlider = openedDocControlSelectedItemContent.volumeSlider;
                double volumeSliderValue = volumeSlider.Value;
                int roundedVolumeSliderValue = ((int)(volumeSliderValue));
                speechSynthesizer.Volume = roundedVolumeSliderValue;
                TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
                string inputBoxContent = inputBox.Text;
                File.WriteAllText(path, inputBoxContent);
                if (isShowPercentOfWorkInTaskBar)
                {
                    DispatcherTimer progressDelay = new DispatcherTimer();
                    progressDelay.Interval = TimeSpan.FromSeconds(1.0);
                    progressDelay.Tick += delegate
                    {
                        this.TaskbarItemInfo.ProgressState = normalProgressState;
                    };
                    progressDelay.Start();
                }
            }
        }

        public void SaveDocAsHandler (object sender, RoutedEventArgs e)
        {
            SaveDocAs();
        }

        public void SaveDocAs ()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            // sfd.FileName = "Документ";
            sfd.DefaultExt = ".txt";
            sfd.Filter = "Текстовые (.txt)|*.txt";
            bool? res = sfd.ShowDialog();
            bool isSave = ((bool)(res));
            if (isSave)
            {
                string path = sfd.FileName;
                int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                ItemCollection openedDocControlItems = openedDocControl.Items;
                object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
                TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
                object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
                Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
                Slider speedSlider = openedDocControlSelectedItemContent.speedSlider;
                double speedSliderValue = speedSlider.Value;
                int roundedSpeedSliderValue = ((int)(speedSliderValue));
                speechSynthesizer.Rate = roundedSpeedSliderValue;
                Slider volumeSlider = openedDocControlSelectedItemContent.volumeSlider;
                double volumeSliderValue = volumeSlider.Value;
                int roundedVolumeSliderValue = ((int)(volumeSliderValue));
                speechSynthesizer.Volume = roundedVolumeSliderValue;
                TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
                string inputBoxContent = inputBox.Text;
                File.WriteAllText(path, inputBoxContent);
            }
        }

        public void InsertAudioHandler (object sender, RoutedEventArgs e)
        {
            InsertAudio();
        }

        public void InsertAudio ()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".wav";
            ofd.Filter = "Аудиоклипы (.wav)|*.wav";
            bool? res = ofd.ShowDialog();
            bool isOpen = ((bool)(res));
            if (isOpen)
            {
                string audioContent = ofd.FileName;
                int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                ItemCollection openedDocControlItems = openedDocControl.Items;
                object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
                TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
                object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
                Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
                TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
                int startSelectionIndex = inputBox.SelectionStart;
                string inputBoxContent = inputBox.Text;
                int audioContentLength = audioContent.Length;
                inputBoxContent = inputBoxContent.Insert(startSelectionIndex, audioContent);
                inputBox.Text = inputBoxContent;
                inputBox.SelectionStart = startSelectionIndex + audioContentLength;
            }
        }

        public void ChangeSelectionToLowwerCaseHandler (object sender, RoutedEventArgs e)
        {
            ChangeSelectionToLowerCase();
        }

        public void ChangeSelectionToLowerCase ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxContent = inputBox.Text;
            int inputBoxContentLength = inputBox.SelectionLength;
            int startSelectionIndex = inputBox.SelectionStart;
            for (int i = startSelectionIndex; i <= startSelectionIndex + inputBoxContentLength; i++)
            {
                char inputBoxContentChar = inputBoxContent[i];
                string inputBoxContentCharStroke = inputBoxContentChar.ToString();
                string upperInputBoxContentCharStroke = inputBoxContentCharStroke.ToLower();
                char[] chars = upperInputBoxContentCharStroke.ToCharArray();
                char updatedInputBoxContentChar = chars[0];
                inputBoxContent = inputBoxContent.Replace(inputBoxContentChar, updatedInputBoxContentChar);
            }
            inputBox.Text = inputBoxContent;
            inputBox.SelectionStart = startSelectionIndex + inputBoxContentLength;
        }

        public void ChangeSelectionToUpperCaseHandler (object sender, RoutedEventArgs e)
        {
            ChangeSelectionToUpperCase();
        }

        public void ChangeSelectionToUpperCase ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxContent = inputBox.Text;
            int inputBoxContentLength = inputBox.SelectionLength;
            int startSelectionIndex = inputBox.SelectionStart;
            for (int i = startSelectionIndex; i <= startSelectionIndex + inputBoxContentLength; i++)
            {
                char inputBoxContentChar = inputBoxContent[i];
                string inputBoxContentCharStroke = inputBoxContentChar.ToString();
                string upperInputBoxContentCharStroke = inputBoxContentCharStroke.ToUpper();
                char[] chars = upperInputBoxContentCharStroke.ToCharArray();
                char updatedInputBoxContentChar = chars[0];
                inputBoxContent = inputBoxContent.Replace(inputBoxContentChar, updatedInputBoxContentChar);
            }
            inputBox.Text = inputBoxContent;
            inputBox.SelectionStart = startSelectionIndex + inputBoxContentLength;
        }

        public void ChangeSelectionToggleCaseHandler (object sender, RoutedEventArgs e)
        {
            ChangeSelectionToggleCase();
        }

        public void ChangeSelectionToggleCase ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxContent = inputBox.Text;
            int inputBoxContentLength = inputBox.SelectionLength;
            int startSelectionIndex = inputBox.SelectionStart;
            for (int i = startSelectionIndex; i <= startSelectionIndex + inputBoxContentLength; i++)
            {
                char inputBoxContentChar = inputBoxContent[i];
                string inputBoxContentCharStroke = inputBoxContentChar.ToString();
                bool isUpper = Char.IsUpper(inputBoxContentChar);
                string upperInputBoxContentCharStroke = inputBoxContentCharStroke;
                if (isUpper)
                {
                    upperInputBoxContentCharStroke = inputBoxContentCharStroke.ToUpper();
                }
                else
                {
                    upperInputBoxContentCharStroke = inputBoxContentCharStroke.ToUpper();
                }
                char[] chars = upperInputBoxContentCharStroke.ToCharArray();
                char updatedInputBoxContentChar = chars[0];
                inputBoxContent = inputBoxContent.Replace(inputBoxContentChar, updatedInputBoxContentChar);
            }
            inputBox.Text = inputBoxContent;
            inputBox.SelectionStart = startSelectionIndex + inputBoxContentLength;
        }

        public void ChangeSelectionWordsToUpperCaseHandler (object sender, RoutedEventArgs e)
        {
            ChangeSelectionWordsToUpperCase();
        }

        public void ChangeSelectionWordsToUpperCase ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxContent = inputBox.Text;
            int inputBoxContentLength = inputBox.SelectionLength;
            int startSelectionIndex = inputBox.SelectionStart;
            for (int i = startSelectionIndex; i <= startSelectionIndex + inputBoxContentLength; i++)
            {
                char inputBoxContentChar = inputBoxContent[i];
                string inputBoxContentCharStroke = inputBoxContentChar.ToString();
                string upperInputBoxContentCharStroke = inputBoxContentCharStroke.ToUpper();
                char[] chars = upperInputBoxContentCharStroke.ToCharArray();
                char updatedInputBoxContentChar = chars[0];
                inputBoxContent = inputBoxContent.Replace(inputBoxContentChar, updatedInputBoxContentChar);
            }
            inputBox.Text = inputBoxContent;
            inputBox.SelectionStart = startSelectionIndex + inputBoxContentLength;
        }

        public void ChangeSelectionSentensesToUpperCaseHandler (object sender, RoutedEventArgs e)
        {
            ChangeSelectionSentensesToUpperCase();
        }

        public void ChangeSelectionSentensesToUpperCase ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxContent = inputBox.Text;
            int inputBoxContentLength = inputBox.SelectionLength;
            int startSelectionIndex = inputBox.SelectionStart;
            for (int i = startSelectionIndex; i <= startSelectionIndex + inputBoxContentLength; i++)
            {
                char inputBoxContentChar = inputBoxContent[i];
                string inputBoxContentCharStroke = inputBoxContentChar.ToString();
                string upperInputBoxContentCharStroke = inputBoxContentCharStroke.ToUpper();
                char[] chars = upperInputBoxContentCharStroke.ToCharArray();
                char updatedInputBoxContentChar = chars[0];
                inputBoxContent = inputBoxContent.Replace(inputBoxContentChar, updatedInputBoxContentChar);
            }
            inputBox.Text = inputBoxContent;
            inputBox.SelectionStart = startSelectionIndex + inputBoxContentLength;
        }

        public void ChangeSelectionLinesToUpperCaseHandler (object sender, RoutedEventArgs e)
        {
            ChangeSelectionLinesToUpperCase();
        }

        public void ChangeSelectionLinesToUpperCase ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxContent = inputBox.Text;
            int inputBoxContentLength = inputBox.SelectionLength;
            int startSelectionIndex = inputBox.SelectionStart;
            int startLineIndex = inputBox.GetLineIndexFromCharacterIndex(startSelectionIndex);
            for (int i = startSelectionIndex; i <= startSelectionIndex + inputBoxContentLength; i++)
            {
                int lineIndex = inputBox.GetLineIndexFromCharacterIndex(i);
                bool isNotFirstLine = startLineIndex != lineIndex;
                int charIndex = 0;
                if (isNotFirstLine)
                {
                    charIndex = inputBox.GetCharacterIndexFromLineIndex(lineIndex);
                    char inputBoxContentChar = inputBoxContent[charIndex];
                    string inputBoxContentCharStroke = inputBoxContentChar.ToString();
                    string upperInputBoxContentCharStroke = inputBoxContentCharStroke.ToUpper();
                    char[] chars = upperInputBoxContentCharStroke.ToCharArray();
                    char updatedInputBoxContentChar = chars[0];
                    inputBoxContent = inputBoxContent.Replace(inputBoxContentChar, updatedInputBoxContentChar);
                }
                else if (i == startSelectionIndex)
                {

                }
            }
            inputBox.Text = inputBoxContent;
            inputBox.SelectionStart = startSelectionIndex + inputBoxContentLength;
        }

        public void RefreshChangeSelectionMenuItemHandler (object sender, RoutedEventArgs e)
        {
            RefreshChangeSelectionMenuItem();
        }

        public void RefreshChangeSelectionMenuItem ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            int selectionLength = inputBox.SelectionLength;
            bool isHaveSelection = selectionLength >= 1;
            changeSelectionMenuItem.IsEnabled = isHaveSelection;
        }

        public void InsertFastBookmarkHandler (object sender, RoutedEventArgs e)
        {
            InsertFastBookmark();
        }

        public void InsertFastBookmark ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            bookmarkIndex = inputBox.SelectionStart;
            goToFastBookmarkMenuItem.IsEnabled = true;
        }

        public void GoToFastBookmarkHandler (object sender, RoutedEventArgs e)
        {
            GoToFastBookmark();
        }

        public void GoToFastBookmark ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            inputBox.SelectionStart = bookmarkIndex;
        }

        public void OpenZoomHandler (object sender, RoutedEventArgs e)
        {
            OpenZoom();
        }

        public void OpenZoom ()
        {
            Dialogs.ZoomDialog dialog = new Dialogs.ZoomDialog();
            dialog.Show();
        }

        public void ReplaceHandler (object sender, RoutedEventArgs e)
        {
            Replace();
        }

        public void Replace ()
        {
            Dialogs.ReplaceDialog dialog = new Dialogs.ReplaceDialog();
            dialog.Show();
        }

        public void FindHandler (object sender, RoutedEventArgs e)
        {
            Find();
        }

        public void Find ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlItem = ((TabItem)(rawOpenedDocControlItem));
            object rawOpenedDocControlItemContent = openedDocControlItem.Content;
            Controls.OpenedDocControl openedDocControlItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlItemContent));
            TextBox inputBox = openedDocControlItemContent.inputBox;
            string inputBoxContent = inputBox.Text;
            Dialogs.FindDialog dialog = new Dialogs.FindDialog(inputBox);
            dialog.Show();
        }

        public void FindNextHandler (object sender, RoutedEventArgs e)
        {
            FindNext ();
        }

        public void FindNext ()
        {

        }

        public void SetDefaultSpeedHandler (object sender, RoutedEventArgs e)
        {
            SetDefaultSpeed();
        }

        public void SetDefaultSpeed ()
        {
            double speedSliderValue = 0;
            double roundedSpeedSliderValue = ((int)(speedSliderValue));
            string rawRoundedSpeedSliderValue = roundedSpeedSliderValue.ToString();
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            openedDocControlSelectedItemContent.speedValueLabel.Text = rawRoundedSpeedSliderValue;
            openedDocControlSelectedItemContent.speedSlider.Value = speedSliderValue;
        }

        public void ToggleVoiceParamsHandler (object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            ToggleVoiceParams(menuItem);
        }

        public void ToggleVoiceParams (MenuItem menuItem)
        {
            bool IsChecked = menuItem.IsChecked;
            Visibility visibility = Visibility.Visible;
            if (IsChecked)
            {
                visibility = Visibility.Visible;
            }
            else
            {
                visibility = Visibility.Collapsed;
            }
            ItemCollection openedDocControlItems = openedDocControl.Items;
            foreach (TabItem openedDocControlItem in openedDocControlItems)
            {
                object rawOpenedDocControlItemContent = openedDocControlItem.Content;
                Controls.OpenedDocControl openedDocControlItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlItemContent));
                openedDocControlItemContent.voiceParams.Visibility = visibility;
            }
        }

        public void OpenBtnsHandler (object sender, RoutedEventArgs e)
        {
            OpenBtns();
        }

        public void OpenBtns ()
        {
            Dialogs.BtnsDialog dialog = new Dialogs.BtnsDialog(this);
            dialog.Show();
        }

        public void ToggleToolBarHandler (object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            ToggleToolBar(menuItem);
        }

        public void ToggleToolBar (MenuItem menuItem)
        {
            bool IsChecked = menuItem.IsChecked;
            Visibility visibility = Visibility.Visible;
            if (IsChecked)
            {
                visibility = Visibility.Visible;
            }
            else
            {
                visibility = Visibility.Collapsed;
            }
            toolBar.Visibility = visibility;
        }

        public void ToggleStatusBarHandler(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            ToggleStatusBar(menuItem);
        }

        public void ToggleStatusBar(MenuItem menuItem)
        {
            bool IsChecked = menuItem.IsChecked;
            Visibility visibility = Visibility.Visible;
            if (IsChecked)
            {
                visibility = Visibility.Visible;
            }
            else
            {
                visibility = Visibility.Collapsed;
            }
            statusBar.Visibility = visibility;
        }

        public void ToggleLangHandler (object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            object menuItemData = menuItem.DataContext;
            string lang = ((string)(menuItemData));
            ToggleLang(lang);
        }

        public void ToggleLang (string lang)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
        }

        public void OpenTimerHandler (object sender, RoutedEventArgs e)
        {
            OpenTimer();
        }

        public void OpenTimer ()
        {
            Dialogs.TimerDialog dialog = new Dialogs.TimerDialog(this);
            dialog.Show();
        }

        public void StartTimer (int minutes, bool isPlaySound, bool isShowAttention)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(minutes);
            timer.Start();
            timer.Tick += delegate
            {
                if (isPlaySound)
                {
                    mainAudio.Source = new Uri(@"C:\wpf_projects\TTS\TTS\Sounds\notification.wav");
                    mainAudio.Play();
                }
                if (isShowAttention)
                {

                }
            };
        }

        public void DoTimerActionHandler (object sender, EventArgs e)
        {
            DispatcherTimer timer = ((DispatcherTimer)(sender));
            DoTimerAction(timer);
        }

        public void DoTimerAction (DispatcherTimer timer)
        {
            Cancel();
            timer.Stop();
        }

        public void StartSpeechTimer(bool isPlaySound, bool isShowAttention)
        {
            Dictionary<String, Object> updatedSpeechTimerData = new Dictionary<String, Object>();
            updatedSpeechTimerData.Add("isEnabled", true);
            updatedSpeechTimerData.Add("isPlaySound", isPlaySound);
            updatedSpeechTimerData.Add("isShowAttention", isShowAttention);
            updatedSpeechTimerData.Add("action", "quit");
            speechTimerData = updatedSpeechTimerData;
        }

        public void InsertNameBookmarkHandler (object sender, RoutedEventArgs e)
        {
            InsertNameBookmark();
        }

        public void InsertNameBookmark ()
        {
            Dialogs.CreateBookmarkDialog dialog = new Dialogs.CreateBookmarkDialog(this);
            dialog.Show();
        }

        public void GoToBookmarkHandler(object sender, RoutedEventArgs e)
        {
            GoToBookmark();
        }

        public void GoToBookmark()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> currentBookmarks = loadedContent.bookmarks;
            int currentBookmarksCount = currentBookmarks.Count;
            bool isHaveBookmarks = currentBookmarksCount >= 1;
            if (isHaveBookmarks)
            {
                Dialogs.GoToBookmarkDialog dialog = new Dialogs.GoToBookmarkDialog(this);
                dialog.Show();
            }
            else
            {
                MessageBox.Show("Текущий документ не содержит закладок", "Информация");
            }
        }

        public void SplitFileHandler (object sender, RoutedEventArgs e)
        {
            SplitFile();
        }

        public void SplitFile ()
        {
            Dialogs.SplitFileDialog dialog = new Dialogs.SplitFileDialog();
            dialog.Show();
        }

        public void ConvertToAudioHandler (object sender, RoutedEventArgs e)
        {
            ConvertToAudio();
        }

        public void ConvertToAudio()
        {
            Dialogs.ConvertToAudioDialog dialog = new Dialogs.ConvertToAudioDialog();
            dialog.Show();
        }

        public void OpenTextImportHandler (object sender, RoutedEventArgs e)
        {
            OpenTextImport();
        }

        public void OpenTextImport ()
        {
            Dialogs.TextImportDialog dialog = new Dialogs.TextImportDialog();
            dialog.Show();
        }

        public void OpenCompareFilesHandler (object sender, RoutedEventArgs e)
        {
            OpenCompareFiles();
        }

        public void OpenCompareFiles ()
        {
            Dialogs.CompareFilesDialog dialog = new Dialogs.CompareFilesDialog();
            dialog.Show();
        }

        public void ToggleDictBarHandler (object sender, RoutedEventArgs e)
        {
            ToggleDictBar();
        }

        public void ToggleDictBar ()
        {
            Visibility dictBarVisibility = dictBar.Visibility;
            Visibility visible = Visibility.Visible;
            Visibility invisible = Visibility.Collapsed;
            bool isVisible = dictBarVisibility == visible;
            if (isVisible)
            {
                dictBar.Visibility = invisible;
                dictBarMenuItem.IsChecked = false;
            }
            else
            {
                dictBar.Visibility = visible;
                dictBarMenuItem.IsChecked = true;
                GetDicts();
            }
        }

        public void GetDictsHandler (object sender, EventArgs e)
        {
            GetDicts();
        }

        public void OpenSpellCheckHandler(object sender, RoutedEventArgs e)
        {
            OpenSpellCheck();
        }

        public void OpenSpellCheck ()
        {
            Dialogs.SpellCheckDialog dialog = new Dialogs.SpellCheckDialog(this);
            dialog.Show();
        }

        public void OpenTextRepeatHandler(object sender, RoutedEventArgs e)
        {
            OpenTextRepeat();
        }

        public void OpenTextRepeat ()
        {
            Dialogs.RepeatTextDialog dialog = new Dialogs.RepeatTextDialog(this);
            dialog.Show();
        }

        public void SaveMultipleAudioHandler (object sender, RoutedEventArgs e)
        {
            SaveMultipleAudio();
        }

        public void SaveMultipleAudio ()
        {
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxContent = inputBox.Text;
            int inputBoxContentLength = inputBoxContent.Length;
            bool isHaveData = inputBoxContentLength >= 1;
            if (isHaveData)
            {
                Dialogs.SaveMultipleAudioDialog dialog = new Dialogs.SaveMultipleAudioDialog(this);
                dialog.Show();
            }
            else
            {
                MessageBox.Show("Отсутствуют данные для записи в файл", "Информация");
            }
        }

        public void OpenSiteHandler(object sender, RoutedEventArgs e)
        {
            OpenSite();
        }

        public void OpenSite()
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "http://transland.herokuapp.com",
                UseShellExecute = true
            });
        }

        public void OpenAboutHandler (object sender, RoutedEventArgs e)
        {
            OpenAbout();
        }

        public void OpenAbout ()
        {
            Dialogs.AboutDialog dialog = new Dialogs.AboutDialog();
            dialog.Show();
        }

        public void OpenSkinsHandler (object sender, RoutedEventArgs e)
        {
            OpenSkins();
        }

        public void OpenSkins ()
        {
            Dialogs.SkinsDialog dialog = new Dialogs.SkinsDialog(this);
            dialog.Show();
        }

        public void OpenSettingsHandler (object sender, RoutedEventArgs e)
        {
            OpenSettings();
        }

        public void OpenSettings ()
        {
            Dialogs.SettingsDialog dialog = new Dialogs.SettingsDialog();
            dialog.Closed += RefreshSettingsHandler;
            dialog.Show();
        }

        public void RefreshSettingsHandler (object sender, EventArgs e)
        {
            RefreshSettings();
        }

        public void RefreshSettings ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            BufferSettings bufferSettings = currentSettings.buffer;
            bool isDetectBufferEnabled = bufferSettings.isEnabled;
            detectBufferMenuItem.IsChecked = isDetectBufferEnabled;
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            UIElementCollection openedDocsChildren = openedDocs.Children;
            UIElement rawOpenedDoc = openedDocsChildren[openedDocControlSelectedIndex];
            Button openedDoc = ((Button)(rawOpenedDoc));
            SelectOpenedDoc(openedDoc);
            InitTray();
        }

        public void FormatTextHandler (object sender, RoutedEventArgs e)
        {
            FormatText();
        }

        public void FormatText ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            TextSettings textSettings = currentSettings.text;
            bool isRemoveExcessSpaces = textSettings.isRemoveExcessSpaces;
            bool isRemoveNewLineChars = textSettings.isRemoveNewLineChars;
            bool isRemoveAllEmptyLines = textSettings.isRemoveAllEmptyLines;
            bool isReplaceManyEmptyLinesToEmptyLine = textSettings.isReplaceManyEmptyLinesToEmptyLine;
            bool isRemoveSpacesBeforeSemicolon = textSettings.isRemoveSpacesBeforeSemicolon;
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            int lineCount = inputBox.LineCount;
            string inputBoxContent = "";
            for (int i = 0; i < lineCount; i++)
            {
                string lineText = inputBox.GetLineText(i);
                if (isRemoveNewLineChars)
                {
                    lineText = lineText.TrimEnd();
                }
                if (isRemoveAllEmptyLines)
                {
                    int lineTextLength = lineText.Length;
                    bool isEmpty = Environment.NewLine == lineText || String.Empty == lineText || lineTextLength <= 0;
                    if (isEmpty)
                    {
                        lineText = "";
                    }
                }
                inputBoxContent += lineText;
            }
            inputBox.Text = inputBoxContent;
            inputBoxContent = "";
            int charIndex = -1;
            foreach (char inputBoxContentItem in inputBox.Text)
            {
                charIndex++;
                if (isRemoveExcessSpaces)
                {
                    bool isSpace = Char.IsWhiteSpace(inputBoxContentItem);
                    if (isSpace)
                    {
                        bool isNotFirstChar = charIndex >= 1;
                        if (isNotFirstChar)
                        {
                            int previousCharIndex = charIndex - 1;
                            char previousChar = inputBox.Text[previousCharIndex];
                            bool isSecondSpace = Char.IsWhiteSpace(previousChar);
                            if (isSecondSpace)
                            {
                            }
                            else
                            {
                                inputBoxContent += inputBoxContentItem;
                            }
                        }
                        else
                        {
                            inputBoxContent += inputBoxContentItem;
                        }
                    }
                    else
                    {
                        inputBoxContent += inputBoxContentItem;
                    }
                }
                else
                {
                    inputBoxContent += inputBoxContentItem;
                }
            }

            lineCount = inputBox.LineCount;
            inputBoxContent = "";
            for (int i = 0; i < lineCount; i++)
            {
                string lineText = inputBox.GetLineText(i);
                if (isReplaceManyEmptyLinesToEmptyLine)
                {
                    int lineTextLength = lineText.Length;
                    bool isEmpty = Environment.NewLine == lineText || String.Empty == lineText || lineTextLength <= 0;
                    if (isEmpty)
                    {
                        bool isNotFirstLine = i >= 1;
                        if (isNotFirstLine)
                        {
                            int previousLineIndex = i - 1;
                            string previousLineText = inputBox.GetLineText(previousLineIndex);
                            int previousLineTextLength = previousLineText.Length;
                            bool isPreviousLineEmpty = Environment.NewLine == previousLineText || String.Empty == previousLineText || previousLineTextLength <= 0;
                            if (isPreviousLineEmpty)
                            {
                                lineText = "";
                            }
                        }
                    }
                }
                inputBoxContent += lineText;
            }
            inputBox.Text = inputBoxContent;

            inputBoxContent = "";
            charIndex = -1;
            foreach (char inputBoxContentItem in inputBox.Text)
            {
                charIndex++;
                if (isRemoveSpacesBeforeSemicolon)
                {
                    bool isSpace = Char.IsWhiteSpace(inputBoxContentItem);
                    if (isSpace)
                    {
                        bool isSpaceBeforeSemicolon = false;
                        string someLine = inputBox.Text.Substring(charIndex, inputBox.Text.Length - charIndex);
                        int semicolonIndex = someLine.IndexOf(',');
                        isSpaceBeforeSemicolon = semicolonIndex >= 0;
                        string tempBoxContent = inputBox.Text;
                        int tempBoxContentLength = tempBoxContent.Length;
                        int leftLength = tempBoxContentLength - charIndex;
                        string someChars = tempBoxContent.Substring(charIndex, leftLength);
                        foreach (char someChar in someChars)
                        {
                            bool isLocalSpace = Char.IsWhiteSpace(someChar);
                            bool isLocalSemicolon = Char.IsPunctuation(someChar);
                            if (isLocalSpace)
                            {
                                continue;
                            }
                            else if (isLocalSemicolon)
                            {
                                isSpaceBeforeSemicolon = true;
                                break;
                            }
                            else
                            {
                                isSpaceBeforeSemicolon = false;
                                break;
                            }
                        }
                        if (isSpaceBeforeSemicolon)
                        {

                        }
                        else
                        {
                            inputBoxContent += inputBoxContentItem;
                        }
                    }
                    else
                    {
                        inputBoxContent += inputBoxContentItem;
                    }
                }
                else
                {
                    inputBoxContent += inputBoxContentItem;
                }
            }
            inputBox.Text = inputBoxContent;

        }

        public void SpeakInput (string content)
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            GeneralSettings generalSettings = currentSettings.general;
            string beginReadSpeakWith = generalSettings.beginReadSpeakWith;
            bool isCursorPosition = beginReadSpeakWith == "cursorPosition";
            bool isTextStart = beginReadSpeakWith == "textStart";
            bool isParagraphStart = beginReadSpeakWith == "paragraphStart";
            int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            Slider speedSlider = openedDocControlSelectedItemContent.speedSlider;
            double speedSliderValue = speedSlider.Value;
            int roundedSpeedSliderValue = ((int)(speedSliderValue));
            speechSynthesizer.Rate = roundedSpeedSliderValue;
            Slider pitchSlider = openedDocControlSelectedItemContent.pitchSlider;
            double pitchSliderValue = pitchSlider.Value;
            int roundedPitchSliderValue = ((int)(pitchSliderValue));
            Slider volumeSlider = openedDocControlSelectedItemContent.volumeSlider;
            double volumeSliderValue = volumeSlider.Value;
            int roundedVolumeSliderValue = ((int)(volumeSliderValue));
            speechSynthesizer.Volume = roundedVolumeSliderValue;
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxContent = content;
            /*if (isCursorPosition)
            {
                int charIndex = inputBox.SelectionStart;
                int inputBoxContentLength = inputBoxContent.Length;
                int leftLength = inputBoxContentLength - charIndex;
                inputBoxContent = content.Substring(charIndex, leftLength);
            }
            else if (isTextStart)
            {
                inputBoxContent = inputBox.Text;
            }
            else if (isParagraphStart)
            {
                inputBoxContent = inputBox.Text;
            }*/
            ResetPause();
            PromptBuilder builder = new PromptBuilder();
            builder.Culture = CultureInfo.CreateSpecificCulture("ru-RU");
            builder.StartVoice(builder.Culture);
            builder.StartSentence();
            PromptEmphasis emphasis = PromptEmphasis.Moderate;
            bool isStrong = roundedPitchSliderValue < 0;
            bool isMiddle = roundedPitchSliderValue == 0;
            bool isHigh = roundedPitchSliderValue > 0;
            string pitch = "default";
            if (isStrong)
            {
                emphasis = PromptEmphasis.Strong;
                pitch = "-50st";
            }
            else if (isMiddle)
            {
                emphasis = PromptEmphasis.Moderate;
                pitch = "default";
            }
            else if (isHigh)
            {
                emphasis = PromptEmphasis.Reduced;
                pitch = "50st";
            }
            builder.StartStyle(new PromptStyle() { Emphasis = emphasis });
            
            // builder.AppendText(inputBoxContent);
            builder.AppendSsmlMarkup("<prosody pitch=\"" + pitch + "\">" + inputBoxContent + "</prosody>");

            builder.EndStyle();
            builder.EndSentence();
            builder.EndVoice();
            speechSynthesizer.SpeakAsync(builder);
            stopSpeechBtn.IsEnabled = true;
        }

        public void InitTray()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            BufferSettings bufferSettings = currentSettings.buffer;
            ViewSettings viewSettings = currentSettings.view;
            bool isAlwaysShowIconInTray = viewSettings.isAlwaysShowIconInTray;
            if (isAlwaysShowIconInTray)
            {
                CreateTrayIcon();
            }
        }

        public void CreateTrayIcon ()
        {
            bool isTrayNotInit = nIcon == null;
            if (isTrayNotInit)
            {
                nIcon = new System.Windows.Forms.NotifyIcon();
                nIcon.Icon = new System.Drawing.Icon(@"C:\wpf_projects\AntiVirus\AntiVirus\Assets\application_icon.ico");
                nIcon.Visible = true;
                string nIconTitle = "Office ware speech reader";
                nIcon.Text = nIconTitle;
                nIcon.MouseClick += delegate
                {
                    this.Show();
                    Application currentApp = Application.Current;
                    WindowCollection currentAppWindows = currentApp.Windows;
                    IEnumerable<Window> myWindows = currentAppWindows.OfType<Window>();
                    List<Window> smallFloatWindows = myWindows.Where<Window>(window =>
                    {
                        bool isSmallFloatWindow = window is Dialogs.SmallFloatDialog;
                        return isSmallFloatWindow;
                    }).ToList<Window>();
                    int smallFloatWindowsCount = smallFloatWindows.Count;
                    bool isHaveSmallFloatWindows = smallFloatWindowsCount >= 1;
                    if (isHaveSmallFloatWindows)
                    {
                        Window smallFloatWindow = smallFloatWindows[0];
                        smallFloatWindow.Close();
                    }
                };
            }
        }

        private void WindowStateChangedHandler (object sender, EventArgs e)
        {
            WindowStateChanged();
        }

        public void WindowStateChanged ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings currentSettings = loadedContent.settings;
            ViewSettings viewSettings = currentSettings.view;
            GeneralSettings generalSettings = currentSettings.general;
            bool isHideAppInTrayWhenMinimize = viewSettings.isHideAppInTrayWhenMinimize;
            bool isShowSmallFloatWindow = generalSettings.isShowSmallFloatWindow;
            switch (this.WindowState)
            {
                case WindowState.Maximized:
                    break;
                case WindowState.Minimized:
                    if (isHideAppInTrayWhenMinimize)
                    {
                        CreateTrayIcon();
                        this.Hide();
                    }
                    if (isShowSmallFloatWindow)
                    {
                        OpenSmallFloatWindow();
                        this.Hide();
                    }
                    break;
                case WindowState.Normal:
                    break;
            }

        }
         
        public void OpenSmallFloatWindow ()
        {
            Dialogs.SmallFloatDialog dialog = new Dialogs.SmallFloatDialog(this);
            dialog.Show();
        }

        public void OpenFolderToExlorerHandler (object sender, RoutedEventArgs e)
        {
            OpenFolderToExlorer();
        }

        public void OpenFolderToExlorer ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string dictsFolder = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\dicts";
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = dictsFolder,
                UseShellExecute = true
            });
        }

        public void ShowFolderNameHandler(object sender, RoutedEventArgs e)
        {
            ShowFolderName();
        }

        public void ShowFolderName ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string dictsFolder = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\dicts";
            string msgBoxContent = "Папка:" + Environment.NewLine + dictsFolder;
            MessageBox.Show(msgBoxContent, "Информация");
        }

        public void CreateDictHandler(object sender, RoutedEventArgs e)
        {
            CreateDict();
        }

        public void CreateDict()
        {
            Dialogs.CreateDictDialog dialog = new Dialogs.CreateDictDialog();
            dialog.Closed += GetDictsHandler;
            dialog.Show();
        }

        public void RenameDictHandler(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            object menuItemBoxData = menuItem.DataContext;
            string dictName = ((string)(menuItemBoxData));
            RenameDict(dictName);
        }

        public void RenameDict(string dictName)
        {
            Dialogs.RenameDiactDialog dialog = new Dialogs.RenameDiactDialog(dictName);
            dialog.Closed += GetDictsHandler;
            dialog.Show();
        }

        public void EditDictHandler (object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            object menuItemBoxData = menuItem.DataContext;
            string dictName = ((string)(menuItemBoxData));
            EditDict(dictName);
        }

        public void EditDict (string dictName)
        {
            Dialogs.EditDictDialog dialog = new Dialogs.EditDictDialog(dictName);
            dialog.Show();
        }

        public void EditDictInNotepadHandler (object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            object menuItemBoxData = menuItem.DataContext;
            string dictName = ((string)(menuItemBoxData));
            EditDictInNotepad(dictName);
        }

        public void EditDictInNotepad (string dictName)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "notepad",
                Arguments = dictName,
                UseShellExecute = true
            });
        }

        public void RemoveDictHandler(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ((MenuItem)(sender));
            object menuItemBoxData = menuItem.DataContext;
            string dictName = ((string)(menuItemBoxData));
            RemoveDict(dictName);
        }

        public void RemoveDict (string dictName)
        {
            File.Delete(dictName);
            GetDicts();

        }

        public void MarkAllDictsHandler(object sender, RoutedEventArgs e)
        {
            MarkAllDicts();
        }

        public void MarkAllDicts()
        {
            foreach (StackPanel dict in dicts.Children)
            {
                UIElementCollection dictChildren = dict.Children;
                UIElement rawDictCheckBox = dictChildren[0];
                CheckBox dictCheckBox = ((CheckBox)(rawDictCheckBox));
                dictCheckBox.IsChecked = true;
            }
        }

        public void ClearAllDictsHandler (object sender, RoutedEventArgs e)
        {
            ClearAllDicts();
        }

        public void ClearAllDicts ()
        {
            foreach (StackPanel dict in dicts.Children)
            {
                UIElementCollection dictChildren = dict.Children;
                UIElement rawDictCheckBox = dictChildren[0];
                CheckBox dictCheckBox = ((CheckBox)(rawDictCheckBox));
                dictCheckBox.IsChecked = false;
            }
        }

        private void GlobalHotKeyHandler (object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            GlobalHotKey(key);
        }

        public void GlobalHotKey (Key key)
        {

            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<HotKey> hotKeys = loadedContent.hotKeys;
            bool isNeedShiftModifier = false;
            bool isNeedCtrlModifier = false;
            HotKey createDocHotKey = hotKeys[0];
            string createDocHotKeyShortCut = createDocHotKey.shortcut;
            bool isModifiersApplied = createDocHotKeyShortCut.Contains("+");
            if (isModifiersApplied)
            {
                string[] hotKeyParts = createDocHotKeyShortCut.Split(new Char[] { '+' });
                string modifier = hotKeyParts[0];
                modifier = modifier.Trim();
                isNeedShiftModifier = modifier == "Shift";
                isNeedCtrlModifier = modifier == "Ctrl";
                createDocHotKeyShortCut = hotKeyParts[1];
                createDocHotKeyShortCut = createDocHotKeyShortCut.Trim();
            }
            else
            {
                createDocHotKeyShortCut = key.ToString();
            }
            var shiftModifier = Keyboard.Modifiers & ModifierKeys.Shift;
            bool isShiftModifierEnabled = shiftModifier > 0;
            var ctrlModifier = Keyboard.Modifiers & ModifierKeys.Control;
            bool isCtrlModifierEnabled = ctrlModifier > 0;
            bool isCreateDocKey = key.ToString() == createDocHotKeyShortCut;
            bool isCreateDoc = isCreateDocKey && ((isShiftModifierEnabled && isNeedShiftModifier) || !isNeedShiftModifier) && ((isCtrlModifierEnabled && isNeedCtrlModifier) || !isNeedCtrlModifier);

            HotKey openDocHotKey = hotKeys[1];
            string openDocHotKeyShortCut = openDocHotKey.shortcut;
            isModifiersApplied = openDocHotKeyShortCut.Contains("+");
            if (isModifiersApplied)
            {
                string[] hotKeyParts = openDocHotKeyShortCut.Split(new Char[] { '+' });
                string modifier = hotKeyParts[0];
                modifier = modifier.Trim();
                isNeedShiftModifier = modifier == "Shift";
                isNeedCtrlModifier = modifier == "Ctrl";
                openDocHotKeyShortCut = hotKeyParts[1];
                openDocHotKeyShortCut = openDocHotKeyShortCut.Trim();
            }
            else
            {
                openDocHotKeyShortCut = key.ToString();
            }
            bool isOpenDocKey = key.ToString() == openDocHotKeyShortCut;
            bool isOpenDoc = isOpenDocKey && ((isShiftModifierEnabled && isNeedShiftModifier) || !isNeedShiftModifier) && ((isCtrlModifierEnabled && isNeedCtrlModifier) || !isNeedCtrlModifier);
            if (isCreateDoc)
            {
                CreateDoc();
            }
            else if (isOpenDoc)
            {
                OpenDoc();
            }
        }

    }

    class SavedContent
    {
        public List<Dictionary<String, Object>> bookmarks;
        public Settings settings;
        public List<DictProfile> dictProfiles;
        public List<HotKey> hotKeys;
    }

    public class Settings
    {
        public BufferSettings buffer;
        public TextSettings text;
        public GeneralSettings general;
        public ViewSettings view;
    }

    public class BufferSettings
    {
        public bool isEnabled;
        public string action;
        public bool ignoreTextInSoftware;
        public bool showAlertTextOperationMsgs;
        public bool ignoreCopiedTextInBufferIfTextNotChanged;
        public int minCountCharsInCopiedText;
    }

    public class TextSettings
    {
        public bool isOpen;
        public bool isRemoveExcessSpaces;
        public bool isRemoveNewLineChars;
        public bool isRemoveAllEmptyLines;
        public bool isReplaceManyEmptyLinesToEmptyLine;
        public bool isRemoveSpacesBeforeSemicolon;
        public bool isNotOfferSaveChangedText;
    }

    public class GeneralSettings
    {
        public string beginReadSpeakWith;
        public string beginWriteToAudioFileWith;
        public bool isLetters;
        public bool isWords;
        public bool isParagraphs;
        public string startupAction;
        public bool isShowSmallFloatWindow;
        public bool isTransparentSmallFloatWindow;
    }

    public class ViewSettings
    {
        public bool isShowIcons;
        public bool isShowFullPathToDoc;
        public bool isShowPercentOfWorkInTaskBar;
        public bool isHideAppInTrayWhenMinimize;
        public bool isAlwaysShowIconInTray;
    }

    public class DictProfile
    {
        public string name;
        public List<DictProfileItem> items;
    }

    public class DictProfileItem
    {
        public string path;
        public bool isChecked;
        public bool isDefault;
    }

    public class HotKey
    {
        public string cmd;
        public string shortcut;
    }

}
