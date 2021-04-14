namespace iivimat
{
    public class NodeViewHandTeleport : NodeViewAction
    {
        public NodeViewHandTeleport(NodeModelAction nodeModel) : base(nodeModel)
        {
            outputContainer.RemoveAt(0);
            extensionContainer.RemoveAt(0);
        }
    }
}