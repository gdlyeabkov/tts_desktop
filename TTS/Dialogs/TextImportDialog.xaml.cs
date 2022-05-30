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
    /// Логика взаимодействия для TextImportDialog.xaml
    /// </summary>
    public partial class TextImportDialog : Window
    {

        public int selectedFileIndex = -1;
        
        public TextImportDialog()
        {
            InitializeComponent();
        }

        public void OkHandler(object sender, RoutedEventArgs e)
        {
            Ok();
        }

        public void Ok()
        {
            UIElementCollection filesChildren = files.Children;
            int filesChildrenCount = filesChildren.Count;
            bool isHaveFiles = filesChildrenCount >= 1;
            if (isHaveFiles)
            {
                Encoding encoding = Encoding.Default;
                object rawIsChecked = ansiCheckBox.IsChecked;
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
                rawIsChecked = mergeCheckBox.IsChecked;
                bool isMerge = ((bool)(rawIsChecked));
                string saveFolderBoxContent = saveFolderBox.Text;
                string totalTextContent = "";
                Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
                foreach (StackPanel file in filesChildren)
                {
                    object fileData = file.DataContext;
                    string path = ((string)(fileData));
                    Microsoft.Office.Interop.Word.Document document = wordApplication.Documents.Add(path);
                }
                int documentCursor = -1;
                foreach (Microsoft.Office.Interop.Word.Document document in wordApplication.Documents)
                {
                    documentCursor++;
                    int count = document.Words.Count;
                    string textContent = "";
                    for (int i = 1; i <= count; i++)
                    {
                        string text = document.Words[i].Text;
                        textContent += text;
                    }
                    if (isMerge)
                    {
                        totalTextContent += textContent;
                    }
                    else
                    {
                        UIElement rawFile = filesChildren[documentCursor];
                        StackPanel file = ((StackPanel)(rawFile));
                        object fileData = file.DataContext;
                        string documentPath = ((string)(fileData));
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(documentPath);
                        string filePath = saveFolderBoxContent + @"\" + fileName + ".txt";
                        using (StreamWriter sw = new StreamWriter(File.Open(filePath, FileMode.Create), encoding))
                        {
                            sw.WriteLine(textContent);
                        }
                    }
                }
                if (isMerge)
                {
                    string fileName = mergeBox.Text;
                    string mergedFilePath = saveFolderBoxContent + @"\" + fileName + ".txt"; ;
                    using (StreamWriter sw = new StreamWriter(File.Open(mergedFilePath, FileMode.Create), encoding))
                    {
                        sw.WriteLine(totalTextContent);
                    }
                }
            }
        }

        public void CancelHandler(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel()
        {
            this.Close();
        }

        public void AddFilesHandler(object sender, RoutedEventArgs e)
        {
            AddFiles();
        }

        public void AddFiles()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".doc";
            ofd.Filter = "DOC|*.doc|DOCX|*.docx";
            ofd.Multiselect = true;
            bool? res = ofd.ShowDialog();
            bool isOpen = ((bool)(res));
            if (isOpen)
            {
                string[] paths = ofd.FileNames;
                foreach (string path in paths)
                {
                    StackPanel file = new StackPanel();
                    file.DataContext = path;
                    TextBlock fileNameLabel = new TextBlock();
                    fileNameLabel.Text = path;
                    file.Children.Add(fileNameLabel);
                    files.Children.Add(file);
                    file.MouseLeftButtonUp += SelectFileHandler;
                }
            }
        }

        public void RemoveFileHandler(object sender, RoutedEventArgs e)
        {
            RemoveFile();
        }

        public void RemoveFile()
        {
            bool isSelected = selectedFileIndex >= 0;
            if (isSelected)
            {
                files.Children.RemoveAt(selectedFileIndex);
                selectedFileIndex = -1;
            }
        }

        public void RemoveFilesHandler(object sender, RoutedEventArgs e)
        {
            RemoveFiles();
        }

        public void RemoveFiles()
        {
            files.Children.Clear();
            selectedFileIndex = -1;
        }

        public void SaveToFolderHandler(object sender, RoutedEventArgs e)
        {
            SaveToFolder();
        }

        public void SaveToFolder()
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

        public void SelectFileHandler(object sender, RoutedEventArgs e)
        {
            StackPanel file = ((StackPanel)(sender));
            SelectFile(file);
        }

        public void SelectFile(StackPanel file)
        {
            foreach (StackPanel localFile in files.Children)
            {
                localFile.Background = System.Windows.Media.Brushes.Transparent;
            }
            file.Background = System.Windows.Media.Brushes.SkyBlue;
            UIElementCollection filesChildren = files.Children;
            selectedFileIndex = filesChildren.IndexOf(file);
        }

        public void ShiftFileHandler(object sender, RoutedEventArgs e)
        {
            ShiftFile();
        }

        public void ShiftFile()
        {
            bool isSelected = selectedFileIndex >= 0;
            if (isSelected)
            {
                UIElementCollection filesChildren = files.Children;
                int filesChildrenCount = filesChildren.Count;
                int lastFileIndex = filesChildrenCount - 1;
                bool isCanShift = selectedFileIndex < lastFileIndex;
                if (isCanShift)
                {
                    selectedFileIndex++;
                    UIElement rawSelectedFile = filesChildren[selectedFileIndex];
                    StackPanel selectedFile = ((StackPanel)(rawSelectedFile));
                    SelectFile(selectedFile);
                }
            }
        }

        public void ReverseShiftFileHandler(object sender, RoutedEventArgs e)
        {
            ReverseShiftFile();
        }

        public void ReverseShiftFile()
        {
            bool isSelected = selectedFileIndex >= 0;
            if (isSelected)
            {
                UIElementCollection filesChildren = files.Children;
                int filesChildrenCount = filesChildren.Count;
                bool isCanShift = selectedFileIndex > 0;
                if (isCanShift)
                {
                    selectedFileIndex--;
                    UIElement rawSelectedFile = filesChildren[selectedFileIndex];
                    StackPanel selectedFile = ((StackPanel)(rawSelectedFile));
                    SelectFile(selectedFile);
                }
            }
        }

    }
}
