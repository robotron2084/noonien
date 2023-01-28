using System.Collections.Generic;

namespace com.enemyhideout.soong
{
  public interface ICollection<T> : IEnumerable<T>,IObservable<ICollection<T>>
  {
    T this[int index]
    {
      get;
    }

    int Count { get; }

    IReadOnlyCollection<CollectionChange<T>> GetChanges();
  }
}