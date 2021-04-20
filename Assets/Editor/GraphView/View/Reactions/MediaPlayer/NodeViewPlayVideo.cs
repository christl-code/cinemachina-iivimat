using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewPlayVideo : NodeViewReaction
    {
        protected SerializedObject so;

        SerializedProperty propVideoLoop;

        PropertyField fieldVideoLoop;

        public NodeViewPlayVideo(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propVideoLoop = so.FindProperty("videoLoop");

            fieldVideoLoop = new PropertyField(propVideoLoop, "Play in loop");
            fieldVideoLoop.Bind(so);

            extensionContainer.Add(fieldVideoLoop);
        }
    }
}