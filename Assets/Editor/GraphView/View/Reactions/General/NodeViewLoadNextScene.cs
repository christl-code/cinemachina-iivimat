
namespace iivimat
{
    public class NodeViewLoadNextScene : NodeViewReactionString
    {
        public NodeViewLoadNextScene(NodeModelReaction nodeModel) : base(nodeModel)
        {
            maxTargets = 1;
        }
    }
}