using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Wpf_XMLEditor.Model;

namespace Wpf_XMLEditor.ViewModel
{
    public class Tab : INotifyPropertyChanged
    {
        public ObservableCollection<File> FilesList { get; } = new ObservableCollection<File>();
        private int selectedTab = -1;
        public File selectedFile;

        public Command OpenCommand { get; }
        public Command CloseCommand { get; }
        public Command SaveCommand { get; }
        public Command SaveAsCommand { get; }
        public Command ExitCommand { get; }
        public Command OkCommand { get; }

        public Tab()
        {
            OpenCommand = new Command(Open_OnExecuted);
            SaveCommand = new Command(Save_OnExecuted, Save_OnCanExecute);
            SaveAsCommand = new Command(SaveAs_OnExecuted, SaveAs_OnCanExecute);
            ExitCommand = new Command(Exit_OnExecuted);
            CloseCommand = new Command(Close_OnExecuted);
            OkCommand = new Command(OkCommand_OnExecute);
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


        private Methods method;
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value)
                    return;

                name = value;
                method.Name = value;
                OnPropertyChanged("Name");
            }
        }
        private string package;
        public string Package
        {
            get { return package; }
            set
            {
                if (package == value)
                    return;

                package = value;
                method.Package = value;
                OnPropertyChanged("Package");
            }
        }

        private int paramsCount;
        public int ParamsCount
        {
            get { return paramsCount; }
            set
            {
                if (paramsCount == value)
                    return;

                paramsCount = value;
                method.ParamsCount = value;
                OnPropertyChanged("ParamsCount");
            }
        }
        private int time;
        public int Time
        {
            get { return time; }
            set
            {
                if (time == value)
                    return;

                time = value;
                method.Time = value;
                OnPropertyChanged("Time");
            }
        }



        public void GetMethod(object sender)
        {
            TreeViewItem item = (TreeViewItem)sender;
            Methods node = (Methods)item.DataContext;
            method = node;
            Name = method.Name;
            Time = method.Time;
            Package = method.Package;
            ParamsCount = method.ParamsCount;

        }

        private void OkCommand_OnExecute(object sender)
        {
            method.Name = Name;
            method.Package = Package;
            method.ParamsCount = ParamsCount;
            method.Time = Time;

            OnPropertyChanged("Name");
            OnPropertyChanged("Package");
            OnPropertyChanged("ParamsCount");
            OnPropertyChanged("Time");
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

