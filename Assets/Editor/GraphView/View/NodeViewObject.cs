using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace iivimat
{
    /// <summary>
    /// 
    /// </summary>
    public class NodeViewObject : Node
    {
        Color backgroundColor = Color.green;

        public NodeViewObject(NodeModelObject nodeModel)
        {
            // SETUP
            title = nodeModel.title;
            capabilities |= Capabilities.Movable;
            viewDataKey = nodeModel.guid;
            name = nodeModel.guid;
            userData = nodeModel;
            SetPosition(new Rect(nodeModel.position, Vector2.zero));

            mainContainer.style.backgroundColor = backgroundColor;


            var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(NodeModelAction));
            outputPort.portName = "Action(s)";
            outputPort.name = "output";

            outputContainer.Add(outputPort);

            // CALLBACKS
            this.RegisterCallback<AttachToPanelEvent>(evt =>
            {
                nodeModel.Renamed += OnRename;
                nodeModel.Destroyed += OnDestruction;
                Selection.selectionChanged += OnSelectionChanged;
            });
            this.RegisterCallback<DetachFromPanelEvent>(evt =>
            {
                nodeModel.Renamed -= OnRename;
                nodeModel.Destroyed -= OnDestruction;
                Selection.selectionChanged -= OnSelectionChanged;
            });
        }

        // Delegate called when nodeModel is renamed
        void OnRename()
        {
            title = ((NodeModelObject)userData).title;
            MarkDirtyRepaint();
        }

        // Delegate called when nodeModel is destroyed
        // As we don't have a reference to GraphViewWindow.Reload(), we just make it uninteractable while waiting for a Reload elsewhere
        // TODO: Still shown in the minimap as long as Reload is not called
        void OnDestruction()
        {
            this.SetEnabled(false);
            // this.visible = false;
        }

        // Delegate called when selection has changed in hierarchy
        // Used to make a node active if the nodeModel.Go is currently selected
        void OnSelectionChanged()
        {
            NodeModelObject nodeModel = userData as NodeModelObject;

            try
            {
                if (Selection.gameObjects.ToList().Contains(nodeModel.Go))
                    selected = true;
                else
                    selected = false;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        // Used to change Selection.gameObjects when the user clicks the node
        public override void OnSelected()
        {
            NodeModelObject objectNodeModel = userData as NodeModelObject;

            if (!Selection.Contains(objectNodeModel.Go))
            {
                GameObject[] gos = new GameObject[Selection.gameObjects.Length + 1];
                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    gos[i] = Selection.gameObjects[i];
                }
                gos[gos.Length - 1] = objectNodeModel.Go;
                Selection.objects = gos;
            }
        }

        // Used to change Selection.gameObjects when the user unclicks the node
        public override void OnUnselected()
        {
            NodeModelObject objectNodeModel = userData as NodeModelObject;
            if(objectNodeModel.Go != null){
                if (Selection.Contains(objectNodeModel.Go))
                {
                    GameObject[] gos = new GameObject[Selection.gameObjects.Length - 1];
                    int offset = 0;
                    for (int i = 0; i < Selection.gameObjects.Length; i++)
                    {
                        if (Selection.gameObjects[i - offset] != objectNodeModel.Go)
                            gos[i] = Selection.gameObjects[i];
                        else
                            offset = 1;
                    }
                    Selection.objects = gos;
                }
                
            }
        }
    }
}