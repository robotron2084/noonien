using com.enemyhideout.soong;
using UnityEngine;

namespace Soong.BasicExample.Code
{
  /// <summary>
  /// An ElementObserver is a MonoBehaviour that listens for changes to a
  /// DataElement. It works alongside EntitySource, which is a source for a
  /// DataElement: when something sets the <see cref="EntitySource.Entity"/>
  /// property then all ElementObservers under it in the hierarchy are notified
  /// and can effectively update.
  /// </summary>
  public class HelloObserver : ElementObserver<HelloElement>
  {
    protected override void DataUpdated(HelloElement instance)
    {
      base.DataUpdated(instance);

      // In this case when the data updates, we simply log hello and the latest count.
      Debug.Log($"Hello! {instance.Count}");
    }
  }
}