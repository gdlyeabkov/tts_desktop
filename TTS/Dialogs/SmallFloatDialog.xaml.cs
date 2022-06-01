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
    /// Логика взаимодействия для SmallFloatDialog.xaml
    /// </summary>
    public partial class SmallFloatDialog : Window
    {

        public MainWindow mainWindow;

        public SmallFloatDialog(MainWindow mainWindow)
        {
            InitializeComponent();

            Init(mainWindow);
        
        }

        public void Init (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }


        private void WindowMouseEnterHandler(object sender, MouseEventArgs e)
        {
            WindowMouseEnter();
        }

        public void WindowMouseEnter()
        {
            this.Opacity = 1;
        }

        private void WindowMouseLeaveHandler(object sender, MouseEventArgs e)
        {
            WindowMouseLeave();
        }

        public void WindowMouseLeave ()
        {
            this.Opacity = 0.5;
        }

        public void SpeakHandler(object sender, RoutedEventArgs e)
        {
            Speak();
        }

        public void Speak()
        {
            mainWindow.Speak();
        }

        public void StopHandler(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        public void Stop()
        {
            mainWindow.Stop();
        }

        public void SpeakBufferHandler(object sender, RoutedEventArgs e)
        {
            SpeakBuffer();
        }

        public void SpeakBuffer()
        {
            mainWindow.SpeakBuffer();
        }

    }
}
