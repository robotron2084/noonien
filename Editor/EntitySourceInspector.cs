using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace com.enemyhideout.soong.editor
{
  
  [CustomEditor(typeof(EntitySource), true)]
  [CanEditMultipleObjects]
  public class EntitySourceInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      if (Application.isPlaying)
      {
        foreach (var obj in serializedObject.targetObjects)
        {
          EntitySource source = obj as EntitySource;
          if (source != null)
          {
            if (source.Entity != null)
            {
              EditorForEntity(source.Entity);
            }
            else
            {
              GUILayout.Box($"No Entity");
            }
          }
        }
        
      }
      base.OnInspectorGUI();
    }

    private static void EditorForEntity(DataEntity entity)
    {
      GUILayout.Box($"Entity : {entity.Name}");

      foreach (var element in entity.Elements)
      {
        EditorForElement(element);
      }
    }

    private static Dictionary<Type, Action<PropertyInfo, DataElement>> _propertyDelegates = new Dictionary<Type, Action<PropertyInfo, DataElement>>()
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

    private static void ShowProperty<T>(PropertyInfo propertyInfo, DataElement element, Func<T,T> showEditor)
    {
      EditorGUI.BeginChangeCheck();
      T value = (T)propertyInfo.GetValue(element);
      T newValue = showEditor(value);
      if (EditorGUI.EndChangeCheck())
      {
        propertyInfo.SetValue(element, newValue);
      }
    }
    
    private static void StringProperty(PropertyInfo propertyInfo, DataElement element)
    {
      ShowProperty<string>(propertyInfo, element, (x) => EditorGUILayout.TextField(propertyInfo.Name, x));
    }

    private static void Vector2Property(PropertyInfo propertyInfo, DataElement element)
    {
      ShowProperty<Vector2>(propertyInfo, element, (x) => EditorGUILayout.Vector2Field(propertyInfo.Name, x));
    }

    private static void Vector3Property(PropertyInfo propertyInfo, DataElement element)
    {
      ShowProperty<Vector3>(propertyInfo, element, (x) => EditorGUILayout.Vector3Field(propertyInfo.Name, x));
    }
    
    private static void Vector4Property(PropertyInfo propertyInfo, DataElement element)
    {
      ShowProperty<Vector4>(propertyInfo, element, (x) => EditorGUILayout.Vector4Field(propertyInfo.Name, x));
    }

    private static void RectProperty(PropertyInfo propertyInfo, DataElement element)
    {
      ShowProperty<Rect>(propertyInfo, element, (x) => EditorGUILayout.RectField(propertyInfo.Name, x));
    }

    private static void IntProperty(PropertyInfo propertyInfo, DataElement element)
    {
      ShowProperty<int>(propertyInfo, element, (x) => EditorGUILayout.IntField(propertyInfo.Name, x));
    }

    private static void FloatProperty(PropertyInfo propertyInfo, DataElement element)
    {
      ShowProperty<float>(propertyInfo, element, (x) => EditorGUILayout.FloatField(propertyInfo.Name, x));
    }

    private static void BoolProperty(PropertyInfo propertyInfo, DataElement element)
    {
      ShowProperty<bool>(propertyInfo, element, (x) => EditorGUILayout.Toggle(propertyInfo.Name, x));
    }

    private static void EditorForElement(DataElement element)
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