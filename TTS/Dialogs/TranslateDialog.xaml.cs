using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для TranslateDialog.xaml
    /// </summary>
    public partial class TranslateDialog : Window
    {

        public MainWindow mainWindow;

        public TranslateDialog(MainWindow mainWindow)
        {
            InitializeComponent();

            Init(mainWindow);
        
        }

        public void Init (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private void TranslateHandler (object sender, RoutedEventArgs e)
        {
            Translate();
        }

        public void Translate ()
        {
            int toLangSelectorSelectedIndex = toLangSelector.SelectedIndex;
            ItemCollection toLangSelectorItems = toLangSelector.Items;
            object rawToLangSelectorSelectedItem = toLangSelectorItems[toLangSelectorSelectedIndex];
            ComboBoxItem toLangSelectorSelectedItem = ((ComboBoxItem)(rawToLangSelectorSelectedItem));
            object toLangSelectorSelectedItemData = toLangSelectorSelectedItem.DataContext;
            string toLang = toLangSelectorSelectedItemData.ToString();
            string inputBoxContent = inputBox.Text;
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(@"http://transland.herokuapp.com/api/translate/?words=" + inputBoxContent + "&outputlanguage=" + toLang);
            webRequest.UserAgent = "Client app";
            webRequest.Method = "GET";
            try
            {
                using (var webResponse = webRequest.GetResponse())
                {
                    using (var reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var objText = reader.ReadToEnd();
                        TranslateResponseInfo myobj = (TranslateResponseInfo)js.Deserialize(objText, typeof(TranslateResponseInfo));
                        string status = myobj.status;
                        Debugger.Log(0, "debug", Environment.NewLine + "status: " + status + Environment.NewLine);
                        bool isOk = status == "OK";
                        if (isOk)
                        {
                            string result = myobj.result;
                            outputBox.Text = result;
                        }
                    }
                }
            }
            catch (WebException)
            {
                MessageBox.Show("Не удалось перевести.", "Ошибка");
            }
        }

        private void OpenSettingsHandler (object sender, RoutedEventArgs e)
        {
            OpenSettings();
        }

        public void OpenSettings ()
        {

        }

        private void CancelHandler (object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel ()
        {
            this.Close();
        }

        public void ToggleLangsHandler (object sender, RoutedEventArgs e)
        {
            ToggleLangs();
        }

        public void ToggleLangs ()
        {
            /*int fromLangSelectorSelectedIndex = fromLangSelector.SelectedIndex;
            ItemCollection fromLangSelectorItems = fromLangSelector.Items;
            object rawFromLangSelectorSelectedItem = fromLangSelectorItems[fromLangSelectorSelectedIndex];
            ComboBoxItem fromLangSelectorSelectedItem = ((ComboBoxItem)(rawFromLangSelectorSelectedItem));
            object fromLangSelectorSelectedItemData = fromLangSelectorSelectedItem.DataContext;
            string fromLang = fromLangSelectorSelectedItemData.ToString();
            int toLangSelectorSelectedIndex = toLangSelector.SelectedIndex;
            ItemCollection toLangSelectorItems = toLangSelector.Items;
            object rawToLangSelectorSelectedItem = toLangSelectorItems[toLangSelectorSelectedIndex];
            ComboBoxItem toLangSelectorSelectedItem = ((ComboBoxItem)(rawToLangSelectorSelectedItem));
            object toLangSelectorSelectedItemData = toLangSelectorSelectedItem.DataContext;
            string toLang = toLangSelectorSelectedItemData.ToString();*/
            int fromLangSelectorSelectedIndex = fromLangSelector.SelectedIndex;
            int toLangSelectorSelectedIndex = toLangSelector.SelectedIndex;
            toLangSelector.SelectedIndex = fromLangSelectorSelectedIndex;
            fromLangSelector.SelectedIndex = toLangSelectorSelectedIndex;
        }

        public void InsertFromBufferHandler (object sender, RoutedEventArgs e)
        {
            InsertFromBuffer();
        }

        public void InsertFromBuffer ()
        {
            string copiedText = Clipboard.GetText();
            inputBox.Text = copiedText;
        }

        public void InsertFromDocHandler (object sender, RoutedEventArgs e)
        {
            InsertFromDoc();
        }

        public void InsertFromDoc ()
        {

            int openedDocControlSelectedIndex = mainWindow.openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = mainWindow.openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox docInputBox = openedDocControlSelectedItemContent.inputBox;
            // TextBox docInputBox = mainWindow.inputBox;
            string docInputBoxContent = docInputBox.Text;
            inputBox.Text = docInputBoxContent;
        }

        public void CopyOutputHandler (object sender, RoutedEventArgs e)
        {
            CopyOutput();
        }

        public void CopyOutput ()
        {
            outputBox.Copy();
        }

        public void OpenAsNewDocHandler (object sender, RoutedEventArgs e)
        {
            OpenAsNewDoc();
        }

        public void OpenAsNewDoc ()
        {

            int openedDocControlSelectedIndex = mainWindow.openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = mainWindow.openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));

            string outputBoxContent = outputBox.Text;
            mainWindow.CreateDoc();

            TextBox docInputBox = openedDocControlSelectedItemContent.inputBox;
            // TextBox docInputBox = mainWindow.inputBox;


            docInputBox.Text = outputBoxContent;
        }


    }

    public class TranslateResponseInfo
    {
        public string status;
        public string result;
    }

}
