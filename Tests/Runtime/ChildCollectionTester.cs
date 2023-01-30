using System;
using System.Collections.Generic;
using com.enemyhideout.noonien;
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

    protected override void CollectionUpdated(IReadOnlyCollection<CollectionChange<Node>> collectionChanges)
    {
      base.CollectionUpdated(collectionChanges);
      foreach (var collectionChange in collectionChanges)
      {
        var child = Instantiate(_prefab, transform);
        var source = child.GetComponent<NodeProvider>();
        source.Node = collectionChange.Item;
        Children.Add(child.GetComponent<HealthObserver>());
      }
    }
  }
}