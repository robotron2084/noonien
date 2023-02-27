using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace com.enemyhideout.noonien.editor
{
  public static class NodeEditorCore
  {
    
    public static void EditorForNode(Node node, Action OnNodeChanged)
    {

      PropertyInfo nameProp = typeof(Node).GetProperty(nameof(Node.Name));
      StringProperty(nameProp, node, OnNodeChanged);
      foreach (var element in node.Elements)
      {
        EditorForElement(element, OnNodeChanged);
      }
    }

    private static Dictionary<Type, Action<PropertyInfo, Element, Action>> _propertyDelegates = new Dictionary<Type, Action<PropertyInfo, Element, Action>>()
    {
      {typeof(bool),  BoolProperty},
      {typeof(string),  StringProperty},
      {typeof(int),  IntProperty},
      {typeof(float),  FloatProperty},
      {typeof(Vector2),  Vector2Property},
      {typeof(Vector3),  Vector3Property},
      {typeof(Vector4),  Vector4Property},
      {typeof(Rect),  RectProperty},
    };

    private static void ShowProperty<T>(PropertyInfo propertyInfo, object element, Func<T,T> showEditor, Action onPropertyChanged)
    {
      EditorGUI.BeginChangeCheck();
      T value = (T)propertyInfo.GetValue(element);
      T newValue = showEditor(value);
      if (EditorGUI.EndChangeCheck())
      {
        propertyInfo.SetValue(element, newValue);
        onPropertyChanged?.Invoke();
      }
    }
    
    private static void StringProperty(PropertyInfo propertyInfo, object element, Action onPropertyChanged)
    {
      ShowProperty<string>(propertyInfo, element, (x) => EditorGUILayout.TextField(propertyInfo.Name, x), onPropertyChanged);
    }

    private static void Vector2Property(PropertyInfo propertyInfo, object element, Action onPropertyChanged)
    {
      ShowProperty<Vector2>(propertyInfo, element, (x) => EditorGUILayout.Vector2Field(propertyInfo.Name, x), onPropertyChanged);
    }

    private static void Vector3Property(PropertyInfo propertyInfo, object element, Action onPropertyChanged)
    {
      ShowProperty<Vector3>(propertyInfo, element, (x) => EditorGUILayout.Vector3Field(propertyInfo.Name, x), onPropertyChanged);
    }
    
    private static void Vector4Property(PropertyInfo propertyInfo, object element, Action onPropertyChanged)
    {
      ShowProperty<Vector4>(propertyInfo, element, (x) => EditorGUILayout.Vector4Field(propertyInfo.Name, x), onPropertyChanged);
    }

    private static void RectProperty(PropertyInfo propertyInfo, object element, Action onPropertyChanged)
    {
      ShowProperty<Rect>(propertyInfo, element, (x) => EditorGUILayout.RectField(propertyInfo.Name, x), onPropertyChanged);
    }

    private static void IntProperty(PropertyInfo propertyInfo, object element, Action onPropertyChanged)
    {
      ShowProperty<int>(propertyInfo, element, (x) => EditorGUILayout.IntField(propertyInfo.Name, x), onPropertyChanged);
    }

    private static void FloatProperty(PropertyInfo propertyInfo, object element, Action onPropertyChanged)
    {
      ShowProperty<float>(propertyInfo, element, (x) => EditorGUILayout.FloatField(propertyInfo.Name, x), onPropertyChanged);
    }

    private static void BoolProperty(PropertyInfo propertyInfo, object element, Action onPropertyChanged)
    {
      ShowProperty<bool>(propertyInfo, element, (x) => EditorGUILayout.Toggle(propertyInfo.Name, x), onPropertyChanged);
    }

    private static void EditorForElement(Element element, Action onElementChanged)
    {
      
      PropertyInfo[] properties = element.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
      bool showFoldout = EditorPrefs.GetBool(element.GetType().Name, false);
      EditorGUI.BeginChangeCheck();
      showFoldout = EditorGUILayout.Foldout(showFoldout, element.GetType().Name);
      if (EditorGUI.EndChangeCheck())
      {
        EditorPrefs.SetBool(element.GetType().Name, showFoldout);
      }

      if (showFoldout)
      {
        EditorGUI.indentLevel++;
        foreach (var propertyInfo in properties)
        {
          if (propertyInfo.CanRead)
          {
            if (propertyInfo.CanWrite)
            {
              if (_propertyDelegates.TryGetValue(propertyInfo.PropertyType, out var del))
              {
                del(propertyInfo, element, onElementChanged);
              }
              else
              {
                object val = propertyInfo.GetValue(element);
                EditorGUILayout.LabelField($"{propertyInfo.Name} : '{val}'");
              }
            }
          }
        }

        EditorGUI.indentLevel--;
      }
    }
  }
  
  
}