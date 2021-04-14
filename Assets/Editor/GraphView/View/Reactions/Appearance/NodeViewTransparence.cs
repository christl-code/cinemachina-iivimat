using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewTransparency : NodeViewReaction
    {
        SerializedObject so;

        SerializedProperty propTransparency;
        SerializedProperty propTransparencyMode;

        PropertyField fieldTransparency;
        PropertyField fieldTransparencyMode;

        public NodeViewTransparency(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propTransparency = so.FindProperty("transparency");
            propTransparencyMode = so.FindProperty("transparencyMode");

            fieldTransparency = new PropertyField(propTransparency, "Transparency");
            fieldTransparency.Bind(so);

            fieldTransparencyMode = new PropertyField(propTransparencyMode, "Transparency mode");
            fieldTransparencyMode.Bind(so);

            extensionContainer.Add(fieldTransparency);
            extensionContainer.Add(fieldTransparencyMode);
        }
    }
}
