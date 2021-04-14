using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewTextureChange : NodeViewReaction
    {
        SerializedObject so;

        SerializedProperty propTexture;
        

        PropertyField fieldTexture;




        public NodeViewTextureChange(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propTexture = so.FindProperty("texture");
            

            fieldTexture = new PropertyField(propTexture, "New Texture");
            fieldTexture.Bind(so);

            

            extensionContainer.Add(fieldTexture);
            
        }
    }
}