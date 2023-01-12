using System;

namespace com.enemyhideout.soong
{
  public interface IDataObserver<T>
  {
    void DataUpdated(T instance);
  }

  public class DataObserver<TElementSubClass> : IDataObserver<DataElement> where TElementSubClass: DataElement
  {
    private Action<TElementSubClass> _callback;
    public DataObserver(Action<TElementSubClass> callback)
    {
      _callback = callback;
    }

    public TElementSubClass Element { get; set; }

    public void DataUpdated(DataElement instance)
    {
      _callback((TElementSubClass)instance);
    }

    public void RemoveObserver(DataObserver<TElementSubClass> dataObserver)
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