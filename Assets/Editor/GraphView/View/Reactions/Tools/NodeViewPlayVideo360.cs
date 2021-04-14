using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewPlayVideo360 : NodeViewPlayVideo
    {
        SerializedProperty propVideoFixeToHead;
        SerializedProperty propBackToInitialPosition;

        PropertyField fieldVideoFixeToHead;
        PropertyField fieldBackToInitialPosition;

        public NodeViewPlayVideo360(NodeModelReaction nodeModel) : base(nodeModel)
        {
            propVideoFixeToHead = so.FindProperty("fixeVideoToHead");
            propBackToInitialPosition = so.FindProperty("videoBackToInitialPosition");

            fieldVideoFixeToHead = new PropertyField(propVideoFixeToHead, "Fixed to the head");
            fieldBackToInitialPosition = new PropertyField(propBackToInitialPosition, "Back to initial position");
            
            fieldVideoFixeToHead.Bind(so);
            fieldBackToInitialPosition.Bind(so);

            extensionContainer.Add(fieldVideoFixeToHead);
            extensionContainer.Add(fieldBackToInitialPosition);
        }
    }
}