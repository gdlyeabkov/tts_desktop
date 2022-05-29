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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TTS.Controls
{
    /// <summary>
    /// Логика взаимодействия для OpenedDocControl.xaml
    /// </summary>
    public partial class OpenedDocControl : UserControl
    {
        public OpenedDocControl()
        {
            InitializeComponent();
        }

        private void TogglePitchSliderHandler(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = ((Slider)(sender));
            TogglePitchSlider(slider);
        }

        public void TogglePitchSlider (Slider slider)
        {
            object controlData = this.DataContext;
            bool isDataExists = controlData != null;
            if (isDataExists)
            {
                MainWindow mainWindow = ((MainWindow)(controlData));
                mainWindow.TogglePitchSlider(slider);
            }
        }

        private void ToggleVolumeSliderHandler (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = ((Slider)(sender));
            ToggleVolumeSlider(slider);
        }

        public void ToggleVolumeSlider (Slider slider)
        {
            object controlData = this.DataContext;
            bool isDataExists = controlData != null;
            if (isDataExists)
            {
                MainWindow mainWindow = ((MainWindow)(controlData));
                mainWindow.ToggleVolumeSlider(slider);
            }
        }

        private void ToggleSpeedSliderHandler (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = ((Slider)(sender));
            ToggleSpeedSlider(slider);
        }

        public void ToggleSpeedSlider(Slider slider)
        {
            object controlData = this.DataContext;
            bool isDataExists = controlData != null;
            if (isDataExists)
            {
                MainWindow mainWindow = ((MainWindow)(controlData));
                mainWindow.ToggleSpeedSlider(slider);
            }
        }



    }
}
