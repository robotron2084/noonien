using System.Collections.Generic;

namespace com.enemyhideout.noonien
{
  public interface ICollection
  {
    
  }
  public interface ICollection<T> : ICollection, System.Collections.Generic.ICollection<T>, IEnumerable<T>,IObservable<ICollection<T>>
  {
    T this[int index]
    {
      get;
    }
    IReadOnlyCollection<CollectionChange<T>> GetChanges();
    
    bool HasObservers();
  }
}