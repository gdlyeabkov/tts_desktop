using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Логика взаимодействия для GoToBookmarkDialog.xaml
    /// </summary>
    public partial class GoToBookmarkDialog : Window
    {

        public MainWindow mainWindow;
        public int selectedBookmarkIndex = 0;

        public GoToBookmarkDialog(MainWindow mainWindow)
        {
            InitializeComponent();

            Init(mainWindow);
        
        }

        public void Init (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            GetBookmarks();
        }

        public void GetBookmarksHandler(object sender, EventArgs e)
        {
            GetBookmarks();
        }

        public void GetBookmarks ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> currentBookmarks = loadedContent.bookmarks; 

            RowDefinitionCollection rows = bookmarks.RowDefinitions;
            int rowsCount = rows.Count;
            bool isHavePreviousData = rowsCount >= 2;
            if (isHavePreviousData)
            {
                int countRemovedRows = rowsCount - 1;
                bookmarks.RowDefinitions.RemoveRange(1, countRemovedRows);
            }
            UIElementCollection recentLoginHistoryLogsChildren = bookmarks.Children;
            int recentLoginHistoryLogsChildrenCount = recentLoginHistoryLogsChildren.Count;
            isHavePreviousData = recentLoginHistoryLogsChildrenCount >= 4;
            isHavePreviousData = rowsCount >= 2;
            if (isHavePreviousData)
            {
                int countRemovedChildren = recentLoginHistoryLogsChildrenCount - 3;
                bookmarks.Children.RemoveRange(3, countRemovedChildren);
            }
            foreach (Dictionary<String, Object> currentBookmark in currentBookmarks)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(30);
                bookmarks.RowDefinitions.Add(row);
                rows = bookmarks.RowDefinitions;
                rowsCount = rows.Count;
                int lastRowIndex = rowsCount - 1;
                TextBlock bookmarkNameLabel = new TextBlock();
                string currentBookmarkName = ((string)(currentBookmark["name"]));
                bookmarkNameLabel.Text = currentBookmarkName;

                bookmarkNameLabel.Margin = new Thickness(0, 5, 0, 5);
                bookmarks.Children.Add(bookmarkNameLabel);
                Grid.SetRow(bookmarkNameLabel, lastRowIndex);
                Grid.SetColumn(bookmarkNameLabel, 0);
                TextBlock bookmarkTextLabel = new TextBlock();
                bookmarkTextLabel.Text = "";
                bookmarkTextLabel.Margin = new Thickness(0, 5, 0, 5);
                bookmarks.Children.Add(bookmarkTextLabel);
                Grid.SetRow(bookmarkTextLabel, lastRowIndex);
                Grid.SetColumn(bookmarkTextLabel, 1);
                TextBlock bookmarkPercentLabel = new TextBlock();
                bookmarkPercentLabel.Text = "100";
                bookmarkPercentLabel.Margin = new Thickness(0, 5, 0, 5);
                bookmarks.Children.Add(bookmarkPercentLabel);
                Grid.SetRow(bookmarkPercentLabel, lastRowIndex);
                Grid.SetColumn(bookmarkPercentLabel, 2);
                bookmarkNameLabel.MouseLeftButtonUp += SelectBookmarkHandler;
                bookmarkTextLabel.MouseLeftButtonUp += SelectBookmarkHandler;
                bookmarkPercentLabel.MouseLeftButtonUp += SelectBookmarkHandler;
                int bookmarkIndex = currentBookmarks.IndexOf(currentBookmark);
                bool isFirstBookmark = bookmarkIndex == 0;
                if (isFirstBookmark)
                {
                    SelectBookmark(bookmarkNameLabel);
                }
            }
        }

        public void RemoveAllBookmarksHandler(object sender, RoutedEventArgs e)
        {
            RemoveAllBookmarks();
        }

        public void RemoveAllBookmarks ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> updatedBookmarks = loadedContent.bookmarks;

            updatedBookmarks.Clear();
            string savedContent = js.Serialize(new SavedContent
            {
                bookmarks = updatedBookmarks
            });
            File.WriteAllText(saveDataFilePath, savedContent);
            Cancel();
        }

        public void RemoveBookmarkHandler(object sender, RoutedEventArgs e)
        {
            RemoveBookmark();
        }

        public void RemoveBookmark ()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> updatedBookmarks = loadedContent.bookmarks;

            updatedBookmarks.RemoveAt(selectedBookmarkIndex);
            string savedContent = js.Serialize(new SavedContent
            {
                bookmarks = updatedBookmarks
            });
            File.WriteAllText(saveDataFilePath, savedContent);
            int updatedBookmarksCount = updatedBookmarks.Count;
            bool isNotBookmarks = updatedBookmarksCount <= 0;
            if (isNotBookmarks)
            {
                Cancel();
            }
            else
            {
                GetBookmarks();
            }
        }

        public void CancelHandler (object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public void Cancel ()
        {
            this.Close();
        }

        public void SelectBookmarkHandler (object sender, RoutedEventArgs e)
        {
            TextBlock label = ((TextBlock)(sender));
            SelectBookmark(label);
        }

        public void SelectBookmark (TextBlock label)
        {
            int rowIndex = Grid.GetRow(label);
            foreach (TextBlock bookmarksItem in bookmarks.Children)
            {
                int localRowIndex = Grid.GetRow(bookmarksItem);
                bool isSelected = rowIndex == localRowIndex;
                if (isSelected)
                {
                    bookmarksItem.Background = System.Windows.Media.Brushes.SkyBlue;
                }
                else
                {
                    bookmarksItem.Background = System.Windows.Media.Brushes.Transparent;
                }
            }
            selectedBookmarkIndex = rowIndex - 1;
        }

        public void RenameBookmarkHandler(object sender, RoutedEventArgs e)
        {
            RenameBookmark();
        }

        public void RenameBookmark()
        {
            Dialogs.RenameBookmarkDialog dialog = new Dialogs.RenameBookmarkDialog(selectedBookmarkIndex);
            dialog.Closed += GetBookmarksHandler;
            dialog.Show();
        }

        public void GoToBookmarkHandler(object sender, RoutedEventArgs e)
        {
            GoToBookmark();
        }

        public void GoToBookmark()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> currentBookmarks = loadedContent.bookmarks;
            Dictionary<String, Object>  currentBookmark = currentBookmarks[selectedBookmarkIndex];
            int index = ((int)(currentBookmark["index"]));
            int openedDocControlSelectedIndex = mainWindow.openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = mainWindow.openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            inputBox.SelectionStart = index;
            Cancel();
        }

        public void RemoveBookmarkWithTextHandler(object sender, RoutedEventArgs e)
        {
            RemoveBookmarkWithText();
        }

        public void RemoveBookmarkWithText()
        {
            Environment.SpecialFolder localApplicationDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string localApplicationDataFolderPath = Environment.GetFolderPath(localApplicationDataFolder);
            string saveDataFilePath = localApplicationDataFolderPath + @"\OfficeWare\SpeechReader\save-data.txt";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string saveDataFileContent = File.ReadAllText(saveDataFilePath);
            SavedContent loadedContent = js.Deserialize<SavedContent>(saveDataFileContent);
            List<Dictionary<String, Object>> currentBookmarks = loadedContent.bookmarks;
            Dictionary<String, Object> currentBookmark = currentBookmarks[selectedBookmarkIndex];
            int index = ((int)(currentBookmark["index"]));
            int openedDocControlSelectedIndex = mainWindow.openedDocControl.SelectedIndex;
            ItemCollection openedDocControlItems = mainWindow.openedDocControl.Items;
            object rawOpenedDocControlSelectedItem = openedDocControlItems[openedDocControlSelectedIndex];
            TabItem openedDocControlSelectedItem = ((TabItem)(rawOpenedDocControlSelectedItem));
            object rawOpenedDocControlSelectedItemContent = openedDocControlSelectedItem.Content;
            Controls.OpenedDocControl openedDocControlSelectedItemContent = ((Controls.OpenedDocControl)(rawOpenedDocControlSelectedItemContent));
            TextBox inputBox = openedDocControlSelectedItemContent.inputBox;
            string inputBoxContent = inputBox.Text;
            int inputBoxContentLength = inputBoxContent.Length;
            inputBoxContent = inputBoxContent.Remove(index, inputBoxContentLength - index);
            inputBox.Text = inputBoxContent;
            RemoveBookmark();
        }

    }
}
