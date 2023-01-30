using Code.Data;
using com.enemyhideout.noonien;
using TMPro;
using UnityEngine;

namespace Code.Views
{
  public class LivesUI : ElementObserver<Player>
  {
    [SerializeField]
    private TMP_Text _label;
    
    protected override void DataUpdated(Player element)
    {
      base.DataUpdated(element);
      _label.text = $"{element.Parent.Name} = {element.Lives}";
    }
  }
}