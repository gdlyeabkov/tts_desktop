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
            ToggleSpeedSlider();
        }

        public void ToggleSpeedSlider ()
        {
            if (isAppInit)
            {
                double speedSliderValue = speedSlider.Value;
                double roundedSpeedSliderValue = ((int)(speedSliderValue));
                string rawRoundedSpeedSliderValue = roundedSpeedSliderValue.ToString();
                speedValueLabel.Text = rawRoundedSpeedSliderValue;
            }
        }

        private void TogglePitchSliderHandler(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TogglePitchSlider();
        }

        public void TogglePitchSlider ()
        {
            if (isAppInit)
            {
                double pitchSliderValue = pitchSlider.Value;
                double roundedPitchSliderValue = ((int)(pitchSliderValue));
                string rawRoundedPitchSliderValue = roundedPitchSliderValue.ToString();
                pitchValueLabel.Text = rawRoundedPitchSliderValue;
            }
        }

        private void ToggleVolumeSliderHandler (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ToggleVolumeSlider();
        }

        public void ToggleVolumeSlider ()
        {
            if (isAppInit)
            {
                double volumeSliderValue = volumeSlider.Value;
                double roundedVolumeSliderValue = ((int)(volumeSliderValue));
                string rawRoundedVolumeSliderValue = roundedVolumeSliderValue.ToString();
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
        }

        public void SpeakBufferHandler (object sender, RoutedEventArgs e)
        {
            SpeakBuffer();
        }

        public void SpeakBuffer ()
        {
            double speedSliderValue = speedSlider.Value;
            int roundedSpeedSliderValue = ((int)(speedSliderValue));
            speechSynthesizer.Rate = roundedSpeedSliderValue;
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
            double speedSliderValue = speedSlider.Value;
            int roundedSpeedSliderValue = ((int)(speedSliderValue));
            speechSynthesizer.Rate = roundedSpeedSliderValue;
            double volumeSliderValue = volumeSlider.Value;
            int roundedVolumeSliderValue = ((int)(volumeSliderValue));
            speechSynthesizer.Volume = roundedVolumeSliderValue;
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
            double speedSliderValue = speedSlider.Value;
            int roundedSpeedSliderValue = ((int)(speedSliderValue));
            speechSynthesizer.Rate = roundedSpeedSliderValue;
            /*double pitchSliderValue = pitchSlider.Value;
            int roundedPitchSliderValue = ((int)(pitchSliderValue));
            speechSynthesizer.;*/
            double volumeSliderValue = volumeSlider.Value;
            int roundedVolumeSliderValue = ((int)(volumeSliderValue));
            speechSynthesizer.Volume = roundedVolumeSliderValue;
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
        }

        public void SelectAllInputHandler (object sender, RoutedEventArgs e)
        {
            SelectAllInput();
        }

        public void SelectAllInput ()
        {
            inputBox.SelectAll();
        }

        public void InsertTextHandler (object sender, RoutedEventArgs e)
        {
            InsertText();
        }

        public void InsertText ()
        {
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
            inputBox.FontSize++;
        }

        public void DecreaseFontSizeHandler (object sender, RoutedEventArgs e)
        {
            DecreaseFontSize();
        }

        public void DecreaseFontSize ()
        {
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
            speedSlider.Value++;
        }

        public void DecreaseSpeedHandler (object sender, RoutedEventArgs e)
        {
            DecreaseSpeed();
        }

        public void DecreaseSpeed()
        {
            speedSlider.Value--;
        }

        public void IncreasePitchHandler (object sender, RoutedEventArgs e)
        {
            IncreasePitch();
        }

        public void IncreasePitch ()
        {
            pitchSlider.Value++;
        }

        public void DecreasePitchHandler(object sender, RoutedEventArgs e)
        {
            DecreasePitch();
        }

        public void DecreasePitch ()
        {
            pitchSlider.Value--;
        }

        public void IncreaseVolumeHandler(object sender, RoutedEventArgs e)
        {
            IncreaseVolume();
        }

        public void IncreaseVolume ()
        {
            volumeSlider.Value++;
        }

        public void DecreaseVolumeHandler (object sender, RoutedEventArgs e)
        {
            DecreaseVolume();
        }

        public void DecreaseVolume ()
        {
            volumeSlider.Value--;
        }

        public void GoToNextLineHandler (object sender, RoutedEventArgs e)
        {
            GoToNextLine();
        }

        public void GoToNextLine ()
        {
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

    }
}
