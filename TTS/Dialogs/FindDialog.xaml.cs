using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для FindDialog.xaml
    /// </summary>
    public partial class FindDialog : Window
    {

        public TextBox inputBox;

        public FindDialog(TextBox inputBox)
        {
            InitializeComponent();

            Init(inputBox);

        }

        public void Init (TextBox inputBox)
        {
            this.inputBox = inputBox;
        }

        private void DetectFromBoxHandler(object sender, TextChangedEventArgs e)
        {
            DetectFromBox();
        }

        public void DetectFromBox()
        {
            string fromBoxContent = fromBox.Text;
            int fromBoxContentLength = fromBoxContent.Length;
            bool isHaveContent = fromBoxContentLength >= 1;
            findBtn.IsEnabled = isHaveContent;
        }

        public void CancelHandler(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel()
        {
            this.Close();
        }

        public void FindHandler(object sender, RoutedEventArgs e)
        {
            Find();
        }

        public void Find()
        {
            string inputBoxContent = inputBox.Text;
            string fromBoxContent = fromBox.Text;
            bool isFound = inputBoxContent.Contains(fromBoxContent);
            if (isFound)
            {
                int findCursor = 0;
                for (int i = 0; i < inputBoxContent.Length; i++)
                {
                    char inputBoxContentItem = inputBoxContent[i];
                    for (int j = 0; j < fromBoxContent.Length; j++)
                    {
                        char fromBoxContentItem = fromBoxContent[j];
                        bool isMatch = inputBoxContentItem == fromBoxContentItem;
                        if (isMatch)
                        {
                            findCursor++;
                        }
                        break;
                    }
                }
                bool isMatchFound = findCursor >= fromBoxContent.Length;
                if (isMatchFound)
                {
                    inputBox.SelectAll();
                }
                Debugger.Log(0, "debug", Environment.NewLine + "findCursor: " + findCursor + ", fromBoxContentLength: " + fromBoxContent.Length + Environment.NewLine);
            }
            else
            {
                MessageBox.Show("Не удается найти " + "\"" + fromBoxContent + "\"", "Внимание");
            }
        }


    }
}
