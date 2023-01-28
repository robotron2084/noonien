using System;
using System.Collections;
using System.Collections.Generic;

namespace com.enemyhideout.soong
{
  public class FilteredEntityCollection : EntityCollection, IDisposable
  {
    private Func<ICollection<DataEntity>, IEnumerable<DataEntity>> _filter;

    private ICollection<DataEntity> _source;
    private DataObserver<ICollection<DataEntity>> _observer;
      
    public FilteredEntityCollection(Func<ICollection<DataEntity>, IEnumerable<DataEntity>> filter, ICollection<DataEntity> source, INotifyManager notifyManager) : base(notifyManager)
    {
      _filter = filter;
      _source = source;
      _observer = new DataObserver<ICollection<DataEntity>>(OnCollectionChanged);
      _source.AddObserver(_observer);
      OnCollectionChanged(_source);
    }

    private void OnCollectionChanged(ICollection<DataEntity> entityCollection)
    {
      _children.Clear();
      _children.AddRange(_filter(entityCollection));
      MarkDirty();
    }

    public void Dispose()
    {
      if (_source != null)
      {
        _source.RemoveObserver(_observer);
      }
    }
  }
}