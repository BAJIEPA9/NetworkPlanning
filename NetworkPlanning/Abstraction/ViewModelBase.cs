using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity;

namespace NetworkPlanning.Abstraction
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase(IUnityContainer container)
        {
            Container = container;
        }

        public IUnityContainer Container { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}