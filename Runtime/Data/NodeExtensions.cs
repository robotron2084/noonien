using System.Collections.Generic;
using System.Text;

namespace com.enemyhideout.noonien
{
  public static class NodeExtensions
  {

    public static List<Node> NormalizeEntities(this Node entity)
    {
      List<Node> normalizedList = new List<Node>();
      NormalizeEntities(normalizedList, entity);
      return normalizedList;
    }

    private static void NormalizeEntities(List<Node> normalizedList, Node entity)
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

    public static Node AddNewChild(this Node entity, string name)
    {
      var child = new Node(entity.NotifyManager, name, entity);
      return child;
    }

    public static string GetPath(this Node entity)
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