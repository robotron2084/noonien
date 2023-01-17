namespace com.enemyhideout.soong
{
  public class EntityManager
  {

    public DataEntity Root;

    public EntityManager(DataEntity root)
    {
      Root = root;
    }

    public DataEntity Find(string query)
    {
      return FindInternal(query, Root);
    }

    public static DataEntity FindInternal(string query, DataEntity root)
    {
      var entityTokens = query.Split(".");
      var searchItem = root;
      var tokenIndex = 0;
      int iteration = 0;
      int maxIterations = 10;
      while (true && iteration < maxIterations)
      {
        var searchString = entityTokens[tokenIndex];
        bool itemFound = false;
        foreach (var child in searchItem.Children)
        {
          if (child.Name == searchString)
          {
            searchItem = child;
            itemFound = true;
            tokenIndex++;
            if (tokenIndex == entityTokens.Length)
            {
              return searchItem;
            }
            break;
          }
        }

        if (!itemFound)
        {
          return null;
        }
        iteration++;
      }
      return null;
    }
  }
}