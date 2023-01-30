using Code.Data;
using com.enemyhideout.noonien;
using TMPro;
using UnityEngine;

namespace Code.Views
{
  public class GameOverUI : ElementObserver<World>
  {
    [SerializeField]
    private GameObject _root;

    [SerializeField] private TMP_Text _label;
    
    protected override void DataUpdated(World element)
    {
      base.DataUpdated(element);
      _root.SetActive(element.GameOver);
      if (element.GameOver)
      {
        _label.text = $"Game Over!\n\n{element.Winner.Name} Won!";
      }
    }
  }
}