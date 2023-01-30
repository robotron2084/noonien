using System;
using System.Collections;
using System.Collections.Generic;

namespace com.enemyhideout.noonien
{
  public class FilteredNodeCollection : NodeCollection, IDisposable
  {
    private Func<ICollection<Node>, IEnumerable<Node>> _filter;

    private ICollection<Node> _source;
    private DataObserver<ICollection<Node>> _observer;
      
    public FilteredNodeCollection(Func<ICollection<Node>, IEnumerable<Node>> filter, ICollection<Node> source, INotifyManager notifyManager) : base(notifyManager)
    {
      _filter = filter;
      _source = source;
      _observer = new DataObserver<ICollection<Node>>(OnCollectionChanged);
      _source.AddObserver(_observer);
      OnCollectionChanged(_source);
    }

    private void OnCollectionChanged(ICollection<Node> entityCollection)
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