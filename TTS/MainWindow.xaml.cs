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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Speech.Synthesis;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using NAudio.Wave;

namespace TTS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public bool isAppInit = false;
        public SpeechSynthesizer speechSynthesizer = null;

        public MainWindow()
        {
            InitializeComponent();
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

        private void InitHandler (object sender, RoutedEventArgs e)
        {
            Init();
        }

        public void Init ()
        {
            isAppInit = true;
            speechSynthesizer = new SpeechSynthesizer();
            CreateDoc();
            GetVoices();
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
            Slider volumeSlider = openedDocControlSelectedItemContent.volumeSlider;
            double volumeSliderValue = volumeSlider.Value;
            
            int roundedVolumeSliderValue = ((int)(volumeSliderValue));
            speechSynthesizer.Volume = roundedVolumeSliderValue;
            string copiedText = Clipboard.GetText();
            ResetPause();
            speechSynthesizer.SpeakAsync(copiedText);
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
            Slider volumeSlider = openedDocControlSelectedItemContent.volumeSlider;
            double volumeSliderValue = volumeSlider.Value;
            int roundedVolumeSliderValue = ((int)(volumeSliderValue));
            speechSynthesizer.Volume = roundedVolumeSliderValue;
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxSelectedContent = inputBox.SelectedText;
            ResetPause();
            speechSynthesizer.SpeakAsync(inputBoxSelectedContent);
            stopSpeechBtn.IsEnabled = true;
        }

        public void SpeakHandler (object sender, RoutedEventArgs e)
        {
            Speak();
        }

        public void Speak ()
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
            Slider volumeSlider = openedDocControlSelectedItemContent.volumeSlider;
            double volumeSliderValue = volumeSlider.Value;
            int roundedVolumeSliderValue = ((int)(volumeSliderValue));
            speechSynthesizer.Volume = roundedVolumeSliderValue;
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxContent = inputBox.Text;
            ResetPause();
            speechSynthesizer.SpeakAsync(inputBoxContent);
            stopSpeechBtn.IsEnabled = true;
        }

        public void CreateDocHandler (object sender, RoutedEventArgs e)
        {
            CreateDoc();
        }

        public void CreateDoc ()
        {
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
            openedDocContentLabel.Text = "Документ" + rawDocumentNumber;
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
            openedDoc.Click += SelectOpenedDocHandler;
            SelectOpenedDoc(openedDoc);
        }

        public void SelectOpenedDocHandler (object sender, RoutedEventArgs e)
        {
            Button btn = ((Button)(sender));
            SelectOpenedDoc(btn);
        }

        public void SelectOpenedDoc (Button selectedOpenedDoc)
        {
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
            Dialogs.SelectOutputDevieDialog dialog = new Dialogs.SelectOutputDevieDialog();
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
            }
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
                string path = sfd.FileName;
                speechSynthesizer.SetOutputToWaveFile(path);
                // WaveFileWriter.CreateWaveFile(path);
            }
        }

        public void OpenFontAndColorsHandler (object sender, RoutedEventArgs e)
        {
            OpenFontAndColors();
        }

        public void OpenFontAndColors ()
        {
            Dialogs.FontAndColors dialog = new Dialogs.FontAndColors();
            dialog.Show();
        }

    }
}
