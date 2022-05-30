using AudioSwitcher.AudioApi.CoreAudio;
using NAudio.CoreAudioApi;
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
    /// Логика взаимодействия для SelectOutputDevieDialog.xaml
    /// </summary>
    public partial class SelectOutputDevieDialog : Window
    {
        public SelectOutputDevieDialog()
        {
            InitializeComponent();
        
            Init();
        
        }


        public void Init ()
        {
            MMDeviceEnumerator names = new MMDeviceEnumerator();
            MMDeviceCollection outputDevices = names.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            foreach (MMDevice device in outputDevices)
            {
                string deviceName = device.FriendlyName;
                ComboBoxItem soundOutputBoxItem = new ComboBoxItem();
                soundOutputBoxItem.Content = device;
                outputDevicesSelector.Items.Add(deviceName);
            }
            int countDevices = outputDevices.Count;
            bool isHaveDevices = countDevices >= 1;
            if (isHaveDevices)
            {
                outputDevicesSelector.SelectedIndex = 0;
            }
            /*IEnumerable<CoreAudioDevice> outputDevices = new CoreAudioController().GetPlaybackDevices().ToList<CoreAudioDevice>();
            foreach (CoreAudioDevice device in outputDevices)
            {
                string deviceName = device.FullName;
                ComboBoxItem soundOutputBoxItem = new ComboBoxItem();
                soundOutputBoxItem.Content = device;
                outputDevicesSelector.Items.Add(deviceName);
            }
            int countDevices = outputDevices.Count();
            bool isHaveDevices = countDevices >= 1;
            if (isHaveDevices)
            {
                outputDevicesSelector.SelectedIndex = 0;
            }*/
        }

        private void OkHandler (object sender, RoutedEventArgs e)
        {
            Ok();
        }

        public void Ok ()
        {
            Cancel();
        }

        private void CancelHandler (object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel ()
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
