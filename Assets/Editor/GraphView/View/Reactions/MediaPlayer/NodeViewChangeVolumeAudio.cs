using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewChangeVolumeAudio : NodeViewPlayVideo
    {
        SerializedProperty propVolumeVariation;

        PropertyField fieldVolumeVariation;

        public NodeViewChangeVolumeAudio(NodeModelReaction nodeModel) : base(nodeModel)
        {
            propVolumeVariation = so.FindProperty("value");

            fieldVolumeVariation = new PropertyField(propVolumeVariation, "Volume variation (- / +)");
            
            fieldVolumeVariation.Bind(so);

            extensionContainer.Add(fieldVolumeVariation);
        }
    }
}