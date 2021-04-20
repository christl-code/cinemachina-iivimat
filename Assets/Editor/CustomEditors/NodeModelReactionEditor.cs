using UnityEditor;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// 
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(NodeModelReaction))]
    public class NodeModelReactionEditor : Editor
    {
        SerializedObject so;

        SerializedProperty reaction;

        public void OnEnable()
        {
            so = serializedObject;
            reaction = so.FindProperty("reaction");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            GUILayout.BeginVertical(EditorStyles.helpBox);
            Editor actionEditor = Editor.CreateEditor(reaction.objectReferenceValue);
            actionEditor.OnInspectorGUI();
            GUILayout.EndVertical();
        }
    }
}