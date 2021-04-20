using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    /// <summary>
    /// Defines appearance of the node of the action "ProjectClock"
    /// </summary>
    public class NodeViewProjectClock : NodeViewAction
    {
        SerializedObject so;

        SerializedProperty propTriggerTime;

        PropertyField fieldTriggerTime;
        public NodeViewProjectClock(NodeModelAction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Action);

            propTriggerTime = so.FindProperty("triggerTime");

            fieldTriggerTime = new PropertyField(propTriggerTime, "Time before start");
            fieldTriggerTime.Bind(so);

            inputContainer.RemoveAt(0);
            extensionContainer.Add(fieldTriggerTime);
            extensionContainer[0].RemoveAt(1);
        }
    }
}