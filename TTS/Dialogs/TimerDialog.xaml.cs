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
    /// Логика взаимодействия для TimerDialog.xaml
    /// </summary>
    public partial class TimerDialog : Window
    {

        public MainWindow mainWindow;

        public TimerDialog(MainWindow mainWindow)
        {
            InitializeComponent();

            Init(mainWindow);
        
        }

        public void Init (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private void OkHandler (object sender, RoutedEventArgs e)
        {
            Ok();
        }

        public void Ok ()
        {
            object rawIsChecked = playSoundCheckBox.IsChecked;
            bool isPlaySound = ((bool)(rawIsChecked));
            rawIsChecked = shotAttentionCheckBox.IsChecked;
            bool isShowAttention = ((bool)(rawIsChecked));
            rawIsChecked = timerCheckBox.IsChecked;
            bool isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                string timerBoxContent = timerBox.Text;
                int minutes = Int32.Parse(timerBoxContent);
                mainWindow.StartTimer(minutes, isPlaySound, isShowAttention);
            }
            rawIsChecked = speechTimerCheckBox.IsChecked;
            isChecked = ((bool)(rawIsChecked));
            if (isChecked)
            {
                mainWindow.StartSpeechTimer(isPlaySound, isShowAttention);
            }
            Cancel();
        }

        private void CancelHandler(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel ()
        {
            this.Close();
        }

    }
}
