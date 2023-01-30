using com.enemyhideout.soong;

namespace Code.Data
{
  public class PlayerElement : DataElement
  {
    private int _lives = 3;
    public int Lives
    {
      get
      {
        return _lives;
      }
      set
      {
        SetProperty(value, ref _lives);
      }
    }

  }
}