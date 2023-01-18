using System.Collections;
using System.Collections.Generic;

namespace com.enemyhideout.soong
{

  public class EntityCollection : IEntityCollection
  {

    protected List<DataEntity> _children = new List<DataEntity>();
    protected INotifyManager _notifyManager;
    protected Observable<IEntityCollection> _observable;

    public EntityCollection(INotifyManager notifyManager)
    {
      _notifyManager = notifyManager;
      _observable = new Observable<IEntityCollection>(this, _notifyManager);
    }

    public void InsertChild(int index, DataEntity entity)
    {
      _children.Insert(index, entity);
      MarkDirty();
    }

    public void AddChild(DataEntity entity)
    {
      _children.Add(entity);
      MarkDirty();
    }

    public void RemoveChild(DataEntity dataEntity)
    {
      _children.Remove(dataEntity);
      MarkDirty();
    }
    
    public void RemoveObserver(IDataObserver<IEntityCollection> element)
    {
      _observable.RemoveObserver(element);
    }

    public void AddObserver(IDataObserver<IEntityCollection> element)
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

    public void MarkDirty()
    {
      _observable.MarkDirty();
    }

  }

  public interface IEntityCollection : IEnumerable<DataEntity>
  {
    DataEntity this[int index]
    {
      get;
    }

    int Count { get; }

    void RemoveObserver(IDataObserver<IEntityCollection> element);

    void AddObserver(IDataObserver<IEntityCollection> element);

  }
}