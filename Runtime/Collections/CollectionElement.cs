namespace com.enemyhideout.soong
{

  public class CollectionElementBase : DataElement
  {
    protected IEntityCollection _collection;

    public IEntityCollection Collection
    {
      get
      {
        return _collection;
      }
      set
      {
        SetProperty(value, ref _collection);
      }
    }

    public CollectionElementBase(DataEntity parent, IEntityCollection collection) : base(parent)
    {
      Collection = collection;
    }
  }

  public class CollectionElement : CollectionElementBase
  {
    public CollectionElement(DataEntity parent) : base(parent, parent.Children) { }
  }
  
  public class VirtualCollectionElement : CollectionElementBase
  {
    public VirtualCollectionElement(DataEntity parent, IEntityCollection collection) : base(parent, collection) { }
  }
}