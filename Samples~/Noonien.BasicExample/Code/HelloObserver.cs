using com.enemyhideout.noonien;
using UnityEngine;

namespace Noonien.BasicExample.Code
{
  /// <summary>
  /// An ElementObserver is a MonoBehaviour that listens for changes to a
  /// Element. It works alongside NodeProvider, which is a source for a
  /// Element: when something sets the <see cref="NodeProvider.Node"/>
  /// property then all ElementObservers under it in the hierarchy are notified
  /// and can effectively update.
  /// </summary>
  public class HelloObserver : ElementObserver<HelloElement>
  {
    protected override void DataUpdated(HelloElement element)
    {
      base.DataUpdated(element);

      // In this case when the data updates, we simply log hello and the latest count.
      Debug.Log($"Hello! {element.Count}");
    }
  }
}