using System.Collections;
using System.Collections.Generic;
using com.enemyhideout.noonien;
using Noonien.BasicExample.Code;
using UnityEngine;

/// <summary>
/// A simple example of the basics of Soong. This shows how you:
///   * Create data in a DataElement and attach it to a DataEntity.
///   * Observe that data and do something with it on a GameObject.
/// </summary>
public class HelloController : MonoBehaviour
{
  /// <summary>
  /// An EntitySource. An entity source binds a GameObject hierarchy to a DataEntity hierarchy.
  /// </summary>
  public NodeProvider Source;

  /// <summary>
  /// The HelloElement extends the DataElement class. A DataElement is a component of data that
  /// exists on a DataEntity.
  /// </summary>
  private HelloElement _element;
  
  void Start()
  {
    // The NotifyManager is a kind of event system that is used by data components to notify 
    // their observers that their data has changed.
    var notifyManager = GetComponent<NotifyManager>();
    
    // A data entity is a simple container that contains data.
    var dataEntity = new Node(notifyManager, "My Entity");
    // This data entity has one element, which contains a Count property that we can update.
    _element = dataEntity.AddElement<HelloElement>();

    // An EntitySource is how we connect the data entity to a game object, so that
    // observers on a monobehaviour can view it.
    Source.Node = dataEntity;

    StartCoroutine(SayHello());
  }

  /// <summary>
  /// Every second we update the count propert of the HelloElement. Any observers
  /// of this element will be notified to update. For more info check out <see cref="HelloObserver" />
  /// </summary>
  /// <returns></returns>
  IEnumerator SayHello()
  {
    while (true)
    {
      yield return new WaitForSeconds(1.0f);
      _element.Count++;
    }
  }
  
}
