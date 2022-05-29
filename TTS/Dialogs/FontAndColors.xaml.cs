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
    /// Логика взаимодействия для FontAndColors.xaml
    /// </summary>
    public partial class FontAndColors : Window
    {
        public FontAndColors()
        {
            InitializeComponent();
        }

        private void ToggleSelectionTextColorHandler (object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color? possibleColor = e.NewValue;
            Color color = possibleColor.Value;
            string rawColor = color.ToString();
            BrushConverter converter = new BrushConverter();
            object rawBrush = converter.ConvertFromString(rawColor);
            Brush brush = ((Brush)(rawBrush));
            ToggleSelectionTextColor(brush);
        }

        public void ToggleSelectionTextColor (Brush brush)
        {
            textColorLabel.Foreground = brush;
        }

    }
}
