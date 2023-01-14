using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Utils.Buffers;

namespace com.enemyhideout.soong
{
  public class FilteredEntityCollection : EntityCollection, IDisposable
  {
    private Func<IEntityCollection, IEnumerable<DataEntity>> _filter;

    private IEntityCollection _source;
    private DataObserver<IEntityCollection> _observer;
      
    public FilteredEntityCollection(Func<IEntityCollection, IEnumerable<DataEntity>> filter, IEntityCollection source, INotifyManager notifyManager) : base(notifyManager)
    {
      _filter = filter;
      _source = source;
      _observer = new DataObserver<IEntityCollection>(OnCollectionChanged);
      _source.AddObserver(_observer);
      OnCollectionChanged(_source);
    }

    private void OnCollectionChanged(IEntityCollection entityCollection)
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