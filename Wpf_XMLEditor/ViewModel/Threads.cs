using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Wpf_XMLEditor.Model;

namespace Wpf_XMLEditor.ViewModel
{
    public class Threads : INotifyPropertyChanged, ITime
    {
        private readonly Thread thread;
        public ObservableCollection<Methods> MethodsList { get; }

        public int Id
        {
            get { return thread.Id; }
            set
            {
                if (thread.Id == value)
                    return;

                thread.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public ulong Time
        {
            get { return thread.Time; }
            set
            {
                if (thread.Time == value)
                    return;

                thread.Time = value;
                OnPropertyChanged("Time");
            }
        }

        public Threads(Thread threadModel)
        {
            thread = threadModel;
            MethodsList = new ObservableCollection<Methods>();

            foreach (var method in threadModel.Methods)
            {
                var newMethod = new Methods(method, this);
                MethodsList.Add(newMethod);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

