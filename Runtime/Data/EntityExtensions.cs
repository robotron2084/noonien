using System.Collections.Generic;
using System.Text;

namespace com.enemyhideout.soong
{
  public static class EntityExtensions
  {

    public static List<DataEntity> NormalizeEntities(this DataEntity entity)
    {
      List<DataEntity> normalizedList = new List<DataEntity>();
      NormalizeEntities(normalizedList, entity);
      return normalizedList;
    }

    private static void NormalizeEntities(List<DataEntity> normalizedList, DataEntity entity)
    {
      normalizedList.Add(entity);
      if (entity.ChildrenCount > 0)
      {
        foreach (var entityChild in entity.Children)
        {
          NormalizeEntities(normalizedList, entityChild);
        }
      }
    }

    public static DataEntity AddNewChild(this DataEntity entity, string name)
    {
      var child = new DataEntity(entity.NotifyManager, name, entity);
      entity.AddChild(child);
      return child;
    }

    public static string GetPath(this DataEntity entity)
    {
      StringBuilder sb = new StringBuilder();
      var item = entity;
      while (item != null)
      {
        string name = item.Name;
        item = item.Parent;
        sb.Insert(0,name);
        if (item != null)
        {
          sb.Insert(0,".");
        }
      }

      return sb.ToString();
    }
  }
}