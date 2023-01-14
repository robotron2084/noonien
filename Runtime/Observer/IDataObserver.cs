using System;

namespace com.enemyhideout.soong
{
  public interface IDataObserver<T>
  {
    void DataUpdated(T instance);
  }

  public class DataObserver<T> : IDataObserver<T>
  {
    public DataObserver(Action<T> callback)
    {
      _callback = callback;
    }
    public Action<T> _callback;
    public void DataUpdated(T instance)
    {
      _callback(instance);
    }
  }

  public class DataElementObserver<TElementSubClass> : IDataObserver<DataElement> where TElementSubClass: DataElement
  {
    private Action<TElementSubClass> _callback;
    public DataElementObserver(Action<TElementSubClass> callback)
    {
      _callback = callback;
    }

    public TElementSubClass Element { get; set; }

    public void DataUpdated(DataElement instance)
    {
      _callback((TElementSubClass)instance);
    }

    public void RemoveObserver(DataElementObserver<TElementSubClass> dataElementObserver)
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