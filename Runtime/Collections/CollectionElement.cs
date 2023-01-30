using System;

namespace com.enemyhideout.soong
{

  public class CollectionElement : DataElement
  {
    public override DataEntity Parent
    {
      get => _parent;
      set
      {
        InitializeParent(value);
        Collection = _parent.Children;
      }
    }

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

  }

  
}