using System;

namespace com.enemyhideout.soong
{
  public class DataElementObserver<TElementSubClass> : IDataObserver<DataElement> where TElementSubClass: DataElement
  {
    private int _cachedVersion;
    private Action<TElementSubClass> _callback;
    public DataElementObserver(Action<TElementSubClass> callback)
    {
      _callback = callback;
    }

    public TElementSubClass Element { get; set; }

    public void DataUpdated(DataElement instance)
    {
      if (_cachedVersion < instance.Version)
      {
        _cachedVersion = instance.Version;
        _callback((TElementSubClass)instance);
      }
    }

    public void RemoveObserver()
    {
      Element.RemoveObserver(this);
      Element = null;
    }

    public void AddObserver(TElementSubClass element)
    {
      if (element != null)
      {
        Element = element;
        Element.AddObserver(this);
      }
      // todo: hmm...what should happen here if this is already set?
      
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