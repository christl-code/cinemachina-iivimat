using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace iivimat
{
    /// <summary>
    /// 
    /// </summary>
    public class NodeViewAction : Node
    {

        public Color backgroundColor = Color.blue;
        Color backgroundDisabledColor;
        SerializedObject so;

        protected SerializedProperty propDelay;
        protected SerializedProperty propShallLoop;
        protected SerializedProperty propState;
        protected SerializedProperty propTimeBetweenTriggers;
        protected SerializedProperty propNbTriggers;

        public NodeViewAction(NodeModelAction nodeModel)
        {
            // SETUP
            title = nodeModel.Action.Title;
            capabilities |= Capabilities.Movable;
            viewDataKey = nodeModel.guid;
            name = nodeModel.guid;
            userData = nodeModel;
            SetPosition(new Rect(nodeModel.position, Vector2.zero));
            mainContainer.style.backgroundColor = backgroundDisabledColor;
            backgroundDisabledColor =  backgroundColor / 3;
            // Ports
            var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(NodeModelObject));
            inputPort.portName = "Game Object(s)";
            inputPort.name = "input";

            var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(NodeModelReaction));
            outputPort.portName = "Reaction(s)";
            outputPort.name = "output";

            inputContainer.Add(inputPort);
            outputContainer.Add(outputPort);

            // Parameters
            extensionContainer.Add(BuildMainContent());

            RefreshExpandedState();
        }

        public VisualElement BuildMainContent()
        {
            so = new SerializedObject( ((NodeModelAction)userData).Action );

            propDelay = so.FindProperty("delay");
            propState = so.FindProperty("state");
            propShallLoop = so.FindProperty("shallLoop");
            propTimeBetweenTriggers = so.FindProperty("timeBetweenTriggers");
            propNbTriggers = so.FindProperty("nbTriggers");

            // Loads and clones our VisualTree (eg. our UXML structure) inside the root.
            VisualElement root = new VisualElement();
            VisualTreeAsset quickToolVisualTree = Resources.Load<VisualTreeAsset>("UI/HelpBox");
            quickToolVisualTree.CloneTree(root);
            VisualElement visualElement = root.Q<VisualElement>("container");

            //State 
            PropertyField fieldState = new PropertyField(propState, "State");
            fieldState.Bind(so);
            visualElement.Add(fieldState);

            fieldState.RegisterValueChangeCallback(evt =>
            {
                if(propState.enumValueIndex == 1){
                    mainContainer.style.backgroundColor = backgroundDisabledColor;
                    visualElement.style.backgroundColor = backgroundDisabledColor;

                }else{
                    mainContainer.style.backgroundColor = backgroundColor;
                    visualElement.style.backgroundColor = backgroundColor;
                }

            });

            //Delay
            PropertyField fieldDelay = new PropertyField(propDelay, "Delay (sec)");
            fieldDelay.Bind(so);
            visualElement.Add(fieldDelay);

            // Additionnal info if the action loops
            PropertyField fieldTimeBetweenTriggers = new PropertyField(propTimeBetweenTriggers, "Time between triggers (sec)");
            fieldTimeBetweenTriggers.Bind(so);

            PropertyField fieldNbTriggers = new PropertyField(propNbTriggers, "Number of triggers");
            fieldNbTriggers.Bind(so);

            // Shall Loop ? If yes, then display the additionnal info
            Toggle fieldShallLoop = new Toggle("Loop ?");
            fieldShallLoop.BindProperty(propShallLoop);

            visualElement.Add(fieldShallLoop);
            // When UI is first loaded
            if (propShallLoop.boolValue)
            {
                visualElement.Add(fieldTimeBetweenTriggers);
                visualElement.Add(fieldNbTriggers);
            }
            // And when UI changes
            fieldShallLoop.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue)
                {
                    visualElement.Add(fieldTimeBetweenTriggers);
                    visualElement.Add(fieldNbTriggers);
                }
                else
                {
                    if(visualElement.Contains(fieldTimeBetweenTriggers)){
                        visualElement.Remove(fieldTimeBetweenTriggers);
                    }
                    if(visualElement.Contains(fieldNbTriggers)){
                        visualElement.Remove(fieldNbTriggers);
                    }
                }
            });

            return visualElement;
        }
    }
}