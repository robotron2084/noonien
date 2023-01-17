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
              EntityEditorCore.EditorForEntity(source.Entity);
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

  }
}