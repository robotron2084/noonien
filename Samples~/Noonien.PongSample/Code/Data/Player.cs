using com.enemyhideout.noonien;

namespace Code.Data
{
  public class Player : Element
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