using System.Diagnostics;
using com.enemyhideout.soong;
using UnityEngine;

namespace Code.Data
{
  public class Unit : DataElement
  {
    private bool _alive = true;

    public bool Alive
    {
      get
      {
        return _alive;
      }
      set
      {
        SetProperty(value, ref _alive);
      }
    }
    
    public Vector2 Position
    {
      get
      {
        return _bounds.center;
      }
      set
      {
        var rect = _bounds;
        rect.center = value;
        SetProperty(rect, ref _bounds);
      }
    }
    
    private Vector2 _velocity;
    public Vector2 Velocity
    {
      get
      {
        return _velocity;
      }
      set
      {
        SetProperty(value, ref _velocity);
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

    public Unit(Rect bounds, DataEntity parent) : base(parent)
    {
      _bounds = bounds;
    }
  }
}