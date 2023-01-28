using System;

namespace com.enemyhideout.soong
{
  
  public class VersionedDataObserver<T> : IDataObserver<T> where T : IObservable<T>
  {
    private int _cachedVersion;
    
    public VersionedDataObserver(Action<T> callback, int startingVersion)
    {
      _cachedVersion = startingVersion;
      _callback = callback;
    }
    
    public Action<T> _callback;
    public void DataUpdated(T instance)
    {
      if (_cachedVersion < instance.Version)
      {
        _cachedVersion = instance.Version;
        _callback(instance);
      }
    }
  }

  public class DataObserver<T> : IDataObserver<T>
  {
    
    public DataObserver(Action<T> callback)
    {
      _callback = callback;
    }
    
    public Action<T> _callback;
    public void DataUpdated(T instance)
    {
      _callback(instance);
    }
  }
}