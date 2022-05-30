using Microsoft.Win32;
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

namespace TTS.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SplitFileDialog.xaml
    /// </summary>
    public partial class SplitFileDialog : Window
    {
        public SplitFileDialog()
        {
            InitializeComponent();
        }


        public void PickSourceFileHandler (object sender, RoutedEventArgs e)
        {
            PickSourceFile();
        }

        public void PickSourceFile ()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "Документы (.txt)|*.txt";
            bool? res = ofd.ShowDialog();
            bool isOpen = ((bool)(res));
            if (isOpen)
            {
                string path = ofd.FileName;
                sourceFileBox.Text = path;
                FileInfo fileInfo = new FileInfo(path);
                // string fileName = fileInfo.Name;
                string fileName = System.IO.Path.GetFileName(path);
                templateFileBox.Text = fileName;
            }
        }

        public void PickSavedFolderHandler (object sender, RoutedEventArgs e)
        {
            PickSavedFolder();
        }

        public void PickSavedFolder ()
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

        public void ToggleFindKeywordsHandler (object sender, RoutedEventArgs e)
        {
            ToggleFindKeywords();
        }

        public void ToggleFindKeywords ()
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

        public void RunHandler(object sender, RoutedEventArgs e)
        {
            Run();
        }

        public void Run()
        {
            string sourceFileBoxContent = sourceFileBox.Text;
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
                string content = File.ReadAllText(sourceFileBoxContent);
                // string fileName = System.IO.Path.GetFileName(sourceFileBoxContent);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(sourceFileBoxContent);
                string fileExt = System.IO.Path.GetExtension(sourceFileBoxContent);
                object rawIsChecked = addNumberAfterFileNameRadioBtn.IsChecked;
                bool isAddAfter = ((bool)(rawIsChecked));
                string startNumberFileNameBoxContent = startNumberFileNameBox.Text;
                string generatedFileName = fileName;
                Encoding encoding = Encoding.Default;
                rawIsChecked = ansiCheckBox.IsChecked;
                bool isAnsi = ((bool)(rawIsChecked));
                rawIsChecked = utf8CheckBox.IsChecked;
                bool isUtf8 = ((bool)(rawIsChecked));
                rawIsChecked = unicodeCheckBox.IsChecked;
                bool isUnicode = ((bool)(rawIsChecked));
                if (isAnsi)
                {
                    encoding = Encoding.Default;
                }
                else if (isUtf8)
                {
                    encoding = Encoding.UTF8;
                }
                else if (isUnicode)
                {
                    encoding = Encoding.Unicode;
                }
                string[] lines = File.ReadAllLines(sourceFileBoxContent);
                int fileSuffix = Int32.Parse(startNumberFileNameBoxContent);
                rawIsChecked = findKeywordsCheckBox.IsChecked;
                bool isFindKeywords = ((bool)(rawIsChecked));
                rawIsChecked = findLinesUpperLetterCheckBox.IsChecked;
                bool isFindLinesUpperLetter = ((bool)(rawIsChecked));
                rawIsChecked = find2EmptyStringsCheckBox.IsChecked;
                bool isFind2EmptyLines = ((bool)(rawIsChecked));
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
                            // generatedFileName = fileName + " " + startNumberFileNameBoxContent + fileExt;
                            generatedFileName = fileName + " " + rawFileSuffix + fileExt;
                        }
                        else
                        {
                            // generatedFileName = startNumberFileNameBoxContent + " " + fileName + fileExt;
                            generatedFileName = rawFileSuffix + " " + fileName + fileExt;
                        }
                        // File.WriteAllText(saveFolderBoxContent + @"\" + generatedFileName, content);
                        string filePath = saveFolderBoxContent + @"\" + generatedFileName;
                        using (StreamWriter sw = new StreamWriter(File.Open(filePath, FileMode.Create), encoding))
                        {
                            // sw.WriteLine(content);
                            sw.WriteLine(line);
                        }
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

        public void ToggleUseFileSizeHandler (object sender, RoutedEventArgs e)
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

    }
}
