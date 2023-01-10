namespace com.enemyhideout.soong
{
  public class DataElement
  {

    public DataModel _parent;

    public DataModel Parent
    {
      get => _parent;
      set => _parent = value;
    }

    public string Name
    {
      get
      {
        return $"{_parent.Name}.{GetType()}";
      }
    }

    public void MarkDirty()
    {
      
    }

    public void Observe()
    {
      
    }
  }
}