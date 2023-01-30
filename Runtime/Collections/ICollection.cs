using System.Collections.Generic;

namespace com.enemyhideout.noonien
{
  public interface ICollection
  {
    
  }
  public interface ICollection<T> : ICollection, IEnumerable<T>,IObservable<ICollection<T>>
  {
    T this[int index]
    {
      get;
    }

    int Count { get; }

    IReadOnlyCollection<CollectionChange<T>> GetChanges();
    
    void Clear();
  }
}