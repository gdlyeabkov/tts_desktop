using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TTS.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для BtnsDialog.xaml
    /// </summary>
    public partial class BtnsDialog : Window
    {

        public MainWindow mainWindow;

        public BtnsDialog(MainWindow mainWindow)
        {
            InitializeComponent();

            Init(mainWindow);

        }

        public void Init (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void OkHandler(object sender, RoutedEventArgs e)
        {
            Ok();
        }

        public void Ok()
        {
            object rawIsChecked = newDocCheckBox.IsChecked;
            bool isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.newDocShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.newDocShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = openDocCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.openDocShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.openDocShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = saveDocCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.saveDocShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.saveDocShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = saveAudioCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.saveAudioShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.saveAudioShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = saveMultipleAudioCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.saveMultipleAudioShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.saveMultipleAudioShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = speakCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.speakShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.speakShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = pauseCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.pauseShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.pauseShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = stopCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.stopShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.stopShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = speakSelectedTextCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.speakSelectedTextShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.speakSelectedTextShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = speakTextFromBufferCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.speakTextFromBufferShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.speakTextFromBufferShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = voiceParamsCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.voiceParamsShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.voiceParamsShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = dictPanelCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.dictPanelShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.dictPanelShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = insertFastBookmarkCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.insertFastBookmarkShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.insertFastBookmarkShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = goToFastBookmarkCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.goToFastBookmarkShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.goToFastBookmarkShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = insertNamedBookmarkCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.insertNamedBookmarkShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.insertNamedBookmarkShortcutBtn.Visibility = Visibility.Collapsed;
            }
            rawIsChecked = helpCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.helpShortcutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.helpShortcutBtn.Visibility = Visibility.Collapsed;
            }
            double size = 24;
            rawIsChecked = smallBtnsRadioBtn.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                size = 28;
            }
            else
            {
                size = 48;
            }
            ToolBarPanel toolBar = mainWindow.toolBar;
            UIElementCollection toolBarChildren = toolBar.Children;
            foreach (ToolBar toolBarChild in toolBarChildren)
            {
                ItemCollection toolBarChildItems = toolBarChild.Items;
                int toolBarChildItemsCount = toolBarChildItems.Count;
                bool isHaveItems = toolBarChildItemsCount >= 1;
                if (isHaveItems)
                {
                    var toolBarChildItem = toolBarChildItems[0];
                    Button btn = ((Button)(toolBarChildItem));
                    btn.Width = size;
                    btn.Height = size;
                }
            }
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

        public void MarkAllHandler(object sender, RoutedEventArgs e)
        {
            MarkAll();
        }

        public void MarkAll()
        {
            /*newDocCheckBox.IsChecked = true;
            openDocCheckBox.IsChecked = true;*/
            UIElementCollection checkBoxes = btns.Children;
            foreach (CheckBox checkBox in checkBoxes)
            {
                checkBox.IsChecked = true;
            }
        }

        public void ResetAllHandler(object sender, RoutedEventArgs e)
        {
            ResetAll();
        }

        public void ResetAll()
        {
            /*newDocCheckBox.IsChecked = false;
            openDocCheckBox.IsChecked = false;*/
            UIElementCollection checkBoxes = btns.Children;
            foreach (CheckBox checkBox in checkBoxes)
            {
                checkBox.IsChecked = false;
            }
        }

        public void ResetToDefaultHandler(object sender, RoutedEventArgs e)
        {
            ResetToDefault();
        }

        public void ResetToDefault ()
        {
            ResetAll();
            newDocCheckBox.IsChecked = true;
            openDocCheckBox.IsChecked = true;
            saveDocCheckBox.IsChecked = true;
            saveAudioCheckBox.IsChecked = true;
            saveMultipleAudioCheckBox.IsChecked = true;
            speakCheckBox.IsChecked = true;
            pauseCheckBox.IsChecked = true;
            stopCheckBox.IsChecked = true;
            speakSelectedTextCheckBox.IsChecked = true;
            speakTextFromBufferCheckBox.IsChecked = true;
            voiceParamsCheckBox.IsChecked = true;
            dictPanelCheckBox.IsChecked = true;
            insertFastBookmarkCheckBox.IsChecked = true;
            goToFastBookmarkCheckBox.IsChecked = true;
            insertNamedBookmarkCheckBox.IsChecked = true;
            helpCheckBox.IsChecked = true;
        }

    }
}
