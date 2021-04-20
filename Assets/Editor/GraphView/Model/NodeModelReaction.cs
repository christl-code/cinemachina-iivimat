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

        /// <summary>
        /// Initialise reaction
        /// </summary>
        /// <param name="reactionType"></param>
        public void SetupReaction(string reactionType)
        {
            // Create the reaction asset.
            reaction = ScriptableObject.CreateInstance(reactionType) as Reaction;
            reaction.name = reaction.assetName;
            reactionID = reaction.Guid;
            //Save it to the scene.
            InteractionsUtility.GetInteractionsSaver().AddReaction(reaction);
        }

        /// <summary>
        /// On Destruction the reaction is unregisterd from all actions linked to it
        /// </summary>
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