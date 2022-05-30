using Microsoft.Win32;
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

using System.Speech.Synthesis;
using System.IO;

namespace TTS.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ConvertToAudioDialog.xaml
    /// </summary>
    public partial class ConvertToAudioDialog : Window
    {

        public int selectedFileIndex = -1;

        public ConvertToAudioDialog()
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
                UIElement rawFirstFile = filesChildren[0];
                StackPanel firstFile = ((StackPanel)(rawFirstFile));
                object firstFileData = firstFile.DataContext;
                string firstFilePath = ((string)(firstFileData));
                string firstFileName = System.IO.Path.GetFileNameWithoutExtension(firstFilePath);
                string content = "";
                foreach (StackPanel file in filesChildren)
                {
                    object fileData = file.DataContext;
                    string path = ((string)(fileData));
                    string localContent = File.ReadAllText(path);
                    content += localContent;
                }
                SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
                string saveFolderBoxContent = saveFolderBox.Text;
                speechSynthesizer.SetOutputToWaveFile(saveFolderBoxContent + @"\" + firstFileName + ".wav");
                speechSynthesizer.SpeakAsync(content);
                speechSynthesizer.SpeakCompleted += SpeakCompletedHandler;
            }
            else
            {

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
            ofd.DefaultExt = ".txt";
            ofd.Filter = "Документы (.txt)|*.txt";
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

        public void SelectFileHandler (object sender, RoutedEventArgs e)
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

        public void ShiftFileHandler (object sender, RoutedEventArgs e)
        {
            ShiftFile();
        }

        public void ShiftFile ()
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

        public void SpeakCompletedHandler (object sender, SpeakCompletedEventArgs e)
        {
            SpeakCompleted();
        }

        public void SpeakCompleted ()
        {
            object rawIsChecked = removeFilesCheckBox.IsChecked;
            bool isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                foreach (StackPanel file in files.Children)
                {
                    object fileData = file.DataContext;
                    string path = ((string)(fileData));
                    File.Delete(path);
                }
                files.Children.Clear();
            }
        }

    }
}
