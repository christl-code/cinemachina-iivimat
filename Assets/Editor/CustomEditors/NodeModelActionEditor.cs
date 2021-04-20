using UnityEditor;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// 
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor (typeof(NodeModelAction))]
    public class NodeModelActionEditor : Editor
    {
        SerializedObject so;

        SerializedProperty action;

        public void OnEnable()
        {
            so = serializedObject;
            action = so.FindProperty("action");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            GUILayout.BeginVertical(EditorStyles.helpBox);
            Editor actionEditor = Editor.CreateEditor(action.objectReferenceValue);
            actionEditor.OnInspectorGUI();
            GUILayout.EndVertical();
        }
    }
}
