using Code.Data;
using com.enemyhideout.soong;
using TMPro;
using UnityEngine;

namespace Code.Views
{
  public class LivesUI : ElementObserver<PlayerElement>
  {
    [SerializeField]
    private TMP_Text _label;
    
    protected override void DataUpdated(PlayerElement instance)
    {
      base.DataUpdated(instance);
      _label.text = $"{instance.Parent.Name} = {instance.Lives}";
    }
  }
}