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
using System.IO;
using System.Windows.Threading;
using System.Web.Script.Serialization;
using System.Globalization;

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

        public MainWindow()
        {

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");

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
            InitVars();
            CreateDoc();
            GetVoices();
            InitCache();
        }

        public void InitVars ()
        {
            isAppInit = true;
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SpeakCompleted += SpeakCompletedHandler;
            openedDocHistory = new List<string>();
            speechTimerData = new Dictionary<String, Object>();
            speechTimerData.Add("isEnabled", false);
            speechTimerData.Add("isPlaySound", false);
            speechTimerData.Add("isShowAttention", false);
            speechTimerData.Add("action", "quit");
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
                    bookmarks = new List<Dictionary<String, Object>>() { }
                });
                File.WriteAllText(saveDataFilePath, savedContent);
            }

        }

        public void SpeakCompletedHandler (object sender, SpeakCompletedEventArgs e)
        {
            SpeakCompleted();
        }

        public void SpeakCompleted ()
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
            builder.AppendText(copiedText);
            builder.EndStyle();
            builder.EndSentence();
            builder.EndVoice();
            speechSynthesizer.SpeakAsync(builder);
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
            builder.AppendText(inputBoxContent);
            builder.EndStyle();
            builder.EndSentence();
            builder.EndVoice();
            speechSynthesizer.SpeakAsync(builder);
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
                int openedDocControlSelectedIndex = openedDocControl.SelectedIndex;
                ItemCollection openedDocControlItems = openedDocControl.Items;
                object rawOpenedDocControlItem = openedDocControlItems[openedDocControlSelectedIndex];
                TabItem openedDocControlItem = ((TabItem)(rawOpenedDocControlItem));
                object rawOpenedDocControlItemContent = openedDocControlItem.Content;
                Controls.OpenedDocControl openedDocControlItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlItemContent));
                TextBox inputBox = openedDocControlItemContent.inputBox;
                string inputBoxContent = inputBox.Text;
                speechSynthesizer.SpeakAsync(inputBoxContent);
                // WaveFileWriter.CreateWaveFile(path);
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
                string path = ofd.FileName;
                InsertDoc(path);
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
            CreateDoc();
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
        }

        public void SaveDocHandler (object sender, RoutedEventArgs e)
        {
            SaveDoc();
        }

        public void SaveDoc ()
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
            // timer.Tick += DoTimerActionHandler;
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
            }
            else
            {
                dictBar.Visibility = visible;
            }
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
            Dialogs.RepeatTextDialog dialog = new Dialogs.RepeatTextDialog();
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

    }

    class SavedContent
    {
        public List<Dictionary<String, Object>> bookmarks;
    }

}
