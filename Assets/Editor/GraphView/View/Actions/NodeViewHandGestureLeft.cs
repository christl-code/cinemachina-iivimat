using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    /// <summary>
    /// Defines appearance of the node of "HandGestureLeft"
    /// Owns a specific list of possible gestures for left ahnd
    /// </summary>
    public class NodeViewHandGestureLeft : NodeViewAction
    {
        SerializedObject so;

        SerializedProperty propHandGesture;

        PropertyField fieldHandGesture;
        public NodeViewHandGestureLeft(NodeModelAction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Action);

            propHandGesture = so.FindProperty("currentGesture");

            fieldHandGesture = new PropertyField(propHandGesture, "HandGestureOptions");
            fieldHandGesture.Bind(so);

            inputContainer.RemoveAt(0);
            extensionContainer.Add(fieldHandGesture);
        }
    }
}