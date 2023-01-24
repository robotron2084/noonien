using System.Collections;
using System.Collections.Generic;
using com.enemyhideout.soong;
using UnityEngine;


/// <summary>
/// This example illustrates the usage of EntityCollection, CollectionElement
/// and CollectionObserver. These items help work with collections of entities,
/// In this example we create a collection of shop items, then grab the first
/// few of those items and add them to an EntityCollection.
///
/// The ShopCollectionObserver exists on a separate game object and observes the
/// changes to the collection and update accordingly. 
/// </summary>
public class ShopCollectionExample : MonoBehaviour
{
    public EntitySource shopEntityListener;
    
    void Start()
    {
        var notify = GetComponent<NotifyManager>();
        var root = new DataEntity(notify, "root");
        // first: create a list of all the shop items that we may want to have.
        var allShopItems = root.AddNewChild("AllShopItems");
        
        // create a bunch of shop items in there.
        for (int i = 0; i < 100; i++)
        {
            var shopItemEntity = allShopItems.AddNewChild("Shop Item " + i);
            var shopItem = new ShopItem(shopItemEntity);
            shopItem.Price = (i+1) * 10;
        }

        // We want a separate data entity that defines our current choices for
        // the shop. These items are held in a CollectionElement on a separate 
        // entity.
        
        // first we create our entity.
        var currentShop = root.AddNewChild("Shop");
        // then we create a CollectionElement to hold our collection that we'll
        // update.
        var shopCollection = new CollectionElement(currentShop);
        // Create a collection of current shop items that we will populate.
        var shopDeals = new EntityCollection(notify);
        // And assign that collection to the CollectionElement.
        shopCollection.Collection = shopDeals;
        
        // put half the items into this collection.
        for (int i = 0; i < 5; i++)
        {
            shopDeals.AddChild(allShopItems.Children[i]);
        }

        // Set the EntitySource to the entity that contains the collection we want to observe.
        shopEntityListener.Entity = currentShop;
    }

    public class ShopItem : DataElement
    {
        private int _price;
        public int Price
        {
            get => _price;
            set => SetProperty(value, ref _price);
        }
        public ShopItem(DataEntity parent) : base(parent)
        {
        }
    }

}
