using System.Collections.ObjectModel;
using System.ComponentModel;
using Wpf_XMLEditor.Model;
namespace Wpf_XMLEditor.ViewModel
{
    public class Methods : INotifyPropertyChanged , ITime
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

                int difference = value - method.Time;

                ITime parentTime = Parent as ITime;
                if (parentTime != null)
                {
                    int newTime = parentTime.Time + difference;
                    if (newTime < 0)
                    {
                        return;
                    }
                    parentTime.Time = (int)newTime;
                }

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

    }
}
