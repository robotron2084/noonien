using com.enemyhideout.soong;

namespace Soong.BasicExample.Code
{
  /// <summary>
  /// An example of how to create properties for a <see cref="DataElement"/>
  /// </summary>
  public class HelloElement : DataElement
  {
    private int _count;
    public int Count
    {
      get
      {
        return _count;
      }
      set
      {
        // SetProperty is a helper function which ensures that if this data changes
        // observers will be notified.
        SetProperty(value, ref _count);
      }
    }

  }
}