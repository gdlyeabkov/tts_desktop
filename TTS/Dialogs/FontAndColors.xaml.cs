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

        public MainWindow mainWindow;

        public FontAndColors (MainWindow mainWindow)
        {
            InitializeComponent();

            Init(mainWindow);
        
        }

        public void Init (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            TabControl openedDocControl = mainWindow.openedDocControl;
            ItemCollection openedDocControlItems = openedDocControl.Items;
            int openedDocControlItemsCount = openedDocControlItems.Count;
            bool isHaveDocs = openedDocControlItemsCount >= 1;
            if (isHaveDocs)
            {
                object rawOpenedDocControlItem = openedDocControlItems[0];
                TabItem openedDocControlItem = ((TabItem)(rawOpenedDocControlItem));
                object rawOpenedDocControlItemContent = openedDocControlItem.Content;
                Controls.OpenedDocControl openedDocControlItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlItemContent));
                TextBox inputBox = openedDocControlItemContent.inputBox;
                Brush textColor = inputBox.Foreground;
                Brush backgroundColor = inputBox.Background;
                Brush selectionColor = inputBox.SelectionBrush;
                string hexColor = textColor.ToString();
                object rawColor = ColorConverter.ConvertFromString(hexColor);
                textColorPicker.SelectedColor =  ((Color)(rawColor));
                hexColor = backgroundColor.ToString();
                rawColor = ColorConverter.ConvertFromString(hexColor);
                backgroundColorPicker.SelectedColor = ((Color)(rawColor));
                glowColorPicker.SelectedColor = System.Windows.Media.Brushes.Transparent.Color;
                hexColor = selectionColor.ToString();
                rawColor = ColorConverter.ConvertFromString(hexColor);
                selectionColorPicker.SelectedColor = ((Color)(rawColor));
                selectionTextColorPicker.SelectedColor = System.Windows.Media.Brushes.White.Color;
            }
        }

        private void ToggleTextColorHandler (object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color? possibleColor = e.NewValue;
            Color color = possibleColor.Value;
            string rawColor = color.ToString();
            BrushConverter converter = new BrushConverter();
            object rawBrush = converter.ConvertFromString(rawColor);
            Brush brush = ((Brush)(rawBrush));
            ToggleSelectionTextColor(brush);
        }

        public void ToggleTextColor (Brush brush)
        {
            textColorLabel.Foreground = brush;
        }

        private void ToggleBackgroundColorHandler (object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color? possibleColor = e.NewValue;
            Color color = possibleColor.Value;
            string rawColor = color.ToString();
            BrushConverter converter = new BrushConverter();
            object rawBrush = converter.ConvertFromString(rawColor);
            Brush brush = ((Brush)(rawBrush));
            ToggleBackgroundColor(brush);
        }

        public void ToggleBackgroundColor (Brush brush)
        {
            
        }

        private void ToggleGlowColorHandler (object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color? possibleColor = e.NewValue;
            Color color = possibleColor.Value;
            string rawColor = color.ToString();
            BrushConverter converter = new BrushConverter();
            object rawBrush = converter.ConvertFromString(rawColor);
            Brush brush = ((Brush)(rawBrush));
            ToggleGlowColor(brush);
        }

        public void ToggleGlowColor(Brush brush)
        {
            textColorLabel.Foreground = brush;
        }

        private void ToggleSelectionColorHandler (object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color? possibleColor = e.NewValue;
            Color color = possibleColor.Value;
            string rawColor = color.ToString();
            BrushConverter converter = new BrushConverter();
            object rawBrush = converter.ConvertFromString(rawColor);
            Brush brush = ((Brush)(rawBrush));
            ToggleSelectionColor(brush);
        }

        public void ToggleSelectionColor (Brush brush)
        {
            selectionTextColorLabel.Background = brush;
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
            selectionTextColorLabel.Foreground = brush;
        }

        private void ToggleLineHeightHandler (object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Xceed.Wpf.Toolkit.IntegerUpDown control = ((Xceed.Wpf.Toolkit.IntegerUpDown)(sender));
            int? possibleValue = control.Value;
            bool isValueExists = possibleValue != null;
            if (isValueExists)
            {
                int value = possibleValue.Value;
                bool isHaveValue = value >= 1;
                if (isHaveValue)
                {
                    ToggleLineHeight(value);
                }
            }
        }

        private void ToggleLineHeight (int value)
        {
            textColorLabel.LineHeight = value;
            glowColorLabel.LineHeight = value;
            selectionColorLabel.LineHeight = value;
            selectionTextColorLabel.LineHeight = value;
        }

        public void IncreaseFontSizeHandler (object sender, RoutedEventArgs e)
        {
            IncreaseFontSize();
        }

        public void IncreaseFontSize ()
        {
            textColorLabel.FontSize++;
            glowColorLabel.FontSize++;
            selectionColorLabel.FontSize++;
            selectionTextColorLabel.FontSize++;
        }

        public void DecreaseFontSizeHandler (object sender, RoutedEventArgs e)
        {
            DecreaseFontSize();
        }

        public void DecreaseFontSize ()
        {
            double fontSize = textColorLabel.FontSize;
            bool isCanDecrease = fontSize > 8;
            if (isCanDecrease)
            {
                textColorLabel.FontSize--;
                glowColorLabel.FontSize--;
                selectionColorLabel.FontSize--;
                selectionTextColorLabel.FontSize--;
            }
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

        public void SetDefaultColorsHandler (object sender, RoutedEventArgs e)
        {
            SetDefaultColors();
        }

        public void SetDefaultColors ()
        {
            textColorPicker.SelectedColor = System.Windows.Media.Brushes.Black.Color;
            backgroundColorPicker.SelectedColor = System.Windows.Media.Brushes.Transparent.Color;
            glowColorPicker.SelectedColor = System.Windows.Media.Brushes.Transparent.Color;
            selectionColorPicker.SelectedColor = System.Windows.Media.Brushes.SkyBlue.Color;
            selectionTextColorPicker.SelectedColor = System.Windows.Media.Brushes.White.Color;
        }

    }
}
