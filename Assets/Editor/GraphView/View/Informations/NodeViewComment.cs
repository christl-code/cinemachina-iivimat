using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;

namespace iivimat
{
    /// <summary>
    /// Defines appearance of the node of "Comment"
    /// </summary>
    public class NodeViewComment : NodeViewInformation
    {
        SerializedObject so;
        SerializedProperty propComment;
        PropertyField fieldComment;

        public NodeViewComment(NodeModelInformation nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Information);

            propComment = so.FindProperty("comment");

            fieldComment = new PropertyField(propComment, " ");
            
            fieldComment.Bind(so);
            extensionContainer.Add(fieldComment);
            mainContainer.RemoveAt(0);
            RefreshExpandedState();
        }
    }
}