using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    /// <summary>
    /// Defines appearance of the node of "HandGestureRight"
    /// Owns a specific list of possible gestures for right ahnd
    /// </summary>
    public class NodeViewHandGestureRight : NodeViewAction
    {
        SerializedObject so;

        SerializedProperty propHandGesture;

        PropertyField fieldHandGesture;
        public NodeViewHandGestureRight(NodeModelAction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Action);

            propHandGesture = so.FindProperty("currentGesture");

            fieldHandGesture = new PropertyField(propHandGesture, "Hand gesture");
            fieldHandGesture.Bind(so);

            inputContainer.RemoveAt(0);
            extensionContainer.Add(fieldHandGesture);
        }
    }
}