namespace com.enemyhideout.soong
{
  public class EntityCollection : Collection<DataEntity>
  {
    public EntityCollection(INotifyManager notifyManager) : base(notifyManager)
    {
    }
  }
}