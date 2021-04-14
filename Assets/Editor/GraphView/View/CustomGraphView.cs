using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace iivimat
{
    public class CustomGraphView : GraphView
    {
        public CustomGraphView(GraphModel graphModel)
        {
            // SETUP
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            Insert(0, new GridBackground());
            // focusIndex = 0;
            FrameAll();

            this.name = "theView";
            this.userData = graphModel;
            this.viewDataKey = "theView";
            this.StretchToParentSize();

            // Add the minimap if there is no one
            var miniMap = new MiniMap();
            miniMap.SetPosition(new Rect(0, 0, 150, 150));
            this.Add(miniMap);

            // CALLBACKS
            this.RegisterCallback<AttachToPanelEvent>(evt => this.graphViewChanged = GraphViewChanged);
            this.RegisterCallback<DetachFromPanelEvent>(evt => this.graphViewChanged = null);
            this.RegisterCallback<FocusEvent>(evt => Selection.objects = null);

            // MANIPULATORS
            // FIXME: add a coordinator so that ContentDragger and SelectionDragger cannot be active at the same time.
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());

        }

        private new GraphViewChange GraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (GraphElement element in graphViewChange.elementsToRemove)
                {
                    if (element is Node)
                    {
                        // Unlink between GOs, Actions and Reactions is done in node.OnDestroy()
                        var nodeModel = element.userData as NodeModelBase;
                        ((GraphModel)userData).Remove(nodeModel);
                    }
                    else if (element is Edge)
                    {
                        // Unlink between GOs, Actions and Reactions is done in edge.OnDestroy()
                        var edgeModel = element.userData as EdgeModel;
                        ((GraphModel)userData).Remove(edgeModel);
                    }
                }
            }

            if (graphViewChange.edgesToCreate != null)
            {
                foreach (Edge edge in graphViewChange.edgesToCreate)
                {
                    // Create new edge model
                    var edgeModel = ScriptableObject.CreateInstance<EdgeModel>();
                    Undo.RegisterCreatedObjectUndo(edgeModel, "edge connexion");

                    // Setup it
                    edgeModel.input = edge.input.node.userData as NodeModelBase;
                    edgeModel.output = edge.output.node.userData as NodeModelBase;
                    edgeModel.name = edgeModel.assetName;

                    // If it's a connection between ObjectNode and ActionNode
                    if (edgeModel.input is NodeModelAction)
                    {
                        // Here, we link GO to Action and vice-versa
                        GameObject go = ((NodeModelObject)edgeModel.output).Go;
                        Action action = ((NodeModelAction)edgeModel.input).Action;
                        action.AddGameObject(go);
                        // Don't forget to (try to) add all the objects of the action to the reaction
                        foreach (Reaction reaction in action.EventListeners)
                        {
                            // Debug.Log("Target " + go.name + " added in " + reaction.Title + " because it was in " + action.Title);
                            // reaction.AddTarget(go);
                        }
                    }
                    // Or if it's a connection between ActionNode and ReactionNode
                    else if (edgeModel.output is NodeModelAction)
                    {
                        // Here, we link Action to Reaction and vice-versa
                        Action action = ((NodeModelAction)edgeModel.output).Action;
                        Reaction reaction = ((NodeModelReaction)edgeModel.input).Reaction;
                        action.RegisterListener(reaction);
                    }

                    edge.viewDataKey = edgeModel.guid;
                    edge.userData = edgeModel;

                    ((GraphModel)userData).Add(edgeModel);

                    nodes.ForEach(node => node.MarkDirtyRepaint());
                }
            }

            if (graphViewChange.movedElements != null)
            {
                foreach (GraphElement element in graphViewChange.movedElements)
                {
                    if (element is Node)
                        (element.userData as NodeModelBase).position = element.GetPosition().position;
                }
            }

            return graphViewChange;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(nap =>
                nap.direction != startPort.direction &&
                nap.node != startPort.node)
                .ToList();
        }

        public void Clean()
        {
            // Remove all UI elements (customGraphView's Clear() method remove background too, which we don't want)
            nodes.ForEach(node => RemoveElement(node));
            edges.ForEach(edge => RemoveElement(edge));
        }
    }
}