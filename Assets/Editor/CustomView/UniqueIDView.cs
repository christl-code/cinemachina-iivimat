using UnityEditor;

namespace iivimat
{
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