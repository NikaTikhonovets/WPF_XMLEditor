using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf_XMLEditor.Model;

namespace Wpf_XMLEditor.ViewModel
{
    public class Methods : INotifyPropertyChanged
    {
        private readonly Method method;
        public ObservableCollection<Methods> MethodsList { get; }

        public object Parent { get; private set; }

        public string Name
        {
            get { return method.Name; }
            set
            {
                if (method.Name == value)
                    return;

                method.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Package
        {
            get { return method.Package; }
            set
            {
                if (method.Package == value)
                    return;

                method.Package = value;
                OnPropertyChanged("Package");
            }
        }

        public int ParamsCount
        {
            get { return method.ParamsCount; }
            set
            {
                if (method.ParamsCount == value)
                    return;

                method.ParamsCount = value;
                OnPropertyChanged("ParamsCount");
            }
        }

        public int Time
        {
            get { return method.Time; }
            set
            {
                if (method.Time == value)
                    return;

                method.Time = value;
                OnPropertyChanged("Time");
            }
        }


        public Methods(Method method, object parent)
        {
            this.method = method;
            Parent = parent;
            MethodsList = new ObservableCollection<Methods>();

            foreach (var childMethod in method.ChildMethods)
            {
                var newMethod = new Methods(childMethod, this);
                MethodsList.Add(newMethod);
            }
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

        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            if (sender is TreeViewItem)
            {
                if (!((TreeViewItem)sender).IsSelected)
                {
                    return;
                }
            }
            MessageBox.Show(((TreeViewItem)sender).IsSelected.ToString());

        }
    }
}
