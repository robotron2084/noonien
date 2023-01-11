using System;

namespace com.enemyhideout.soong
{
  public interface IDataObserver
  {
    void ElementUpdated(DataElement instance);
  }

  public class DataObserver<T> : IDataObserver where T: DataElement
  {
    private Action<T> _callback;
    public DataObserver(Action<T> callback)
    {
      _callback = callback;
    }

    public T Element { get; set; }

    public void ElementUpdated(DataElement instance)
    {
      _callback((T)instance);
    }

    public void RemoveObserver(DataObserver<T> dataObserver)
    {
      Element.RemoveObserver(this);
      Element = null;
    }

    public void AddObserver(T element)
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