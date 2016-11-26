using Wpf_XMLEditor.Model;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Wpf_XMLEditor.ViewModel
{
    public class File : INotifyPropertyChanged
    {
        private readonly FileInformation file;
        public ObservableCollection<Threads> Threads { get; }


        public string FilePath
        {
            get { return file.FilePath; }
            private set
            {
                if (file.FilePath == value)
                    return;

                file.FilePath = value;
                OnPropertyChanged("FilePath");
                OnPropertyChanged("FileName");
            }
        }
        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(FilePath);
            }
        }
        public bool IsSave
        {
            get { return file.IsSave; }
            set
            {
                if (file.IsSave == value)
                    return;

                file.IsSave = value;
                OnPropertyChanged("IsSave");
            }
        }


        public void SaveAs(string path)
        {
            FilePath = path;
            file.SaveFile();
            OnPropertyChanged("IsSave");
        }

        public void Save()
        {
            file.SaveFile();
            OnPropertyChanged("IsSave");
        }


        public File(FileInformation file)
        {
            this.file = file;
            Threads = new ObservableCollection<Threads>();

            foreach (var thread in file.Threads)
            {
                var t = new Threads(thread);
                Threads.Add(t);
            }

        }

        public void ChangeFile()
        {
            IsSave = false;
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

