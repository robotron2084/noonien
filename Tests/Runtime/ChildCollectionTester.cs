using System.Collections.Generic;
using com.enemyhideout.soong;
using UnityEngine;

namespace Tests.Runtime
{
  public class ChildCollectionTester : CollectionObserver
  {
    private GameObject _prefab;

    protected override void Awake()
    {
      base.Awake();
      _prefab = Resources.Load<GameObject>("ChildPrefabTest");
    }

    protected override void CollectionUpdated(IReadOnlyCollection<CollectionChange<DataEntity>> collectionChanges)
    {
      base.CollectionUpdated(collectionChanges);
      foreach (var collectionChange in collectionChanges)
      {
        var child = Instantiate(_prefab, transform);
        var source = child.GetComponent<EntitySource>();
        source.Entity = collectionChange.Item;
      }
    }
  }
}