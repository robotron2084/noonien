using System;

namespace com.enemyhideout.noonien
{

  public class CollectionElement : Element
  {
    public override Node Parent
    {
      get => _parent;
      set
      {
        InitializeParent(value);
        Collection = _parent.Children;
      }
    }

    protected ICollection<Node> _collection;

    public ICollection<Node> Collection
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