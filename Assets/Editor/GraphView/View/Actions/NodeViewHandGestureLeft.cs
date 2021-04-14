using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
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