using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace com.enemyhideout.noonien.editor
{

  [CustomEditor(typeof(NodeProvider), true)]
  [CanEditMultipleObjects]
  public class NodeProviderInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      if (Application.isPlaying)
      {
        foreach (var obj in serializedObject.targetObjects)
        {
          NodeProvider source = obj as NodeProvider;
          if (source != null)
          {
            if (source.Node != null)
            {
              NodeEditorCore.EditorForNode(source.Node, null);
            }
            else
            {
              GUILayout.Box($"No Node");
            }
          }
        }

      }

      base.OnInspectorGUI();
    }

  }
}