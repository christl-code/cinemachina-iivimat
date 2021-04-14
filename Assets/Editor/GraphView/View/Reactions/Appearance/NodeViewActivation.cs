using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewActivation : NodeViewReaction
    {
        SerializedObject so;

        SerializedProperty propActivation;

        PropertyField fieldActivation;

        public NodeViewActivation(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propActivation = so.FindProperty("activation");

            fieldActivation = new PropertyField(propActivation, "Activation");
            fieldActivation.Bind(so);

            extensionContainer.Add(fieldActivation);
        }
    }
}