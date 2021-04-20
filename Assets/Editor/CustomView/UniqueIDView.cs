using UnityEditor;

namespace iivimat
{
    /// <summary>
    /// Gives unique ID to each child of environment
    /// </summary>
    [CustomEditor(typeof(UniqueID))]
    public class UniqueIDView : Editor
    {
        SerializedProperty uniqueID;

        void OnEnable()
        {
            uniqueID = serializedObject.FindProperty("guid");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(uniqueID.stringValue);
        }
    }
}