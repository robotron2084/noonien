using System;
using UnityEngine.Assertions;

namespace com.enemyhideout.soong
{
  public abstract class DataElementObserver
  {
    public abstract void EntityUpdated(DataEntity entity);
    public abstract void Destroy();
  }
  
  public class DataElementObserver<TElementSubClass> : DataElementObserver, IDataObserver<DataElement> where TElementSubClass: DataElement
  {
    private int _cachedVersion;
    private Action<TElementSubClass> _updated;
    private readonly Action<TElementSubClass> _added;
    private readonly Action<TElementSubClass> _removed;
    public TElementSubClass Element { get; set; }

    public DataElementObserver(Action<TElementSubClass> updated, Action<TElementSubClass> added=null, Action<TElementSubClass> removed=null)
    {
      _updated = updated;
      _added = added;
      _removed = removed;
      Assert.IsNotNull(_updated);
    }

    public override void EntityUpdated(DataEntity entity)
    {
      if (Element != null)
      {
        if (_removed != null)
        {
          _removed(Element);
        }
        RemoveObserver();
      }
        
      if (entity != null)
      {
        var element = entity.GetElement<TElementSubClass>();
        if (element != null)
        {
          if (_added != null)
          {
            _added(element);
          }
          AddObserver(element);
          _updated(element);
        }
      }
    }


    public void DataUpdated(DataElement instance)
    {
      if (_cachedVersion < instance.Version)
      {
        _cachedVersion = instance.Version;
        _updated((TElementSubClass)instance);
      }
    }

    private void RemoveObserver()
    {
      Element.RemoveObserver(this);
      Element = null;
      _cachedVersion = 0;
    }

    private void AddObserver(TElementSubClass element)
    {
      if (element != null)
      {
        Element = element;
        Element.AddObserver(this);
      }
      // todo: hmm...what should happen here if this is already set?
      
    }

    public override void Destroy()
    {
      if (Element != null)
      {
        RemoveObserver();
      }
    }
  }
}