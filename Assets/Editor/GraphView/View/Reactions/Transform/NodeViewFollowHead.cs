using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewFollowHead : NodeViewReaction
    {
        SerializedObject so;

        SerializedProperty propOffset;


        PropertyField fieldOffset;


        public NodeViewFollowHead(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propOffset = so.FindProperty("offset");

            fieldOffset = new PropertyField(propOffset);
            fieldOffset.Bind(so);

 
            extensionContainer.Add(fieldOffset);

        }
    }
}