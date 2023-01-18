using System;
using System.Collections.Generic;
using System.Linq;

namespace com.enemyhideout.soong
{
  public class CollectionDelta<T>
  {
    private List<T> _cachedItems = new List<T>();

    public List<CollectionChange<T>> ComputeChanges(List<T> updatedList)
    {
      var retVal = ComputeDelta(_cachedItems, updatedList);
      _cachedItems = updatedList.ToList();
      return retVal;
    }

    public static List<CollectionChange<T>> ComputeDelta(List<T> cachedItems, List<T> updatedList)
    {
      var retVal = new List<CollectionChange<T>>();
      Dictionary<T, int> indexLookup = new Dictionary<T, int>();
      for (var i = 0; i < cachedItems.Count; i++)
      {
        indexLookup[cachedItems[i]] = i;
      }

      for (var i = 0; i < updatedList.Count; i++)
      {
        var item = updatedList[i];
        if (indexLookup.TryGetValue(item, out int oldIndex))
        {
          //it exists
          if (oldIndex != i)
          {
            //item moved!
            var change = new CollectionChange<T>
            {
              Item = item,
              OldIndex = oldIndex,
              NewIndex = i,
              Action = CollectionChangeAction.Moved
            };
            retVal.Add(change);
          }// else same position!
          // remove item from lookup.
          indexLookup.Remove(item);
        }
        else
        {
          //item not found!
          var change = new CollectionChange<T>
          {
            Item = item,
            OldIndex = -1,
            NewIndex = i,
            Action = CollectionChangeAction.Added
          };
          retVal.Add(change);
        }
      }
      
      foreach (var kvp in indexLookup)
      {
        var change = new CollectionChange<T>
        {
          Item = kvp.Key,
          OldIndex = kvp.Value,
          NewIndex = -1,
          Action = CollectionChangeAction.Removed
        };
        retVal.Add(change);
      }

      return retVal;
    }
  }

  public enum CollectionChangeAction
  {
    Added,
    Removed,
    Moved
  }

  public class CollectionChange<T>
  {
    public override int GetHashCode()
    {
      return HashCode.Combine(Item, OldIndex, NewIndex, (int)Action);
    }

    protected bool Equals(CollectionChange<T> other)
    {
      return EqualityComparer<T>.Default.Equals(Item, other.Item) && OldIndex == other.OldIndex && NewIndex == other.NewIndex && Action == other.Action;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((CollectionChange<T>)obj);
    }

    public T Item;
    public int OldIndex;
    public int NewIndex;
    public CollectionChangeAction Action;

    public override string ToString()
    {
      return $"[CollectionChange {Item}, OldIndex={OldIndex}, NewIndex={NewIndex}, Action={Action}]";
    }
  }
}