using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace iivimat{
    public class NodeViewInformation : Node
    {
        SerializedObject so;
        public Color backgroundColor = Color.grey;
        public NodeViewInformation(NodeModelInformation nodeModel){
            // SETUP
            title = nodeModel.Information.GetType().Name;
            capabilities |= Capabilities.Movable;
            viewDataKey = nodeModel.guid;
            name = nodeModel.guid;
            userData = nodeModel;
            SetPosition(new Rect(nodeModel.position, Vector2.zero));
            mainContainer.style.backgroundColor = backgroundColor;
            RefreshExpandedState();
        }

    }
}
