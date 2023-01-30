using System;
using System.Collections.Generic;

namespace com.enemyhideout.noonien
{
  public class CollectionDataObserver<T> : IDataObserver<ICollection<T>>
  {
    private Action<IReadOnlyCollection<CollectionChange<T>>> _callback;
    public CollectionDataObserver(Action<IReadOnlyCollection<CollectionChange<T>>> callback)
    {
      _callback = callback;
    }
    public ICollection<T> Element { get; set; }

    public void DataUpdated(ICollection<T> instance)
    {
      _callback(instance.GetChanges());
    }
    
    public void RemoveObserver(ICollection<T> dataElementObserver)
    {
      Element.RemoveObserver(this);
      Element = null;
    }

    public void AddObserver(ICollection<T> element)
    {
      if (element != null)
      {
        Element = element;
        Element.AddObserver(this);
      }
    }

    public void Destroy()
    {
      if (Element != null)
      {
        Element.RemoveObserver(this);
        Element = null;
      }
    }
  }
}