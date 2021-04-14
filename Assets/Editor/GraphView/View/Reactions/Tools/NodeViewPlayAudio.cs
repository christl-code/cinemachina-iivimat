using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewPlayAudio : NodeViewReaction
    {
        SerializedObject so;

        SerializedProperty propAudioLoop;

        PropertyField fieldAudioLoop;

        public NodeViewPlayAudio(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propAudioLoop = so.FindProperty("audioLoop");

            fieldAudioLoop = new PropertyField(propAudioLoop, "Play in loop");
            fieldAudioLoop.Bind(so);

            extensionContainer.Add(fieldAudioLoop);
        }
    }
}