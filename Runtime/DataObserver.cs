using com.enemyhideout.soong;
using UnityEngine;

namespace DefaultNamespace
{
  public class DataObserver : MonoBehaviour
  {
    
  }

  public class DataObserver<T> : DataObserver where T : DataElement
  {
    protected T instance;

    protected virtual void DataUpdated(T instance)
    {
      
    }
    
  }
}