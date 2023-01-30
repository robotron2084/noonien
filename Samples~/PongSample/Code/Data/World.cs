using com.enemyhideout.soong;
using UnityEngine;

namespace Code.Data
{
  public class World : DataElement
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

    private DataEntity _winner;
    public DataEntity Winner
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