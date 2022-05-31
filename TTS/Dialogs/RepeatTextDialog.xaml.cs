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
    /// Логика взаимодействия для RepeatTextDialog.xaml
    /// </summary>
    public partial class RepeatTextDialog : Window
    {

        public MainWindow mainWindow;

        public RepeatTextDialog(MainWindow mainWindow)
        {
            InitializeComponent();

            Init(mainWindow);

        }

        public void Init (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
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
            int openedDocControlSelectedIndex = mainWindow.openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = mainWindow.openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxContent = inputBox.Text;
            int? possibleValue = countRepeatsSpinner.Value;
            bool isValueExists = possibleValue != null;
            if (isValueExists)
            {
                int countRepeat = possibleValue.Value;
                inputBoxContent = "";
                int lineCount = inputBox.LineCount;
                for (int i = 0; i < lineCount; i++)
                {
                    string line = inputBox.GetLineText(i);
                    line = line.Trim();
                    string totalLineContent = "";
                    for (int j = 0; j < countRepeat; j++)
                    {
                        totalLineContent += line;
                    }
                    int lastLineIndex = lineCount - 1;
                    bool isAddNewLine = i < lastLineIndex;
                    if (isAddNewLine)
                    {
                        totalLineContent += Environment.NewLine;
                    }
                    inputBoxContent += totalLineContent;
                }
                inputBox.Text = inputBoxContent;
                Cancel();
            }
        }

    }
}
