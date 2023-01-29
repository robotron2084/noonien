using System;
using System.Collections.Generic;
using com.enemyhideout.soong;
using UnityEngine;

namespace Tests.Runtime
{
  public class ChildCollectionTester : CollectionObserver
  {
    [SerializeField]
    public GameObject _prefab;
    
    [NonSerialized]
    public List<HealthObserver> Children = new List<HealthObserver>();

    protected override void Awake()
    {
      base.Awake();
    }

    protected override void CollectionUpdated(IReadOnlyCollection<CollectionChange<DataEntity>> collectionChanges)
    {
      base.CollectionUpdated(collectionChanges);
      foreach (var collectionChange in collectionChanges)
      {
        var child = Instantiate(_prefab, transform);
        var source = child.GetComponent<EntitySource>();
        source.Entity = collectionChange.Item;
        Children.Add(child.GetComponent<HealthObserver>());
      }
    }
  }
}