using System;
using UnityEngine;

namespace iivimat
{
    public class EdgeModel : ScriptableObject
    {
        public string guid { get; private set; }
        public NodeModelBase input;
        public NodeModelBase output;

        public string assetName { get { return "Edge_" + guid; } private set { } }

        public EdgeModel()
        {
            guid = Guid.NewGuid().ToString();
        }

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