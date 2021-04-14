using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewVisibility : NodeViewReaction
    {
        SerializedObject so;

        SerializedProperty propVisibility;

        PropertyField fieldVisibility;

        public NodeViewVisibility(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propVisibility = so.FindProperty("visibility");

            fieldVisibility = new PropertyField(propVisibility, "Visibility mode");
            fieldVisibility.Bind(so);

            extensionContainer.Add(fieldVisibility);
        }
    }
}