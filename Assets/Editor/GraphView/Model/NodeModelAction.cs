using System;
using UnityEditor;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class NodeModelAction : NodeModelBase
    {
        [SerializeField]
        private Action action;
        public Action Action
        {
            get
            {
                if (action == null)
                    action = InteractionsUtility.FindActionByGuid(actionID);
                return action;
            }
        }

        public string actionID;

        /// <summary>
        /// Initialises an action and saves it 
        /// </summary>
        /// <param name="actionType"></param>
        public void SetupAction(string actionType)
        {
            // Create the action asset.
            action = ScriptableObject.CreateInstance(actionType) as Action;
            action.name = action.assetName;
            actionID = action.Guid;

            //Save it to the scene.
            InteractionsUtility.GetInteractionsSaver().AddAction(action);
        }

        /// <summary>
        /// On Destruction, actions are removed from local actions
        /// and their reactions,which were linkedwith, are unregistered 
        /// </summary>
        private void OnDestroy()
        {
            if (action != null)
            {
                foreach (GameObject go in action.Objects)
                {
                    LocalActions localActions = go.GetComponent<LocalActions>();
                    localActions.RemoveAction(action);
                }
                foreach (Reaction reaction in action.EventListeners)
                {
                    action.UnregisterListener(reaction);
                }

                Undo.RegisterCompleteObjectUndo(InteractionsUtility.GetInteractionsSaver(), "");
                InteractionsUtility.GetInteractionsSaver().RemoveAction(action);
                // DestroyImmediate(action, true);
                Undo.DestroyObjectImmediate(action);
            }
        }
    }
}