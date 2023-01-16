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
              TypeCode typeCode = Type.GetTypeCode(propertyInfo.PropertyType);
              EditorGUI.BeginChangeCheck();
              switch (typeCode)
              {
                case TypeCode.Boolean:
                  bool value = (bool)propertyInfo.GetValue(element);
                  bool newValue = EditorGUILayout.Toggle(propertyInfo.Name,value);
                  if (EditorGUI.EndChangeCheck())
                  {
                    propertyInfo.SetValue(element, newValue);
                  }
                  break;
              
                default:
                  // just show it.
                  object val = propertyInfo.GetValue(element);
                  EditorGUILayout.LabelField($"{propertyInfo.Name} : '{val}'");
                  break;
              }
            }
          }
        }

        EditorGUI.indentLevel--;
      }
    }
  }
  
  
}