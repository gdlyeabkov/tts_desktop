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
    /// Логика взаимодействия для SpellCheckDialog.xaml
    /// </summary>
    public partial class SpellCheckDialog : Window
    {

        public MainWindow mainWindow;

        public SpellCheckDialog(MainWindow mainWindow)
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
            int charIndex = 0;
            int indx = 0;
            LogicalDirection forwardDirection = LogicalDirection.Forward;
            while (true)
            {
                indx = inputBox.GetNextSpellingErrorCharacterIndex(indx, forwardDirection);
                bool isIndexFound = indx > -1;
                if (isIndexFound)
                {
                    SpellingError error = inputBox.GetSpellingError(indx);
                    IEnumerable<string> suggestions = error.Suggestions;
                    List<string> suggestionsList = suggestions.ToList<string>();
                    int suggestionsCount = suggestionsList.Count;
                    bool isHaveSuggestions = suggestionsCount >= 1;
                    if (isHaveSuggestions)
                    {
                        string suggestion = suggestionsList[0];
                        RowDefinition row = new RowDefinition();
                        errors.RowDefinitions.Add(row);
                        RowDefinitionCollection rows = errors.RowDefinitions;
                        int rowsCount = rows.Count;
                        int lastRowIndex = rowsCount - 1;
                        string inputBoxContent = inputBox.Text;
                        int errorStartIndex = inputBox.GetSpellingErrorStart(indx);
                        int errorLength = inputBox.GetSpellingErrorLength(indx);
                        string errorName = inputBoxContent.Substring(errorStartIndex, errorLength);
                        TextBlock errorNameLabel = new TextBlock();
                        errorNameLabel.Text = errorName;
                        errorNameLabel.Margin = new Thickness(15);
                        errors.Children.Add(errorNameLabel);
                        Grid.SetRow(errorNameLabel, lastRowIndex);
                        Grid.SetColumn(errorNameLabel, 0);
                        TextBlock errorFixLabel = new TextBlock();
                        errorFixLabel.Text = suggestion;
                        errorFixLabel.Margin = new Thickness(15);
                        errors.Children.Add(errorFixLabel);
                        Grid.SetRow(errorFixLabel, lastRowIndex);
                        Grid.SetColumn(errorFixLabel, 1);
                        charIndex = inputBox.GetSpellingErrorStart(charIndex);
                    }
                    int len = inputBox.GetSpellingErrorLength(indx);
                    indx += len;
                }
                else
                {
                    break;
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

        public void OkHandler(object sender, RoutedEventArgs e)
        {
            Ok();
        }

        public void Ok()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Документ";
            sfd.DefaultExt = ".txt";
            sfd.Filter = "Документы (.txt)|*.txt";
            bool? res = sfd.ShowDialog();
            bool isSave = ((bool)(res));
            if (isSave)
            {
                string path = sfd.FileName;
                string content = "";
                string newLine = Environment.NewLine;
                UIElementCollection errorsChildren = errors.Children;
                foreach (TextBlock errorsItem in errorsChildren)
                {
                    int rowIndex = Grid.GetRow(errorsItem);
                    bool isData = rowIndex >= 1;
                    if (isData)
                    {
                        int colIndex = Grid.GetColumn(errorsItem);
                        bool isError = colIndex == 0;
                        if (isError)
                        {
                            string msg = errorsItem.Text;
                            content += msg + newLine;
                        }
                    }
                }
                using (System.IO.Stream s = File.Open(path, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(content);
                    }
                };
            }
        }

    }
}
