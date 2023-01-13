using System.Collections.Generic;
using com.enemyhideout.soong;

namespace Tests.Runtime
{
  public class CollectionCounter : CollectionObserver
  {
    public List<CollectionChange<DataEntity>> LatestChanges;
    
    public int ItemsCount;
    protected override void CollectionUpdated(List<CollectionChange<DataEntity>> collectionChanges)
    {
      base.CollectionUpdated(collectionChanges);
      ItemsCount = _element.Collection.Count;
      LatestChanges = collectionChanges;
    }
  }
}