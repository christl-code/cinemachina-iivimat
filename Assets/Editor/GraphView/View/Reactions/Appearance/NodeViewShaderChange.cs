using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewShaderChange : NodeViewReaction
    {
        SerializedObject so;

        SerializedProperty propShader;
        

        PropertyField fieldShader;


        public NodeViewShaderChange(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propShader = so.FindProperty("shader");
            

            fieldShader = new PropertyField(propShader, "New Shader");
            fieldShader.Bind(so);

            

            extensionContainer.Add(fieldShader);
            
        }
    }
}