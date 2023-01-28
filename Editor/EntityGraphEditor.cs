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

    private static EntityManager _manager;
    private static DataEntity _root = null;
    public static DataEntity Root
    {
      get
      {
        return _root;
      }
      set
      {
        _root = value;
        _manager = new EntityManager(_root);
        if (instance != null)
        {
          instance.CurrentSelection = _root;
        }
      }
    }

    private DataEntity _currentSelection;

    public DataEntity CurrentSelection
    {
      get => _currentSelection;
      set
      {
        _currentSelection = value;
        UpdateSelection();
      }
    }
    
    private IMGUIContainer _entityInfo;
    
    private ListView _siblingsView;
    private List<DataEntity> _siblingsList;

    private ListView _parentsView;
    private List<DataEntity> _parentsList;

    private ListView _childrenView;
    private List<DataEntity> _childrenList;

    private TextField _selectionPath;


    private void CreateGUI()
    {
      instance = this;
      uxml.CloneTree(rootVisualElement);
      InitializeListView("Siblings", OnSiblingSelectionChange, ((element, i) => OnBindItem(element, i, _siblingsList)), rootVisualElement, ref _siblingsView);
      InitializeListView("Parent", OnSiblingSelectionChange, ((element, i) => OnBindItem(element, i, _parentsList)), rootVisualElement, ref _parentsView);
      InitializeListView("Children", OnSiblingSelectionChange, ((element, i) => OnBindItem(element, i, _childrenList)), rootVisualElement, ref _childrenView);
      
      _selectionPath = rootVisualElement.Q<TextField>("SelectionPath");
      _selectionPath.RegisterCallback<ChangeEvent<string>>(OnSelectionPathChanged);
      _currentSelection = _root;
      UpdateSelection();
    }

    private static void InitializeListView(string listViewName, Action<IEnumerable<object>> selectionChange, Action<VisualElement,int> onBind, VisualElement root, ref ListView listVar)
    {
      listVar = root.Q<ListView>(listViewName);
      listVar.makeItem = () => new Label();
      listVar.bindItem = onBind;
      listVar.onSelectionChange += selectionChange;
    }

    private void OnSelectionPathChanged(ChangeEvent<string> evt)
    {
      var entity = _manager.Find(evt.newValue);
      if (entity != null)
      {
        CurrentSelection = entity;
      }
    }

    private void OnBindItem(VisualElement element, int index, List<DataEntity> dataSource)
    {
      (element as Label).text = dataSource[index].Name;
    }

    private void UpdateSelection()
    {
      DataEntity siblingSelection = null;
      DataEntity _parentSelection = null;
      DataEntity _childSelection = null;
      if (_currentSelection != null)
      {
        siblingSelection = _currentSelection;
        if (_currentSelection.Parent == null)
        {
          //root of hierarchy.
          _siblingsList =  new List<DataEntity> { _currentSelection };
          _parentsList = new List<DataEntity>();
        }
        else
        {
          _parentSelection = _currentSelection.Parent;
          // there is a parent, get siblings.
          _siblingsList = _currentSelection.Parent.Children.ToList();
          if (_currentSelection.Parent.Parent != null)
          {
            _parentsList = _currentSelection.Parent.Parent.Children.ToList();
          }
          else
          {
            _parentsList = new List<DataEntity> { _currentSelection.Parent };
          }
        }

        if (_currentSelection.ChildrenCount > 0)
        {
          _childrenList = _currentSelection.Children.ToList();
        }
        else
        {
          _childrenList = new List<DataEntity>();
        }

        _parentsView.itemsSource = _parentsList;
        _childrenView.itemsSource = _childrenList;
        _siblingsView.itemsSource = _siblingsList;
        SetSelectedIndex(_siblingsView, siblingSelection, _siblingsList);
        SetSelectedIndex(_childrenView, _childSelection, _childrenList);
        SetSelectedIndex(_parentsView, _parentSelection, _parentsList);

        _selectionPath.value = _currentSelection.GetPath();
      }

    }

    private static void SetSelectedIndex(ListView listView, DataEntity item, List<DataEntity> dataSource)
    {
      int index = -1;
      if (item != null)
      {
        index = dataSource.IndexOf(item);
      }

      listView.SetSelectionWithoutNotify(new int[]{ index });
    }
    
    private void OnEntityOnGUI()
    {
      if (_currentSelection != null)
      {
        EntityEditorCore.EditorForEntity(_currentSelection);
      }
    }

    private void OnSiblingSelectionChange(IEnumerable<object> selectedItems)
    {
      CurrentSelection = selectedItems.Cast<DataEntity>().FirstOrDefault();
    }

    public void Update()
    {
      Repaint();
    }


  }
}