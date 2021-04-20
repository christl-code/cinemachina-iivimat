namespace iivimat
{
    /// <summary>
    /// Defines appearance of the node of "HandTeleport"s
    /// </summary>
    public class NodeViewHandTeleport : NodeViewAction
    {
        public NodeViewHandTeleport(NodeModelAction nodeModel) : base(nodeModel)
        {
            outputContainer.RemoveAt(0);
            extensionContainer.RemoveAt(0);
            mainContainer.style.backgroundColor = backgroundColor;
        }
    }
}