using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace WpWinNl.Utilities
{
  public class ResettableObservableCollection<T> : ObservableCollection<T>
  {
    public ResettableObservableCollection()
    {
    }

    public ResettableObservableCollection(List<T> list)
      : base(list)
    {
    }

    public ResettableObservableCollection(IEnumerable<T> list)
      : base(list)
    {
    }

    public void ForceReset()
    {
      OnCollectionChanged(
       new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
  }
}