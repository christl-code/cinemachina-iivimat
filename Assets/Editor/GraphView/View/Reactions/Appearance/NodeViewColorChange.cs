using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewColorChange : NodeViewReaction
    {
        SerializedObject so;

        SerializedProperty propRandomColor;
        SerializedProperty propColor;

        PropertyField fieldRandomColor;
        PropertyField fieldColor;

        public NodeViewColorChange(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propRandomColor = so.FindProperty("randomColor");
            propColor = so.FindProperty("color");

            fieldRandomColor = new PropertyField(propRandomColor, "Random Color ?");
            fieldRandomColor.Bind(so);

            fieldColor = new PropertyField(propColor, "Specific Color");
            fieldColor.Bind(so);

            extensionContainer.Add(fieldRandomColor);
            extensionContainer.Add(fieldColor);
        }
    }
}