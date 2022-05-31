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
    /// Логика взаимодействия для SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();

            Init();
        
        }


        public void Init ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            Settings updatedSettings = loadedContent.settings;
            bool isBufferEnabled = updatedSettings.buffer.isEnabled;
            detectBufferCheckBox.IsChecked = isBufferEnabled;
            minCountCharsInCopiedTextCheckBox.IsEnabled = isBufferEnabled;
            minCountCharsInCopiedTextBox.IsEnabled = isBufferEnabled;
            showAlertTextOperationMsgsCheckBox.IsEnabled = isBufferEnabled;
            ignoreTextInSoftwareCheckBox.IsEnabled = isBufferEnabled;
            ignoreCopiedTextInBufferIfTextNotChangedCheckBox.IsEnabled = isBufferEnabled;
            speakRadioBtn.IsEnabled = isBufferEnabled;
            createDocRadioBtn.IsEnabled = isBufferEnabled;
            addTextToCurrentDocRadioBtn.IsEnabled = isBufferEnabled;
            replaceTextToCurrentDocRadioBtn.IsEnabled = isBufferEnabled;
            createDocAndSpeakRadioBtn.IsEnabled = isBufferEnabled;
            addTextToCurrentDocAndSpeakRadioBtn.IsEnabled = isBufferEnabled;
            replaceTextToCurrentDocAndSpeakRadioBtn.IsEnabled = isBufferEnabled;
            if (isBufferEnabled)
            {
                minCountCharsInCopiedTextCheckBox.Foreground = System.Windows.Media.Brushes.Black;
                minCountCharsInCopiedTextBox.Foreground = System.Windows.Media.Brushes.Black;
                minCountCharsInCopiedTextBox.BorderBrush = System.Windows.Media.Brushes.Black;
                showAlertTextOperationMsgsCheckBox.Foreground = System.Windows.Media.Brushes.Black;
                ignoreTextInSoftwareCheckBox.Foreground = System.Windows.Media.Brushes.Black;
                ignoreCopiedTextInBufferIfTextNotChangedCheckBox.Foreground = System.Windows.Media.Brushes.Black;
                speakRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                createDocRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                addTextToCurrentDocRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                replaceTextToCurrentDocRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                createDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                addTextToCurrentDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                replaceTextToCurrentDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
            }
            else
            {
                minCountCharsInCopiedTextCheckBox.Foreground = System.Windows.Media.Brushes.LightGray;
                minCountCharsInCopiedTextBox.Foreground = System.Windows.Media.Brushes.LightGray;
                minCountCharsInCopiedTextBox.BorderBrush = System.Windows.Media.Brushes.LightGray;
                showAlertTextOperationMsgsCheckBox.Foreground = System.Windows.Media.Brushes.LightGray;
                ignoreTextInSoftwareCheckBox.Foreground = System.Windows.Media.Brushes.LightGray;
                ignoreCopiedTextInBufferIfTextNotChangedCheckBox.Foreground = System.Windows.Media.Brushes.LightGray;
                speakRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                createDocRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                addTextToCurrentDocRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                replaceTextToCurrentDocRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                createDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                addTextToCurrentDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                replaceTextToCurrentDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
            }
            string bufferAction = updatedSettings.buffer.action;
            bool IsBufferSpeakActionChecked = bufferAction == "speak";
            bool IsBufferCreateDocActionChecked = bufferAction == "createDoc";
            bool IsBufferAddTextToCurrentDocActionChecked = bufferAction == "addTextToCurrentDoc";
            bool IsBufferReplaceTextToCurrentDocActionChecked = bufferAction == "replaceTextToCurrentDoc";
            bool IsBufferCreateDocAndSpeakActionChecked = bufferAction == "createDocAndSpeak";
            bool IsBufferAddTextToDocAndSpeakActionChecked = bufferAction == "addTextToCurrentDocAndSpeak";
            bool IsBufferReplaceTextToCurrentDocAndSpeakActionChecked = bufferAction == "replaceTextToCurrentDocAndSpeak";
            if (IsBufferSpeakActionChecked)
            {
                speakRadioBtn.IsChecked = true;
            }
            else if (IsBufferCreateDocActionChecked)
            {
                createDocRadioBtn.IsChecked = true;
            }
            else if (IsBufferAddTextToCurrentDocActionChecked)
            {
                addTextToCurrentDocRadioBtn.IsChecked = true;
            }
            else if (IsBufferReplaceTextToCurrentDocActionChecked)
            {
                replaceTextToCurrentDocRadioBtn.IsChecked = true;
            }
            else if (IsBufferCreateDocAndSpeakActionChecked)
            {
                createDocAndSpeakRadioBtn.IsChecked = true;
            }
            else if (IsBufferAddTextToDocAndSpeakActionChecked)
            {
                addTextToCurrentDocAndSpeakRadioBtn.IsChecked = true;
            }
            else if (IsBufferReplaceTextToCurrentDocAndSpeakActionChecked)
            {
                replaceTextToCurrentDocAndSpeakRadioBtn.IsChecked = true;
            }
            bool isIgnoreTextInSoftware = updatedSettings.buffer.ignoreTextInSoftware;
            ignoreTextInSoftwareCheckBox.IsChecked = isIgnoreTextInSoftware;
        }

        public void CancelHandler (object sender, RoutedEventArgs e)
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
            Settings updatedSettings = loadedContent.settings;
            object rawIsChecked = detectBufferCheckBox.IsChecked;
            bool IsChecked = ((bool)(rawIsChecked));
            updatedSettings.buffer.isEnabled = IsChecked;
            string bufferAction = "speak";
            rawIsChecked = speakRadioBtn.IsChecked;
            bool IsBufferSpeakActionChecked = ((bool)(rawIsChecked));
            rawIsChecked = createDocRadioBtn.IsChecked;
            bool IsBufferCreateDocActionChecked = ((bool)(rawIsChecked));
            rawIsChecked = addTextToCurrentDocRadioBtn.IsChecked;
            bool IsBufferAddTextToCurrentDocActionChecked = ((bool)(rawIsChecked));
            rawIsChecked = replaceTextToCurrentDocRadioBtn.IsChecked;
            bool IsBufferReplaceTextToCurrentDocActionChecked = ((bool)(rawIsChecked));
            rawIsChecked = createDocAndSpeakRadioBtn.IsChecked;
            bool IsBufferCreateDocAndSpeakActionChecked = ((bool)(rawIsChecked));
            rawIsChecked = addTextToCurrentDocAndSpeakRadioBtn.IsChecked;
            bool IsBufferAddTextToDocAndSpeakActionChecked = ((bool)(rawIsChecked));
            rawIsChecked = replaceTextToCurrentDocAndSpeakRadioBtn.IsChecked;
            bool IsBufferReplaceTextToCurrentDocAndSpeakActionChecked = ((bool)(rawIsChecked));
            if (IsBufferSpeakActionChecked)
            {
                bufferAction = "speak";
            }
            else if (IsBufferCreateDocActionChecked)
            {
                bufferAction = "createDoc";
            }
            else if (IsBufferAddTextToCurrentDocActionChecked)
            {
                bufferAction = "addTextToCurrentDoc";
            }
            else if (IsBufferReplaceTextToCurrentDocActionChecked)
            {
                bufferAction = "replaceTextToCurrentDoc";
            }
            else if (IsBufferCreateDocAndSpeakActionChecked)
            {
                bufferAction = "createDocAndSpeak";
            }
            else if (IsBufferAddTextToDocAndSpeakActionChecked)
            {
                bufferAction = "addTextToCurrentDocAndSpeak";
            }
            else if (IsBufferReplaceTextToCurrentDocAndSpeakActionChecked)
            {
                bufferAction = "replaceTextToCurrentDocAndSpeak";
            }
            updatedSettings.buffer.action = bufferAction;
            rawIsChecked = ignoreTextInSoftwareCheckBox.IsChecked;
            bool isIgnoreTextInSoftware = ((bool)(rawIsChecked));
            updatedSettings.buffer.ignoreTextInSoftware = isIgnoreTextInSoftware;
            updatedSettings.buffer.ignoreTextInSoftware = isShowNo; 
            string savedContent = js.Serialize(new SavedContent
            {
                bookmarks = currentBookmarks,
                settings = updatedSettings
            });
            File.WriteAllText(saveDataFilePath, savedContent);
            Cancel();
        }

        private void ToggleDetectBufferHandler (object sender, RoutedEventArgs e)
        {
            ToggleDetectBuffer();
        }

        public void ToggleDetectBuffer ()
        {
            object rawIsChecked = detectBufferCheckBox.IsChecked;
            bool isChecked = ((bool)(rawIsChecked));
            minCountCharsInCopiedTextCheckBox.IsEnabled = isChecked;
            minCountCharsInCopiedTextBox.IsEnabled = isChecked;
            showAlertTextOperationMsgsCheckBox.IsEnabled = isChecked;
            ignoreTextInSoftwareCheckBox.IsEnabled = isChecked;
            ignoreCopiedTextInBufferIfTextNotChangedCheckBox.IsEnabled = isChecked;
            speakRadioBtn.IsEnabled = isChecked;
            createDocRadioBtn.IsEnabled = isChecked;
            addTextToCurrentDocRadioBtn.IsEnabled = isChecked;
            replaceTextToCurrentDocRadioBtn.IsEnabled = isChecked;
            createDocAndSpeakRadioBtn.IsEnabled = isChecked;
            addTextToCurrentDocAndSpeakRadioBtn.IsEnabled = isChecked;
            replaceTextToCurrentDocAndSpeakRadioBtn.IsEnabled = isChecked;
            if (isChecked)
            {
                minCountCharsInCopiedTextCheckBox.Foreground = System.Windows.Media.Brushes.Black;
                minCountCharsInCopiedTextBox.Foreground = System.Windows.Media.Brushes.Black;
                minCountCharsInCopiedTextBox.BorderBrush = System.Windows.Media.Brushes.Black;
                showAlertTextOperationMsgsCheckBox.Foreground = System.Windows.Media.Brushes.Black;
                ignoreTextInSoftwareCheckBox.Foreground = System.Windows.Media.Brushes.Black;
                ignoreCopiedTextInBufferIfTextNotChangedCheckBox.Foreground = System.Windows.Media.Brushes.Black;
                speakRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                createDocRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                addTextToCurrentDocRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                replaceTextToCurrentDocRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                createDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                addTextToCurrentDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
                replaceTextToCurrentDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.Black;
            }
            else
            {
                minCountCharsInCopiedTextCheckBox.Foreground = System.Windows.Media.Brushes.LightGray;
                minCountCharsInCopiedTextBox.Foreground = System.Windows.Media.Brushes.LightGray;
                minCountCharsInCopiedTextBox.BorderBrush = System.Windows.Media.Brushes.LightGray;
                showAlertTextOperationMsgsCheckBox.Foreground = System.Windows.Media.Brushes.LightGray;
                ignoreTextInSoftwareCheckBox.Foreground = System.Windows.Media.Brushes.LightGray;
                ignoreCopiedTextInBufferIfTextNotChangedCheckBox.Foreground = System.Windows.Media.Brushes.LightGray;
                speakRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                createDocRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                addTextToCurrentDocRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                replaceTextToCurrentDocRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                createDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                addTextToCurrentDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
                replaceTextToCurrentDocAndSpeakRadioBtn.Foreground = System.Windows.Media.Brushes.LightGray;
            }
        }

    }
}
