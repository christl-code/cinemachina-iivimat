using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace iivimat
{
    public class GraphModel : ScriptableObject
    {
        public List<NodeModelBase> nodes;
        public List<EdgeModel> edges;

        public GraphModel()
        {
            nodes = new List<NodeModelBase>();
            edges = new List<EdgeModel>();
        }

        public void Add(NodeModelBase node)
        {
            nodes.Add(node);
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
        }

        public void Add(EdgeModel edge)
        {
            edges.Add(edge);
            AssetDatabase.AddObjectToAsset(edge, this);
            AssetDatabase.SaveAssets();
        }

        public void Remove(NodeModelBase node)
        {
            if (node == null)
                return;

            Undo.SetCurrentGroupName(node.title + " node deletion");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "");
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
            // ScriptableObject.DestroyImmediate(node, true);
            Undo.DestroyObjectImmediate(node);

            Undo.CollapseUndoOperations(group);
            Clean();
        }

        public void Remove(EdgeModel edge)
        {
            if (edge == null)
                return;

            Undo.SetCurrentGroupName("edge deletion");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "");
            edges.Remove(edge);
            AssetDatabase.RemoveObjectFromAsset(edge);
            AssetDatabase.SaveAssets();
            // ScriptableObject.DestroyImmediate(edge, true);
            Undo.DestroyObjectImmediate(edge);

            Undo.CollapseUndoOperations(group);
            Clean();

        }

        public void Clean()
        {

            List<EdgeModel> edgesToRemove = new List<EdgeModel>();
            List<NodeModelBase> nodesToRemove = new List<NodeModelBase>();

            // Clean lists from null
            for (int i = 0; i < nodes.Count; i++)
                if (nodes[i] == null)
                    nodes.Remove(nodes[i]);
            for (int i = 0; i < edges.Count; i++)
                if (edges[i] == null)
                    edges.Remove(edges[i]);

            // Fix for Unity's undo object in asset - Add all ghosts elements
            nodes.ForEach(node =>
            {
                if (!AssetDatabase.Contains(node)){
                    
                    AssetDatabase.AddObjectToAsset(node, this);
                }
                    
            });
            edges.ForEach(edge =>
            {
                if (!AssetDatabase.Contains(edge))
                    AssetDatabase.AddObjectToAsset(edge, this);

                // When a redo is done, add actions to objects according to edge models, as edges are reconstructed before nodes
                if (edge.output is NodeModelObject nodeModelObject)
                {
                    NodeModelAction nodeModelAction = edge.input as NodeModelAction;
                    if(nodeModelAction.Action != null && nodeModelObject.Go != null){
                        if (!nodeModelObject.Go.GetComponent<LocalActions>().Actions.Contains(nodeModelAction.Action))
                        {
                            nodeModelAction.Action.AddGameObject(nodeModelObject.Go);
                            Debug.Log("Link re-done");
                        }
                    }else{                            
                        nodesToRemove.Add(nodeModelObject);
                        edgesToRemove.Add(edge);
                    }
                }
                else if (edge.input is NodeModelReaction nodeModelReaction)
                {
                    NodeModelAction nodeModelAction = edge.output as NodeModelAction;
                    //if(nodeModelAction.Action != null && nodeModelReaction.Reaction != null){
                        if (!nodeModelReaction.Reaction.Events.Contains(nodeModelAction.Action))
                        {
                            nodeModelAction.Action.RegisterListener(nodeModelReaction.Reaction);
                            Debug.Log("Link re-done");
                        }
                    //}

                }
            });

            edgesToRemove.ForEach(edge => Remove(edge));
            nodesToRemove.ForEach(node => Remove(node));

        }
    }
}
