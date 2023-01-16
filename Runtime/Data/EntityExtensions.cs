namespace com.enemyhideout.soong
{
  public static class EntityExtensions
  {
    public static DataEntity AddNewChild(this DataEntity entity, string name)
    {
      var child = new DataEntity(entity.NotifyManager, name, entity);
      entity.AddChild(child);
      return child;
    }
  }
}