using System;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Defines variables and behaviour of edge 
    /// </summary>
    public class EdgeModel : ScriptableObject
    {
        public string guid { get; private set; }
        public NodeModelBase input;
        public NodeModelBase output;

        public string assetName { get { return "Edge_" + guid; } private set { } }

        /// <summary>
        /// Edgemodel constructor
        /// Allocates GUID to edges
        /// </summary>
        public EdgeModel()
        {
            guid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// On Destruction, action removes actor if action and actor were connected
        /// or unregisters reaction, action were connected to reaction
        /// </summary>
        private void OnDestroy()
        {
            if (input is NodeModelAction)
            {
                GameObject go = ((NodeModelObject)output).Go;
                Action action = ((NodeModelAction)input).Action;
                action.RemoveGameObject(go);
            }
            else if (output is NodeModelAction)
            {
                Action action = ((NodeModelAction)output).Action;
                Reaction reaction = ((NodeModelReaction)input).Reaction;
                action.UnregisterListener(reaction);
            }
        }
    }
}