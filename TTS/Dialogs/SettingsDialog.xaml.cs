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
