using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Xml;
using Wpf_XMLEditor.Model;

namespace Wpf_XMLEditor.ViewModel
{
    public class Tab : INotifyPropertyChanged
    {
        public ObservableCollection<File> FilesList { get; } = new ObservableCollection<File>();
        private int selectedTab = -1;
        public File selectedFile;

        public RelayCommand OpenCommand { get; }
        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand SaveAsCommand { get; }
        public RelayCommand ExitCommand { get; }

        public Tab()
        {
            OpenCommand = new RelayCommand(Open_OnExecuted);
            SaveCommand = new RelayCommand(Save_OnExecuted, Save_OnCanExecute);
            SaveAsCommand = new RelayCommand(SaveAs_OnExecuted, SaveAs_OnCanExecute);
            ExitCommand = new RelayCommand(Exit_OnExecuted);
            CloseCommand = new RelayCommand(Close_OnExecuted);
        }

        public int SelectedTab
        {
            get { return selectedTab; }
            set
            {
                if (selectedTab == value)
                    return;

                selectedTab = value;
                OnPropertyChanged("SelectedTab");
                OnPropertyChanged("SelectedFile");
            }
        }

        public File SelectedFile =>
            (SelectedTab != -1) ? FilesList[SelectedTab] : null;

        public void AddFile(File newFile)
        {
            FilesList.Add(newFile);
            SelectedTab = FilesList.IndexOf(newFile);
        }

        public void SelectOldFile(string path)
        {
            if (CheckFile(path))
            {
                File oldFile = FilesList.First(file => file.FilePath.Equals(path));
                SelectedTab = FilesList.IndexOf(oldFile);
            }                
        }

        public bool CheckFile(string path)
        {
            return FilesList.Any(file => file.FilePath.Equals(path));
        }


        public void CloseTab()
        {
            if (SelectedTab == -1)
                return;

            FilesList.RemoveAt(SelectedTab);
            if (FilesList.Count == 0)
                SelectedTab = -1;
        }

        public void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedFile == null)
                return;

            SelectedFile.SelectedValue =
                e.NewValue as Methods;
        }

        public bool CloseAllTab()
        {
            SelectedTab = 0;
            while (FilesList.Count > 0)
            {
                if (CanCloseFile(SelectedFile))
                    CloseTab();
                else
                {
                    return false;
                }
            }
            return true;
        }

        private void Open_OnExecuted(object sender)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "XML-file|*.xml";

            bool? result = OpenFileDialog.ShowDialog();
            if (result == null || !result.Value)
                return;

            string path = OpenFileDialog.FileNames[0];
            if (CheckFile(path))
            {
                SelectOldFile(path);
                return;
            }

            try
            {
                File newFile = new File(FileInformation.GetFileInformation(path));
                AddFile(newFile);
            }
            catch (XmlException)
            {
                MessageBox.Show(string.Format("Невозможно загрузить файл \"{0}\".", path), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Close_OnExecuted(object sender)
        {
            if (CanCloseFile(SelectedFile))
                CloseTab();
        }

        public static bool CanCloseFile(File file)
        {
            if (file.IsSave)
                return true;

            MessageBoxResult answer = MessageBox.Show(string.Format("Файл \"{0}\" не сохранен.\nХотели бы вы сохранить данный фай до закрытия?", file.FilePath),
                "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

            bool result = false;
            switch (answer)
            {
                case MessageBoxResult.No:
                    result = true;
                    break;

                case MessageBoxResult.Yes:
                    file.Save();
                    result = true;
                    break;
            }
            return result;
        }


        private bool SaveAs_OnCanExecute(object sender)
        {
            return (FilesList.Count > 0);
        }

        private void SaveAs_OnExecuted(object sender)
        {
            SaveFileDialog SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.Filter = "XML-file|*.xml";

            bool? result = SaveFileDialog.ShowDialog();
            if (result == null || !result.Value)
                return;

            string path = SaveFileDialog.FileNames[0];
            try
            {
                SelectedFile.SaveAs(path);
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("Can't save file \"{0}\".", path),"Warning", MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }


        private bool Save_OnCanExecute(object sender)
        {
            return (FilesList.Count > 0)
                && (!SelectedFile.IsSave);  
        }

        private void Save_OnExecuted(object sender)
        {
            SelectedFile.Save();
        }


        private void Exit_OnExecuted(object sender)
        {
            Application.Current.MainWindow.Close();
        }


        private void FileTabsWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = !CloseAllTab();
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

}

