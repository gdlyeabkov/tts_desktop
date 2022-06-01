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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TTS.Controls
{
    /// <summary>
    /// Логика взаимодействия для OpenedDocControl.xaml
    /// </summary>
    public partial class OpenedDocControl : UserControl
    {
        public OpenedDocControl()
        {
            InitializeComponent();
        }

        private void TogglePitchSliderHandler(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = ((Slider)(sender));
            TogglePitchSlider(slider);
        }

        public void TogglePitchSlider (Slider slider)
        {
            object controlData = this.DataContext;
            bool isDataExists = controlData != null;
            if (isDataExists)
            {
                MainWindow mainWindow = ((MainWindow)(controlData));
                mainWindow.TogglePitchSlider(slider);
            }
        }

        private void ToggleVolumeSliderHandler (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = ((Slider)(sender));
            ToggleVolumeSlider(slider);
        }

        public void ToggleVolumeSlider (Slider slider)
        {
            object controlData = this.DataContext;
            bool isDataExists = controlData != null;
            if (isDataExists)
            {
                MainWindow mainWindow = ((MainWindow)(controlData));
                mainWindow.ToggleVolumeSlider(slider);
            }
        }

        private void ToggleSpeedSliderHandler (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = ((Slider)(sender));
            ToggleSpeedSlider(slider);
        }

        public void ToggleSpeedSlider(Slider slider)
        {
            object controlData = this.DataContext;
            bool isDataExists = controlData != null;
            if (isDataExists)
            {
                MainWindow mainWindow = ((MainWindow)(controlData));
                mainWindow.ToggleSpeedSlider(slider);
            }
        }

        private void DetectInputHandler (object sender, TextChangedEventArgs e)
        {
            DetectInput();
        }

        public void DetectInput ()
        {
            object controlData = this.DataContext;
            bool isDataExists = controlData != null;
            if (isDataExists)
            {

                Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
                string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
                string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
                JavaScriptSerializer js = new JavaScriptSerializer();
                string saveDataFileContent = File.ReadAllText(saveDataFilePath);
                SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
                Settings currentSettings = loadedContent.settings;
                GeneralSettings generalSettings = currentSettings.general;
                bool isLetters = generalSettings.isLetters;
                bool isWords = generalSettings.isWords;
                bool isParagraphs = generalSettings.isParagraphs;
                if (isLetters)
                {
                    int characterIndex = inputBox.SelectionStart;
                    string inputBoxContent = inputBox.Text;
                    inputBoxContent = inputBoxContent.Substring(characterIndex - 1, 1);
                    MainWindow mainWindow = ((MainWindow)(controlData));
                    mainWindow.SpeakInput(inputBoxContent);
                }
                else if (isWords)
                {
                    int characterIndex = inputBox.SelectionStart;
                    int lineIndex = inputBox.GetLineIndexFromCharacterIndex(characterIndex);
                    string inputBoxContent = inputBox.GetLineText(lineIndex);
                    MainWindow mainWindow = ((MainWindow)(controlData));
                    mainWindow.SpeakInput(inputBoxContent);
                }
                else if (isParagraphs)
                {
                    string inputBoxContent = inputBox.Text;
                    MainWindow mainWindow = ((MainWindow)(controlData));
                    mainWindow.SpeakInput(inputBoxContent);
                }
            }
        }

    }
}
