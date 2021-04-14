using System;
using UnityEditor;
using UnityEngine;

namespace iivimat
{
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

        public void SetupAction(string actionType)
        {
            // Create the action asset.
            action = ScriptableObject.CreateInstance(actionType) as Action;
            action.name = action.assetName;
            action.Title = "" + action.GetType() + action.GetInstanceID();
            actionID = action.Guid;

            //Save it to the scene.
            InteractionsUtility.GetInteractionsSaver().AddAction(action);
        }

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