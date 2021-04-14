using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewActivationAction : NodeViewReactionString
    {
        SerializedObject so;

        SerializedProperty propState;
        SerializedProperty propList;
        PropertyField fieldState;
        PropertyField fieldList;

        public NodeViewActivationAction(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propState = so.FindProperty("state");

            fieldState = new PropertyField(propState, "State");
            fieldState.Bind(so);

            extensionContainer.Add(fieldState);

        }
    }
}