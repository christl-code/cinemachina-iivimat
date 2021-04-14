using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
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