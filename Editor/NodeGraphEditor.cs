using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace com.enemyhideout.noonien.editor
{
  public class NodeGraphEditor : EditorWindow
  {
    [SerializeField]
    protected VisualTreeAsset uxml;
    
    [SerializeField] public string _selectionPath;

    public static NodeGraphEditor instance;

    [MenuItem("Tools/Soong/Node Editor")]
    public static void ShowMyEditor()
    {
      // This method is called when the user selects the menu item in the Editor
      NodeGraphEditor wnd = GetWindow<NodeGraphEditor>();
      wnd.titleContent = new GUIContent("Node Editor");
      instance = wnd;
    }

    private static NodeManager _manager;
    private static Node _root = null;
    public static Node Root
    {
      get
      {
        return _root;
      }
      set
      {
        _root = value;
        _manager = new NodeManager(_root);
        if (instance != null)
        {
          instance.InitializeSelection();
        }
      }
    }

    private void InitializeSelection()
    {
      if (_root == null)
      {
        return;  // not initialized yet.
      }

      var initialSelection = _root;
      if (!string.IsNullOrEmpty(_selectionPath))
      {
        var node = _manager.Find(_selectionPath);
        if (node != null)
        {
          initialSelection = node;
        }
      }
      CurrentSelection = initialSelection;
      
    }

    private Node _currentSelection;

    public Node CurrentSelection
    {
      get => _currentSelection;
      set
      {
        _currentSelection = value;
        UpdateSelection();
      }
    }
    
    private Vector2 _scrollPosition;
    private IMGUIContainer _nodeInfo;
    
    private ListView _siblingsView;
    private List<Node> _siblingsList;

    private ListView _parentsView;
    private List<Node> _parentsList;

    private ListView _childrenView;
    private List<Node> _childrenList;

    private TextField _selectionPathField;

    private void CreateGUI()
    {
      instance = this;
      uxml.CloneTree(rootVisualElement);
      InitializeListView("Siblings", OnSiblingSelectionChange, ((element, i) => OnBindItem(element, i, _siblingsList)), rootVisualElement, ref _siblingsView);
      InitializeListView("Parent", OnSiblingSelectionChange, ((element, i) => OnBindItem(element, i, _parentsList)), rootVisualElement, ref _parentsView);
      InitializeListView("Children", OnSiblingSelectionChange, ((element, i) => OnBindItem(element, i, _childrenList)), rootVisualElement, ref _childrenView);
      
      _selectionPathField = rootVisualElement.Q<TextField>("SelectionPath");
      _selectionPathField.RegisterCallback<ChangeEvent<string>>(OnSelectionPathChanged);
      InitializeSelection();

      _nodeInfo = rootVisualElement.Q<IMGUIContainer>();
      _nodeInfo.onGUIHandler = OnNodeOnGUI;
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
      var node = _manager.Find(evt.newValue);
      if (node != null)
      {
        CurrentSelection = node;
      }
    }

    private void OnBindItem(VisualElement element, int index, List<Node> dataSource)
    {
      (element as Label).text = dataSource[index].Name;
    }

    private void UpdateSelection()
    {
      Node siblingSelection = null;
      Node _parentSelection = null;
      Node _childSelection = null;
      if (_currentSelection != null)
      {
        siblingSelection = _currentSelection;
        if (_currentSelection.Parent == null)
        {
          //root of hierarchy.
          _siblingsList =  new List<Node> { _currentSelection };
          _parentsList = new List<Node>();
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
            _parentsList = new List<Node> { _currentSelection.Parent };
          }
        }

        if (_currentSelection.ChildrenCount > 0)
        {
          _childrenList = _currentSelection.Children.ToList();
        }
        else
        {
          _childrenList = new List<Node>();
        }

        _parentsView.itemsSource = UseEmptyIfNull(_parentsList);
        _childrenView.itemsSource = UseEmptyIfNull(_childrenList);
        _siblingsView.itemsSource = UseEmptyIfNull(_siblingsList);
        SetSelectedIndex(_siblingsView, siblingSelection, _siblingsList);
        SetSelectedIndex(_childrenView, _childSelection, _childrenList);
        SetSelectedIndex(_parentsView, _parentSelection, _parentsList);

        _selectionPath = _currentSelection.GetPath();
        _selectionPathField.value = _selectionPath;
      }

    }

    private static List<Node> Empty = new List<Node>();

    private static List<Node> UseEmptyIfNull(List<Node> list)
    {
      return list == null ? Empty : list;
    }

    private static void SetSelectedIndex(ListView listView, Node item, List<Node> dataSource)
    {
      int index = -1;
      if (item != null)
      {
        index = dataSource.IndexOf(item);
        listView.SetSelectionWithoutNotify(new int[]{ index });
      }

    }
    
    private void OnNodeOnGUI()
    {
      if (_currentSelection != null)
      {
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        NodeEditorCore.EditorForNode(_currentSelection, null);
        GUILayout.EndScrollView();
      }
    }

    private void OnSiblingSelectionChange(IEnumerable<object> selectedItems)
    {
      CurrentSelection = selectedItems.Cast<Node>().FirstOrDefault();
    }

    public void OnInspectorUpdate()
    {
      Repaint();
    }


  }
}