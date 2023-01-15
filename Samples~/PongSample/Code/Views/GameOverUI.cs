using Code.Data;
using com.enemyhideout.soong;
using TMPro;
using UnityEngine;

namespace Code.Views
{
  public class GameOverUI : ElementObserver<World>
  {
    [SerializeField]
    private GameObject _root;

    [SerializeField] private TMP_Text _label;
    
    protected override void DataUpdated(World instance)
    {
      base.DataUpdated(instance);
      _root.SetActive(instance.GameOver);
      if (instance.GameOver)
      {
        _label.text = $"Game Over!\n\n{instance.Winner.Name} Won!";
      }
    }
  }
}