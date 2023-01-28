namespace com.enemyhideout.soong
{
  public interface IDataObserver<T>
  {
    void DataUpdated(T instance);
  }
}