using System;
using UnityEditor;
using UnityEngine;

namespace iivimat
{
    [Serializable]
    public class NodeModelReaction : NodeModelBase
    {
        [SerializeField]
        private Reaction reaction;
        public Reaction Reaction
        {
            get
            {
                if (reaction == null)
                    reaction = InteractionsUtility.FindReactionByGuid(reactionID);
                return reaction;
            }
        }

        public string reactionID;

        public void SetupReaction(string reactionType)
        {
            // Create the reaction asset.
            reaction = ScriptableObject.CreateInstance(reactionType) as Reaction;
            reaction.name = reaction.assetName;
            reaction.SetTitle("" + reaction.GetType() + reaction.GetInstanceID());
            reactionID = reaction.GetGuid();

            //Save it to the scene.
            InteractionsUtility.GetInteractionsSaver().AddReaction(reaction);
        }

        private void OnDestroy()
        {
            if (reaction != null)
            {

                foreach (Action action in reaction.Events)
                {
                    action.UnregisterListener(reaction);
                }

                Undo.RegisterCompleteObjectUndo(InteractionsUtility.GetInteractionsSaver(), "");
                InteractionsUtility.GetInteractionsSaver().RemoveReaction(reaction);
                // DestroyImmediate(reaction, true);
                Undo.DestroyObjectImmediate(reaction);
            }
        }
    }
}