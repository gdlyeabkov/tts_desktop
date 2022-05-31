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
    /// Логика взаимодействия для SkinsDialog.xaml
    /// </summary>
    public partial class SkinsDialog : Window
    {

        public MainWindow mainWindow;

        public SkinsDialog(MainWindow mainWindow)
        {
            InitializeComponent();
        
            Init(mainWindow);
        
        }

        public void Init(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private void SelectSkinHandler (object sender, MouseButtonEventArgs e)
        {
            StackPanel skin = ((StackPanel)(sender));
            SelectSkin(skin);
        }

        public void SelectSkin (StackPanel skin)
        {
            foreach (StackPanel someSkin in skins.Children)
            {
                someSkin.Background = System.Windows.Media.Brushes.Transparent;
            }
            skin.Background = System.Windows.Media.Brushes.SkyBlue;
            object skinData = skin.DataContext;
            string skineName = skinData.ToString();
            skins.DataContext = skineName;
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
            var mainSkinBrush = mainWindow.mainSkinBrush;
            object skinsData = skins.DataContext;
            string activeSkin = skinsData.ToString();
            bool isNone = activeSkin == "none";
            bool isBlueGauze = activeSkin == "BlueGauze";
            bool isNeutral = activeSkin == "Neutral";
            bool isSapphire = activeSkin == "Sapphire";
            bool isVienna = activeSkin == "Vienna";
            bool isWLM = activeSkin == "WLM";
            if (isNone)
            {
                mainSkinBrush.Color = System.Windows.Media.Brushes.Red.Color;
            }
            else if (isBlueGauze)
            {
                mainSkinBrush.Color = System.Windows.Media.Brushes.Green.Color;
            }
            else if (isNeutral)
            {
                mainSkinBrush.Color = System.Windows.Media.Brushes.Blue.Color;
            }
            else if (isSapphire)
            {
                mainSkinBrush.Color = System.Windows.Media.Brushes.Cyan.Color;
            }
            else if (isVienna)
            {
                mainSkinBrush.Color = System.Windows.Media.Brushes.Pink.Color;
            }
            else if (isWLM)
            {
                mainSkinBrush.Color = System.Windows.Media.Brushes.Gray.Color;
            }
            Cancel();
        }

    }
}
