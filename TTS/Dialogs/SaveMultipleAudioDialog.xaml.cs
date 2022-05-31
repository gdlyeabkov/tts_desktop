using System;
using System.Collections.Generic;
using System.IO;
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

using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace TTS.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SaveMultipleAudioDialog.xaml
    /// </summary>
    public partial class SaveMultipleAudioDialog : Window
    {

        public MainWindow mainWindow;

        public SaveMultipleAudioDialog(MainWindow mainWindow)
        {
            InitializeComponent();

            Init(mainWindow);

        }

        public void Init (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            int openedDocControlSelectedIndex = mainWindow.openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = mainWindow.openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            templateFileBox.Text = inputBox.GetLineText(0);
        }

        public void TestHandler(object sender, RoutedEventArgs e)
        {
            Test();
        }

        public void Test()
        {
            Cancel();
        }

        public void CancelHandler(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel()
        {
            this.Close();
        }

        public void PickSavedFolderHandler(object sender, RoutedEventArgs e)
        {
            PickSavedFolder();
        }

        public void PickSavedFolder()
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult res = fbd.ShowDialog();
            System.Windows.Forms.DialogResult okResult = System.Windows.Forms.DialogResult.OK;
            bool isOpen = res == okResult;
            if (isOpen)
            {
                string path = fbd.SelectedPath;
                saveFolderBox.Text = path;
            }
        }

        public void ToggleUseFileSizeHandler(object sender, RoutedEventArgs e)
        {
            ToggleUseFileSize();
        }

        public void ToggleUseFileSize()
        {
            object rawIsChecked = useFileSizeCheckBox.IsChecked;
            bool isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                useFileSizeLabel.Foreground = System.Windows.Media.Brushes.LightGray;
                useFileSizeBox.Foreground = System.Windows.Media.Brushes.LightGray;
                useFileSizeBox.BorderBrush = System.Windows.Media.Brushes.LightGray;
                useFileSizeMeasureLabel.Foreground = System.Windows.Media.Brushes.LightGray;
            }
            else
            {
                useFileSizeLabel.Foreground = System.Windows.Media.Brushes.Black;
                useFileSizeBox.Foreground = System.Windows.Media.Brushes.Black;
                useFileSizeBox.BorderBrush = System.Windows.Media.Brushes.Black;
                useFileSizeMeasureLabel.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        public void ToggleFindKeywordsHandler(object sender, RoutedEventArgs e)
        {
            ToggleFindKeywords();
        }

        public void ToggleFindKeywords()
        {
            object rawIsChecked = findKeywordsCheckBox.IsChecked;
            bool isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                keywordsLabel.Foreground = System.Windows.Media.Brushes.Black;
                keywordsBox.Foreground = System.Windows.Media.Brushes.Black;
                keywordsBox.BorderBrush = System.Windows.Media.Brushes.Black;
                keywordsDeleteWordCheckBox.Foreground = System.Windows.Media.Brushes.Black;
            }
            else
            {
                keywordsLabel.Foreground = System.Windows.Media.Brushes.LightGray;
                keywordsBox.Foreground = System.Windows.Media.Brushes.LightGray;
                keywordsBox.BorderBrush = System.Windows.Media.Brushes.LightGray;
                keywordsDeleteWordCheckBox.Foreground = System.Windows.Media.Brushes.LightGray;
            }
        }

        public void SplitAndConvertHandler (object sender, RoutedEventArgs e)
        {
            SplitAndConvert();
        }

        public void SplitAndConvert ()
        {
            int openedDocControlSelectedIndex = mainWindow.openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = mainWindow.openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string sourceFileBoxContent = inputBox.Text;
            int sourceFileBoxContentLength = sourceFileBoxContent.Length;
            bool isSourceFileBoxContentLengthExists = sourceFileBoxContentLength >= 1;
            bool isSourceFileBoxContentLengthNotExists = !isSourceFileBoxContentLengthExists;
            string saveFolderBoxContent = saveFolderBox.Text;
            int saveFolderBoxContentLength = saveFolderBoxContent.Length;
            bool isSaveFolderBoxContentLengthExists = saveFolderBoxContentLength >= 1;
            bool isSaveFolderBoxContentLengthNotExists = !isSaveFolderBoxContentLengthExists;
            bool isCanRun = isSourceFileBoxContentLengthExists && isSaveFolderBoxContentLengthExists;
            if (isCanRun)
            {
                string content = sourceFileBoxContent;

                // string fileName = inputBox.GetLineText(0);
                string fileName = templateFileBox.Text;
                
                string fileExt = ".wav";
                object rawIsChecked = addNumberAfterFileNameRadioBtn.IsChecked;
                bool isAddAfter = ((bool)(rawIsChecked));
                string startNumberFileNameBoxContent = startNumberFileNameBox.Text;
                string generatedFileName = fileName;
                int fileSuffix = Int32.Parse(startNumberFileNameBoxContent);
                rawIsChecked = findKeywordsCheckBox.IsChecked;
                bool isFindKeywords = ((bool)(rawIsChecked));
                rawIsChecked = findLinesUpperLetterCheckBox.IsChecked;
                bool isFindLinesUpperLetter = ((bool)(rawIsChecked));
                rawIsChecked = find2EmptyStringsCheckBox.IsChecked;
                bool isFind2EmptyLines = ((bool)(rawIsChecked));
                rawIsChecked = useNamedBookmarkCheckBox.IsChecked;
                bool isUseNamedBookmark = ((bool)(rawIsChecked));
                List<string> lines = new List<string>();
                if (isUseNamedBookmark)
                {
                    Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
                    string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
                    string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string saveDataFileContent = File.ReadAllText(saveDataFilePath);
                    SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
                    List<Dictionary<String, Object>> currentBookmarks = loadedContent.bookmarks;
                    foreach (Dictionary<String, Object> currentBookmark in currentBookmarks)
                    {
                        int index = ((int)(currentBookmark["index"]));
                        inputBox.Text.Insert(index, Environment.NewLine);
                    }
                }
                int linesCount = inputBox.LineCount;
                Debugger.Log(0, "debug", Environment.NewLine + "linesCount: " + linesCount.ToString() + Environment.NewLine);
                for (int i = 0; i < linesCount; i++)
                {
                    string line = inputBox.GetLineText(i);
                    lines.Add(line);
                }
                bool isLastLineEmpty = false;
                foreach (string line in lines)
                {
                    bool isKeywordsMatch = false;
                    bool isLineUpperLetterMatch = false;
                    bool is2EmptyLines = false;
                    if (isFindKeywords)
                    {
                        string keywordsBoxContent = keywordsBox.Text;
                        isKeywordsMatch = line.Contains(keywordsBoxContent);
                    }
                    if (isFindLinesUpperLetter)
                    {
                        isLineUpperLetterMatch = line.All((char someChar) =>
                        {
                            bool isUpper = Char.IsUpper(someChar);
                            return isUpper;
                        });
                    }
                    if (isFind2EmptyLines)
                    {
                        int lineLength = line.Length;
                        bool isLineEmpty = lineLength <= 0;
                        is2EmptyLines = isLastLineEmpty && isLineEmpty;
                        isLastLineEmpty = lineLength <= 0;
                    }

                    bool isAddFile = isKeywordsMatch || isLineUpperLetterMatch || is2EmptyLines;
                    if (isAddFile)
                    {
                        string rawFileSuffix = fileSuffix.ToString();
                        if (isAddAfter)
                        {
                            generatedFileName = fileName + " " + rawFileSuffix + fileExt;
                        }
                        else
                        {
                            generatedFileName = rawFileSuffix + " " + fileName + fileExt;
                        }
                        string filePath = saveFolderBoxContent + @"\" + generatedFileName;
                        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
                        speechSynthesizer.SetOutputToWaveFile(filePath);
                        speechSynthesizer.Speak(line);
                        fileSuffix++;
                    }
                }
                Cancel();
            }
            else if (isSourceFileBoxContentLengthNotExists)
            {
                MessageBox.Show("Необходимо указать имя исходного файла.", "Ошибка");
            }
            else if (isSaveFolderBoxContentLengthNotExists)
            {
                MessageBox.Show("Необходимо указать имя папки с результатами деления.", "Ошибка");
            }
        }


    }
}
