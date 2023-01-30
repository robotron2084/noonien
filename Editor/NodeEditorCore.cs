using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace com.enemyhideout.noonien.editor
{
  public static class NodeEditorCore
  {
    
    public static void EditorForNode(Node node)
    {
      GUILayout.Box($"Node : {node.Name}");

      foreach (var element in node.Elements)
      {
        EditorForElement(element);
      }
    }

    private static Dictionary<Type, Action<PropertyInfo, Element>> _propertyDelegates = new Dictionary<Type, Action<PropertyInfo, Element>>()
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

    private static void ShowProperty<T>(PropertyInfo propertyInfo, Element element, Func<T,T> showEditor)
    {
      EditorGUI.BeginChangeCheck();
      T value = (T)propertyInfo.GetValue(element);
      T newValue = showEditor(value);
      if (EditorGUI.EndChangeCheck())
      {
        propertyInfo.SetValue(element, newValue);
      }
    }
    
    private static void StringProperty(PropertyInfo propertyInfo, Element element)
    {
      ShowProperty<string>(propertyInfo, element, (x) => EditorGUILayout.TextField(propertyInfo.Name, x));
    }

    private static void Vector2Property(PropertyInfo propertyInfo, Element element)
    {
      ShowProperty<Vector2>(propertyInfo, element, (x) => EditorGUILayout.Vector2Field(propertyInfo.Name, x));
    }

    private static void Vector3Property(PropertyInfo propertyInfo, Element element)
    {
      ShowProperty<Vector3>(propertyInfo, element, (x) => EditorGUILayout.Vector3Field(propertyInfo.Name, x));
    }
    
    private static void Vector4Property(PropertyInfo propertyInfo, Element element)
    {
      ShowProperty<Vector4>(propertyInfo, element, (x) => EditorGUILayout.Vector4Field(propertyInfo.Name, x));
    }

    private static void RectProperty(PropertyInfo propertyInfo, Element element)
    {
      ShowProperty<Rect>(propertyInfo, element, (x) => EditorGUILayout.RectField(propertyInfo.Name, x));
    }

    private static void IntProperty(PropertyInfo propertyInfo, Element element)
    {
      ShowProperty<int>(propertyInfo, element, (x) => EditorGUILayout.IntField(propertyInfo.Name, x));
    }

    private static void FloatProperty(PropertyInfo propertyInfo, Element element)
    {
      ShowProperty<float>(propertyInfo, element, (x) => EditorGUILayout.FloatField(propertyInfo.Name, x));
    }

    private static void BoolProperty(PropertyInfo propertyInfo, Element element)
    {
      ShowProperty<bool>(propertyInfo, element, (x) => EditorGUILayout.Toggle(propertyInfo.Name, x));
    }

    private static void EditorForElement(Element element)
    {
      
      PropertyInfo[] properties = element.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
      bool showFoldout = EditorPrefs.GetBool(element.GetType().ToString(), false);
      EditorGUI.BeginChangeCheck();
      showFoldout = EditorGUILayout.Foldout(showFoldout, element.GetType().ToString());
      if (EditorGUI.EndChangeCheck())
      {
        EditorPrefs.SetBool(element.GetType().ToString(), showFoldout);
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
                del(propertyInfo, element);
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