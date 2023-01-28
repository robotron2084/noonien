namespace com.enemyhideout.soong
{

  public class CollectionElementBase : DataElement
  {
    protected ICollection<DataEntity> _collection;

    public ICollection<DataEntity> Collection
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

    public CollectionElementBase(DataEntity parent, ICollection<DataEntity> collection) : base(parent)
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
    public VirtualCollectionElement(DataEntity parent, ICollection<DataEntity> collection) : base(parent, collection) { }
  }
}