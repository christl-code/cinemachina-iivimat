using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewStopVideo360 : NodeViewPlayVideo
    {
        SerializedProperty propVideoFixeToHead;
        SerializedProperty propBackToInitialPosition;

        PropertyField fieldVideoFixeToHead;
        PropertyField fieldBackToInitialPosition;

        public NodeViewStopVideo360(NodeModelReaction nodeModel) : base(nodeModel)
        {
            propVideoFixeToHead = so.FindProperty("stopFollowingHead");
            propBackToInitialPosition = so.FindProperty("goBackToPosition");

            fieldVideoFixeToHead = new PropertyField(propVideoFixeToHead, "Stop following the head");
            fieldBackToInitialPosition = new PropertyField(propBackToInitialPosition, "Back to initial position");
            
            fieldVideoFixeToHead.Bind(so);
            fieldBackToInitialPosition.Bind(so);

            extensionContainer.Add(fieldVideoFixeToHead);
            extensionContainer.Add(fieldBackToInitialPosition);
        }
    }
}