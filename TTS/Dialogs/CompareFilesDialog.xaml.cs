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
    /// Логика взаимодействия для CompareFilesDialog.xaml
    /// </summary>
    public partial class CompareFilesDialog : Window
    {



        public CompareFilesDialog()
        {
            InitializeComponent();
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
            object rawIsChecked = haveBothFileCheckBox.IsChecked;
            bool isHaveBothFileChecked = ((bool)(rawIsChecked));
            rawIsChecked = haveOneFileCheckBox.IsChecked;
            bool isHaveOneFileChecked = ((bool)(rawIsChecked));
            rawIsChecked = haveFirstFileAndSecondFileAbsentCheckBox.IsChecked;
            bool isHaveFirstFileAndSecondFileAbsentChecked = ((bool)(rawIsChecked));
            rawIsChecked = haveSecondFileAndFirstFileAbsentCheckBox.IsChecked;
            bool isHaveSecondFileAndFirstFileAbsentChecked = ((bool)(rawIsChecked));
            rawIsChecked = anyFileCheckBox.IsChecked;
            bool isAnyFileChecked = ((bool)(rawIsChecked));

            string firstFileBoxContent = firstFileBox.Text;
            string secondFileBoxContent = secondFileBox.Text;
            if (isHaveBothFileChecked)
            {
                firstFileBoxContent = "";
                secondFileBoxContent = "";
                object firstFileBoxData = firstFileBox.DataContext;
                string firstFilePath = ((string)(firstFileBoxData));
                string[] lines = File.ReadAllLines(firstFilePath);
                object secondFileBoxData = secondFileBox.DataContext;
                string secondFilePath = ((string)(secondFileBoxData));
                string[] otherLines = File.ReadAllLines(secondFilePath);
                int linesCursor = 0;
                int linesCount = lines.Length;
                foreach (string line in lines)
                {
                    linesCursor++;
                    foreach (string otherLine in otherLines)
                    {
                        bool isLineMatch = otherLine == line;
                        if (isLineMatch)
                        {
                            firstFileBoxContent += line;
                            bool isNotLastLine = linesCursor < linesCount;
                            if (isNotLastLine)
                            {
                                firstFileBoxContent += Environment.NewLine;
                            }
                            break;
                        }
                    }
                }
            }
            else if (isHaveOneFileChecked)
            {
                string tempFirstFileBoxContent = firstFileBoxContent;
                string tempSecondFileBoxContent = secondFileBoxContent;
                firstFileBox.Text = firstFileBoxContent;
                firstFileBoxContent = "";
                secondFileBoxContent = "";
                object firstFileBoxData = firstFileBox.DataContext;
                string firstFilePath = ((string)(firstFileBoxData));
                string[] lines = File.ReadAllLines(firstFilePath);
                object secondFileBoxData = secondFileBox.DataContext;
                string secondFilePath = ((string)(secondFileBoxData));
                string[] otherLines = File.ReadAllLines(secondFilePath);
                int linesCursor = 0;
                int linesCount = lines.Length;
                foreach (string line in lines)
                {
                    linesCursor++;
                    bool isLineNotMatch = tempSecondFileBoxContent.Contains(line);
                    bool isLineMatch = !isLineNotMatch;
                    if (isLineMatch)
                    {
                        firstFileBoxContent += line;
                        bool isNotLastLine = linesCursor < linesCount;
                        if (isNotLastLine)
                        {
                            firstFileBoxContent += Environment.NewLine;
                        }
                    }
                }
                linesCursor = 0;
                foreach (string otherLine in otherLines)
                {
                    linesCursor++;
                    bool isLineNotMatch = tempFirstFileBoxContent.Contains(otherLine);
                    bool isLineMatch = !isLineNotMatch;
                    if (isLineMatch)
                    {
                        firstFileBoxContent += otherLine;
                        bool isNotLastLine = linesCursor < linesCount;
                        if (isNotLastLine)
                        {
                            firstFileBoxContent += Environment.NewLine;
                        }
                    }
                }
            }
            else if (isHaveFirstFileAndSecondFileAbsentChecked)
            {
                string tempFirstFileBoxContent = firstFileBoxContent;
                string tempSecondFileBoxContent = secondFileBoxContent;
                firstFileBox.Text = firstFileBoxContent;
                firstFileBoxContent = "";
                secondFileBoxContent = "";
                object firstFileBoxData = firstFileBox.DataContext;
                string firstFilePath = ((string)(firstFileBoxData));
                string[] lines = File.ReadAllLines(firstFilePath);
                object secondFileBoxData = secondFileBox.DataContext;
                string secondFilePath = ((string)(secondFileBoxData));
                string[] otherLines = File.ReadAllLines(secondFilePath);
                int linesCursor = 0;
                int linesCount = lines.Length;
                foreach (string line in lines)
                {
                    linesCursor++;
                    bool isHaveSecondFile = tempSecondFileBoxContent.Contains(line);
                    bool isSecondFileAbsent = !isHaveSecondFile;
                    bool isHaveFirstFile = tempFirstFileBoxContent.Contains(line);
                    bool isLineMatch = isSecondFileAbsent && isHaveFirstFile;
                    if (isLineMatch)
                    {
                        firstFileBoxContent += line;
                        bool isNotLastLine = linesCursor < linesCount;
                        if (isNotLastLine)
                        {
                            firstFileBoxContent += Environment.NewLine;
                        }
                    }
                }
            }
            else if (isHaveSecondFileAndFirstFileAbsentChecked)
            {
                string tempFirstFileBoxContent = firstFileBoxContent;
                string tempSecondFileBoxContent = secondFileBoxContent;
                firstFileBox.Text = firstFileBoxContent;
                firstFileBoxContent = "";
                secondFileBoxContent = "";
                object firstFileBoxData = firstFileBox.DataContext;
                string firstFilePath = ((string)(firstFileBoxData));
                object secondFileBoxData = secondFileBox.DataContext;
                string secondFilePath = ((string)(secondFileBoxData));
                string[] otherLines = File.ReadAllLines(secondFilePath);
                int linesCursor = 0;
                int linesCount = otherLines.Length;
                foreach (string line in otherLines)
                {
                    linesCursor++;
                    bool isHaveFirstFile = tempFirstFileBoxContent.Contains(line);
                    bool isFirstFileAbsent = !isHaveFirstFile;
                    bool isHaveSecondFile = tempSecondFileBoxContent.Contains(line);
                    bool isLineMatch = isFirstFileAbsent && isHaveSecondFile;
                    if (isLineMatch)
                    {
                        firstFileBoxContent += line;
                        bool isNotLastLine = linesCursor < linesCount;
                        if (isNotLastLine)
                        {
                            firstFileBoxContent += Environment.NewLine;
                        }
                    }
                }
            }
            else if (isAnyFileChecked)
            {
                string tempFirstFileBoxContent = firstFileBoxContent;
                string tempSecondFileBoxContent = secondFileBoxContent;
                firstFileBox.Text = firstFileBoxContent;
                firstFileBoxContent = "";
                secondFileBoxContent = "";
                object firstFileBoxData = firstFileBox.DataContext;
                string firstFilePath = ((string)(firstFileBoxData));
                string[] lines = File.ReadAllLines(firstFilePath);
                object secondFileBoxData = secondFileBox.DataContext;
                string secondFilePath = ((string)(secondFileBoxData));
                string[] otherLines = File.ReadAllLines(secondFilePath);
                int linesCursor = 0;
                int linesCount = lines.Length;
                foreach (string line in lines)
                {
                    linesCursor++;
                    bool isHaveSecondFile = tempSecondFileBoxContent.Contains(line);
                    bool isHaveFirstFile = tempFirstFileBoxContent.Contains(line);
                    bool isLineMatch = isHaveSecondFile && isHaveFirstFile;
                    if (isLineMatch)
                    {
                        firstFileBoxContent += line;
                        bool isNotLastLine = linesCursor < linesCount;
                        if (isNotLastLine)
                        {
                            firstFileBoxContent += Environment.NewLine;
                        }
                    }
                }
            }
            string resultBoxContent = firstFileBoxContent + Environment.NewLine + secondFileBoxContent;
            resultBox.Text = resultBoxContent;

        }

        public void SaveResultHandler(object sender, RoutedEventArgs e)
        {
            SaveResult();
        }

        public void SaveResult()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Документ";
            sfd.DefaultExt = ".txt";
            sfd.Filter = "Документы (.txt)|*.txt";
            bool? res = sfd.ShowDialog();
            bool isSave = ((bool)(res));
            if (isSave)
            {
                string resultBoxContent = resultBox.Text;
                string path = sfd.FileName;
                using (System.IO.Stream s = File.Open(path, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(resultBoxContent);
                    }
                };
            }
        }

        public void OpenFirstFileHandler(object sender, RoutedEventArgs e)
        {
            OpenFirstFile();
        }

        public void OpenFirstFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "Документы (.txt)|*.txt";
            bool? res = ofd.ShowDialog();
            bool isOpen = ((bool)(res));
            if (isOpen)
            {
                string path = ofd.FileName;
                string content = File.ReadAllText(path);
                firstFileBox.Text = content;
                string[] lines = File.ReadAllLines(path);
                int linesCount = lines.Length;
                string rawLinesCount = linesCount.ToString();
                firstFileCountLinesLabel.Text = rawLinesCount;
                string firstFileBoxContent = firstFileBox.Text;
                int firstFileBoxContentLength = firstFileBoxContent.Length;
                bool isHaveFirstFile = firstFileBoxContentLength >= 1;
                string secondFileBoxContent = secondFileBox.Text;
                int secondFileBoxContentLength = secondFileBoxContent.Length;
                bool isHaveSecondFile = secondFileBoxContentLength >= 1;
                bool isCanCompare = isHaveFirstFile && isHaveSecondFile;
                okBtn.IsEnabled = isCanCompare;
                firstFileBox.DataContext = path;
            }
        }

        public void OpenSecondFileHandler(object sender, RoutedEventArgs e)
        {
            OpenSecondFile();
        }

        public void OpenSecondFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "Документы (.txt)|*.txt";
            bool? res = ofd.ShowDialog();
            bool isOpen = ((bool)(res));
            if (isOpen)
            {
                string path = ofd.FileName;
                string content = File.ReadAllText(path);
                secondFileBox.Text = content;
                string[] lines = File.ReadAllLines(path);
                int linesCount = lines.Length;
                string rawLinesCount = linesCount.ToString();
                secondFileCountLinesLabel.Text = rawLinesCount;
                string firstFileBoxContent = firstFileBox.Text;
                int firstFileBoxContentLength = firstFileBoxContent.Length;
                bool isHaveFirstFile = firstFileBoxContentLength >= 1;
                string secondFileBoxContent = secondFileBox.Text;
                int secondFileBoxContentLength = secondFileBoxContent.Length;
                bool isHaveSecondFile = secondFileBoxContentLength >= 1;
                bool isCanCompare = isHaveFirstFile && isHaveSecondFile;
                okBtn.IsEnabled = isCanCompare;
                secondFileBox.DataContext = path;
            }
        }

        public void ToggleModeHandler (object sender, RoutedEventArgs e)
        {
            RadioButton radioBtn = ((RadioButton)(sender));
            ToggleMode(radioBtn);
        }

        public void ToggleMode (RadioButton radioBtn)
        {
            UIElementCollection modesChildren = modes.Children;
            foreach (StackPanel mode in modesChildren)
            {
                UIElementCollection modeChildren = mode.Children;
                object rawBtn = modeChildren[1];
                RadioButton btn = ((RadioButton)(rawBtn));
                btn.IsChecked = false;
            }
            radioBtn.IsChecked = true;
        }

    }
}
