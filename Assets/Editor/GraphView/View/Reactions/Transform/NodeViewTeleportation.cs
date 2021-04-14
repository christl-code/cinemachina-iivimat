using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewTeleportation : NodeViewReaction
    {
        SerializedObject so;


        public NodeViewTeleportation(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);
            maxTargets = 1;
        }
    }
}