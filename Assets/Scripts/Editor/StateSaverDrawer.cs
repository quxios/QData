using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace QData {
  [CustomEditor(typeof(StateSaver))]
  public class StateSaverDrawer : Editor {
    private SerializedProperty rememberComponents;
    private SerializedProperty uid;

    private void OnEnable() {
      rememberComponents = serializedObject.FindProperty("rememberComponents");
      uid = serializedObject.FindProperty("_UID");
    }

    public override void OnInspectorGUI() {
      serializedObject.Update();
      EditorGUILayout.PropertyField(uid, new GUIContent("UID", "Set a Unique ID for this object"));
      EditorGUILayout.LabelField("Components to Save");
      StateSaver curr = (StateSaver)serializedObject.targetObject;
      GameObject go = curr.gameObject;
      string[] components = go.GetComponents<MonoBehaviour>()
        .Where(c => c != null && c != curr)
        .Select(c => c.GetType().ToString()).ToArray();
      HashSet<string> prevSet = new HashSet<string>();
      for (int i = 0; i < rememberComponents.arraySize; i++) {
        prevSet.Add(rememberComponents.GetArrayElementAtIndex(i).stringValue);
      }
      List<string> newSet = new List<string>();
      EditorGUI.indentLevel++;
      for (int i = 0; i < components.Length; i++) {
        EditorGUILayout.BeginHorizontal();
        bool temp = EditorGUILayout.Toggle(components[i], prevSet.Contains(components[i]));
        EditorGUILayout.EndHorizontal();
        if (temp == true) {
          newSet.Add(components[i]);
        }
      }
      EditorGUI.indentLevel--;
      rememberComponents.arraySize = newSet.Count;
      for (int i = 0; i < rememberComponents.arraySize; i++) {
        rememberComponents.GetArrayElementAtIndex(i).stringValue = newSet[i];
      }
      serializedObject.ApplyModifiedProperties();
    }

  }
}
