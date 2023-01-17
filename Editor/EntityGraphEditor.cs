using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace com.enemyhideout.soong.editor
{
  public class EntityGraphEditor : EditorWindow
  {
    [SerializeField]
    protected VisualTreeAsset uxml;

    public static EntityGraphEditor instance;

    [MenuItem("Tools/Soong/Entity Editor")]
    public static void ShowMyEditor()
    {
      // This method is called when the user selects the menu item in the Editor
      EntityGraphEditor wnd = GetWindow<EntityGraphEditor>();
      wnd.titleContent = new GUIContent("Entities");
      instance = wnd;
    }

    private static List<DataEntity> TestEntities = new List<DataEntity>
    {
      new DataEntity(null),
      new DataEntity(null),
      new DataEntity(null),
      new DataEntity(null),
      new DataEntity(null),
    };

    private List<DataEntity> _graph = null;
    public List<DataEntity> Graph
    {
      get
      {
        return _graph;
      }
      set
      {
        _graph = value;
        _listView.itemsSource = _graph;
      }
    }

    private IMGUIContainer _entityInfo;

    private ListView _listView;
    private void CreateGUI()
    {
      instance = this;
      uxml.CloneTree(rootVisualElement);
      _listView = rootVisualElement.Q<ListView>();
      _entityInfo = rootVisualElement.Q<IMGUIContainer>();
      _entityInfo.onGUIHandler = OnEntityOnGUI;
      Graph = TestEntities;
      _listView.makeItem = () => new Label();
      _listView.bindItem = ((element, i) => (element as Label).text = _graph[i].Name);
      _listView.onSelectionChange += OnEntitySelectionChange;
    }

    private IEnumerable<DataEntity> _selection;

    private void OnEntityOnGUI()
    {
      if (_selection != null)
      {
        foreach (var dataEntity in _selection)
        {
          EntityEditorCore.EditorForEntity(dataEntity);
        }
      }
    }

    private void OnEntitySelectionChange(IEnumerable<object> selectedItems)
    {
      _selection = selectedItems.Cast<DataEntity>().ToList();
    }
  }
}