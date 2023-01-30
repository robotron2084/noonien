using com.enemyhideout.noonien;
using UnityEngine;

namespace Code.Data
{
  public class World : Element
  {

    private bool _gameOver = false;
    public bool GameOver
    {
      get
      {
        return _gameOver;
      }
      set
      {
        SetProperty(value, ref _gameOver);
      }
    }

    private Node _winner;
    public Node Winner
    {
      get
      {
        return _winner;
      }
      set
      {
        SetProperty(value, ref _winner);
      }
    }
    
    
    private Rect _bounds;
    public Rect Bounds
    {
      get
      {
        return _bounds;
      }
      set
      {
        SetProperty(value, ref _bounds);
      }
    }
  }
}