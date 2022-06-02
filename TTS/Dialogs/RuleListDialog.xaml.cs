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
using System.Windows.Shapes;

namespace TTS.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для RuleListDialog.xaml
    /// </summary>
    public partial class RuleListDialog : Window
    {
        public RuleListDialog()
        {
            InitializeComponent();

            Init();

        }

        public void Init()
        {
            dicts.Children.Clear();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string dictsFolder = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\dicts";
            string[] dictFiles = Directory.GetFileSystemEntries(dictsFolder);
            foreach (string dictName in dictFiles)
            {
                string saveDataFileContent = File.ReadAllText(dictName);
                Dictionary<String, Object> dictContent = js.Deserialize<Dictionary<String, Object>>(saveDataFileContent);
                int cursor = -1;
                foreach (var dictContentKey in dictContent.Keys)
                {
                    cursor++;
                    string dictContentValue = ((string)(dictContent[dictContentKey]));
                    StackPanel dictItem = new StackPanel();
                    dictItem.Background = System.Windows.Media.Brushes.Transparent;
                    TextBlock dictItemLabel = new TextBlock();
                    string dictItemLabelContent = dictContentKey + "=" + dictContentValue;
                    dictItemLabel.Text = dictItemLabelContent;
                    dictItem.Children.Add(dictItemLabel);
                    dicts.Children.Add(dictItem);
                    dictItem.DataContext = dictItemLabelContent;
                    dictItem.MouseLeftButtonUp += SelectDictItemHandler;
                }
            }
        }

        public void FindHandler (object sender, RoutedEventArgs e)
        {
            Find();
        }

        public void Find ()
        {
            string keywords = keywordsBox.Text;
            string insensitiveCaseKeywords = keywords.ToLower();
            StackPanel item = null;
            foreach (StackPanel dict in dicts.Children)
            {
                object rawDictData = dict.DataContext;
                string dictData = ((string)(rawDictData));
                string insensitiveCaseDictData = dictData.ToLower();
                bool isMatch = insensitiveCaseDictData.Contains(insensitiveCaseKeywords);
                if (isMatch)
                {
                    item = dict;
                    SelectDictItem(item);
                    break;
                }
            }
            bool isItemFound = item != null;
            if (isItemFound)
            {
                var point = item.TranslatePoint(Mouse.GetPosition(dictsScroll), dicts);
                dictsScroll.ScrollToVerticalOffset(point.Y + (item.ActualHeight / 2));
            }
        }

        public void SelectDictItemHandler (object sender, RoutedEventArgs e)
        {
            StackPanel dictItem = ((StackPanel)(sender)); 
            SelectDictItem(dictItem);
        }

        public void SelectDictItem (StackPanel dictItem)
        {
            foreach (StackPanel dict in dicts.Children)
            {
                dict.Background = System.Windows.Media.Brushes.Transparent;
            }
            dictItem.Background = System.Windows.Media.Brushes.SkyBlue;
        }

        public void CancelHandler(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel()
        {
            this.Close();
        }

    }
}
