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
    /// Логика взаимодействия для ReplaceDialog.xaml
    /// </summary>
    public partial class ReplaceDialog : Window
    {
        public ReplaceDialog()
        {
            InitializeComponent();
        }

        private void DetectFromBoxHandler (object sender, TextChangedEventArgs e)
        {
            DetectFromBox();
        }

        public void DetectFromBox ()
        {
            string fromBoxContent = fromBox.Text;
            int fromBoxContentLength = fromBoxContent.Length;
            bool isHaveContent = fromBoxContentLength >= 1;
            findBtn.IsEnabled = isHaveContent;
            replaceBtn.IsEnabled = isHaveContent;
            replaceAllBtn.IsEnabled = isHaveContent;
        }

        public void CancelHandler(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel()
        {
            this.Close();
        }

        public void ReplaceHandler(object sender, RoutedEventArgs e)
        {
            Replace();
        }

        public void Replace()
        {

        }


    }
}
