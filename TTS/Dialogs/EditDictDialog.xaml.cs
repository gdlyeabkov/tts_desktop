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
using System.Web.Script.Serialization;

namespace TTS.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для EditDictDialog.xaml
    /// </summary>
    public partial class EditDictDialog : Window
    {

        public string dictName;
        public TextBox lastBox = null;
        public int selectedDictItemIndex = -1;

        public EditDictDialog(string dictName)
        {
            InitializeComponent();

            Init(dictName);
        
        }

        public void Init (string dictName)
        {
            this.dictName = dictName;
            GetDictContent();
            lastBox = fromBox;
        }

        public void GetDictContent ()
        {
            /*string dictContent = File.ReadAllText(dictName);
            dictContentBox.Text = dictContent;*/
            mainDictContent.Children.Clear();
            JavaScriptSerializer js = new JavaScriptSerializer();
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
                dictItemLabel.Text = dictContentKey + "=" + dictContentValue;
                dictItem.Children.Add(dictItemLabel);
                mainDictContent.Children.Add(dictItem);
                dictItem.MouseLeftButtonUp += SelectDictItemHandler;
            }
            UIElementCollection mainDictContentChildren = mainDictContent.Children;
            int mainDictContentChildrenCount = mainDictContentChildren.Count;
            bool isHaveItems = mainDictContentChildrenCount >= 1;
            if (isHaveItems)
            {
                SelectDictItem(0);
            }
        }

        public void SpeakFromHandler (object sender, RoutedEventArgs e)
        {
            SpeakFrom();
        }

        public void SpeakFrom ()
        {
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
            string fromBoxContent = fromBox.Text;
            speechSynthesizer.Speak(fromBoxContent);
        }

        public void SpeakToHandler (object sender, RoutedEventArgs e)
        {
            SpeakTo();
        }

        public void SpeakTo ()
        {
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
            string toBoxContent = toBox.Text;
            speechSynthesizer.Speak(toBoxContent);
        }

        public void NewHandler(object sender, RoutedEventArgs e)
        {
            New();
        }

        public void New()
        {
            fromBox.Text = "";
            toBox.Text = "";
        }

        public void AddHandler(object sender, RoutedEventArgs e)
        {
            Add();
        }

        public void Add()
        {

            string fromBoxContent = fromBox.Text;
            string toBoxContent = toBox.Text;

            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(dictName);
            // Dictionary<String, Object>  dictContent = new Dictionary<String, Object>();
            Dictionary<String, Object> dictContent = js.Deserialize<Dictionary<String, Object>>(saveDataFileContent);
            dictContent.Add(fromBoxContent, toBoxContent);
            string savedContent = js.Serialize(dictContent);
            File.WriteAllText(dictName, savedContent);

            GetDictContent();
        }

        public void AddLeftCornerBracketCharHandler (object sender, RoutedEventArgs e)
        {
            AddLeftCornerBracketChar();
        }

        public void AddLeftCornerBracketChar()
        {
            /*
            bool isFromBoxFocused = fromBox.IsMouseCaptured;
            if (isFromBoxFocused)
            {
                string fromBoxContent = fromBox.Text;
                fromBoxContent += "<";
                fromBox.Text = fromBoxContent;
            }
            else
            {
                string toBoxContent = toBox.Text;
                toBoxContent += "<";
                toBox.Text = toBoxContent;
            }
            */
            string lastBoxContent = lastBox.Text;
            lastBoxContent += "<";
            lastBox.Text = lastBoxContent;
        }

        public void AddRightCornerBracketCharHandler(object sender, RoutedEventArgs e)
        {
            AddRightCornerBracketChar();
        }

        public void AddRightCornerBracketChar()
        {
            /*
            bool isFromBoxFocused = fromBox.IsMouseCaptured;
            if (isFromBoxFocused)
            {
                string fromBoxContent = fromBox.Text;
                fromBoxContent += ">";
                fromBox.Text = fromBoxContent;
            }
            else
            {
                string toBoxContent = toBox.Text;
                toBoxContent += ">";
                toBox.Text = toBoxContent;
            }
            */
            string lastBoxContent = lastBox.Text;
            lastBoxContent += "<";
            lastBox.Text = lastBoxContent;
        }

        public void AddStarCharHandler(object sender, RoutedEventArgs e)
        {
            AddStarChar();
        }

        public void AddStarChar()
        {
            /*
            bool isFromBoxFocused = fromBox.IsMouseCaptured;
            if (isFromBoxFocused)
            {
                string fromBoxContent = fromBox.Text;
                fromBoxContent += "*";
                fromBox.Text = fromBoxContent;
            }
            else
            {
                string toBoxContent = toBox.Text;
                toBoxContent += "*";
                toBox.Text = toBoxContent;
            }
            */
            string lastBoxContent = lastBox.Text;
            lastBoxContent += "<";
            lastBox.Text = lastBoxContent;
        }

        private void SetActiveBoxHandler (object sender, MouseEventArgs e)
        {
            TextBox box = ((TextBox)(sender));
            SetActiveBox(box);
        }

        public void SetActiveBox (TextBox box)
        {
            lastBox = box;
        }

        public void InsertHandler(object sender, RoutedEventArgs e)
        {
            Insert();
        }

        public void Insert()
        {

            Add();
        }

        public void ReplaceHandler(object sender, RoutedEventArgs e)
        {
            Replace();
        }

        public void Replace()
        {
            string fromBoxContent = fromBox.Text;
            string toBoxContent = toBox.Text;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(dictName);
            Dictionary<String, Object> dictContent = js.Deserialize<Dictionary<String, Object>>(saveDataFileContent);
            int dictsCount = dictContent.Count;
            bool isHaveDicts = dictsCount >= 1;
            if (isHaveDicts)
            {
                var dictKeys = dictContent.Keys;
                // string firstKey = dictKeys.First();
                KeyValuePair<String, Object> dictElement = dictContent.ElementAt(selectedDictItemIndex);
                string firstKey = dictElement.Key; 
                dictContent[firstKey] = toBoxContent;
                string savedContent = js.Serialize(dictContent);
                File.WriteAllText(dictName, savedContent);
                GetDictContent();
            }
        }

        public void RemoveHandler(object sender, RoutedEventArgs e)
        {
            Remove();
        }

        public void Remove()
        {
            string fromBoxContent = fromBox.Text;
            string toBoxContent = toBox.Text;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(dictName);
            Dictionary<String, Object> dictContent = js.Deserialize<Dictionary<String, Object>>(saveDataFileContent);
            int dictsCount = dictContent.Count;
            bool isHaveDicts = dictsCount >= 1;
            if (isHaveDicts)
            {
                var dictKeys = dictContent.Keys;
                // string firstKey = dictKeys.First();
                KeyValuePair<String, Object> dictElement = dictContent.ElementAt(selectedDictItemIndex);
                string firstKey = dictElement.Key;
                dictContent.Remove(firstKey);
                string savedContent = js.Serialize(dictContent);
                File.WriteAllText(dictName, savedContent);
                GetDictContent();
            }
        }

        public void SelectDictItemHandler (object sender, RoutedEventArgs e)
        {
            StackPanel dictItem = ((StackPanel)(sender));
            int dictItemIndex = mainDictContent.Children.IndexOf(dictItem);
            SelectDictItem(dictItemIndex);
        }

        public void SelectDictItem (int dictItemIndex)
        {
            UIElementCollection mainDictContentChildren = mainDictContent.Children;
            foreach (StackPanel mainDictContentChildrenItem in mainDictContentChildren)
            {
                mainDictContentChildrenItem.Background = System.Windows.Media.Brushes.Transparent;
            }
            selectedDictItemIndex = dictItemIndex;
            UIElement rawDictItem = mainDictContentChildren[selectedDictItemIndex];
            StackPanel dictItem = ((StackPanel)(rawDictItem));
            dictItem.Background = System.Windows.Media.Brushes.SkyBlue;
        }

    }

}
