namespace com.enemyhideout.noonien
{
  public interface IDataObserver<T>
  {
    void DataUpdated(T instance);
  }
}