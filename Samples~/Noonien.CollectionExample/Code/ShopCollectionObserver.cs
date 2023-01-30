using System.Collections.Generic;
using com.enemyhideout.noonien;
using UnityEngine;

namespace Soong.CollectionExample.Code
{
  public class ShopCollectionObserver : CollectionObserver
  {
    protected override void CollectionUpdated(IReadOnlyCollection<CollectionChange<Node>> collectionChanges)
    {
      base.CollectionUpdated(collectionChanges);
      // when changes to the collection occcur, the collection observer will recieve
      // CollectionChange objects, which we can then use to display info, add/remove/move
      // prefabs, or other things.
      foreach (var collectionChange in collectionChanges)
      {
        var shopItem = collectionChange.Item.GetElement<ShopCollectionExample.ShopItem>();
        Debug.Log($"Shop Item Added: {collectionChange.Item.Name} ${shopItem.Price}.");
      }
    }
  }
}