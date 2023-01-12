using System.Collections;
using System.Collections.Generic;
using Codice.Client.GameUI.Checkin;

namespace com.enemyhideout.soong
{

  public class EntityCollection : IEntityCollection
  {

    private List<DataEntity> _children = new List<DataEntity>();
    private INotifyManager _notifyManager;
    private Observable<EntityCollection> _observable;

    public EntityCollection(INotifyManager notifyManager)
    {
      _notifyManager = notifyManager;
      _observable = new Observable<EntityCollection>(this, _notifyManager);
    }

    public void AddChild(DataEntity entity)
    {
      _children.Add(entity);
      _observable.MarkDirty();
    }

    public void RemoveChild(DataEntity dataEntity)
    {
      _children.Remove(dataEntity);
      _observable.MarkDirty();
    }
    
    public void RemoveObserver(IDataObserver<EntityCollection> element)
    {
      _observable.RemoveObserver(element);
    }

    public void AddObserver(IDataObserver<EntityCollection> element)
    {
      _observable.AddObserver(element);
    }
    
    public IEnumerator<DataEntity> GetEnumerator()
    {
      return _children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public DataEntity this[int index] => _children[index];
    public int Count
    {
      get => _children.Count;
    }

  }

  public interface IEntityCollection : IEnumerable<DataEntity>
  {
    DataEntity this[int index]
    {
      get;
    }

    int Count { get; }
  }
}