using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Gathers useful methods for reaching GameObjects, actions and reactions
    /// </summary>
    public class InteractionsUtility
    {
        public static InteractionsSaver GetInteractionsSaver()
        {
            GameObject meta = GameObject.FindWithTag("Meta");
            if (meta != null)
            {
                // If GetComponent is null, then return AddComponent
                return meta.GetComponent<InteractionsSaver>() ?? meta.AddComponent<InteractionsSaver>();
            }
            else
            {
                Debug.LogError("No Scene Meta Game Object found");
                return null;
            }
        }

        public static EventManager GetEventManager()
        {
            GameObject meta = GameObject.FindWithTag("Meta");
            if (meta != null)
            {
                // If GetComponent is null, then return AddComponent
                return meta.GetComponent<EventManager>() ?? meta.AddComponent<EventManager>();
            }
            else
            {
                Debug.LogError("No Scene Meta Game Object found");
                return null;
            }
        }

        public static EventEndHandler GetEventEndHandler()
        {
            GameObject meta = GameObject.FindWithTag("Meta");
            if (meta != null)
            {
                // If GetComponent is null, then return AddComponent
                return meta.GetComponent<EventEndHandler>() ?? meta.AddComponent<EventEndHandler>();
            }
            else
            {
                Debug.LogError("No Scene Meta Game Object found");
                return null;
            }
        }
        public static GlobalActions GetGlobalActions()
        {
            GameObject meta = GameObject.FindWithTag("Meta");
            if (meta != null)
            {
                // If GetComponent is null, then return AddComponent
                return meta.GetComponent<GlobalActions>() ?? meta.AddComponent<GlobalActions>();
            }
            else
            {
                Debug.LogError("No Event End Handler Game Object found");
                return null;
            }
        }


        public static GameObject FindGameObjectByGuid(string guid)
        {
            if(InteractionsUtility.GetInteractionsSaver().actualGUIDS.Contains(guid)){
                GameObject environment = GameObject.FindWithTag("Environment");
                if (environment != null)
                {
                    List<Transform> children = environment.GetComponentsInChildren<Transform>().Except(new[] { environment.transform }).ToList();
                    foreach (Transform child in children)
                    {
                        GameObject go = child.gameObject;

                        if (go.CompareTag("Feedback"))
                            continue;

                        if (go.GetComponent<UniqueID>().Guid.Equals(guid))
                            return go;
                    }
                    return null;
                }
                else
                {
                    Debug.LogError("No Scene Environment Game Object found");
                    return null;
                }
            }
            return null;
            
        }
        public static Information FindInformationByGuid(string guid)
        {
            List<Information> informations = InteractionsUtility.GetInteractionsSaver().informations;
            foreach(Information information in informations){
                if(information.Guid.Equals(guid)){
                    return information;
                }
            }
            return null;
        }

        public static Action FindActionByGuid(string guid)
        {
            List<Action> actions = InteractionsUtility.GetInteractionsSaver().actions;
            foreach (Action action in actions)
            {
                if (action.Guid.Equals(guid))
                {
                    return action;
                }
            }
            return null;
        }

        public static Reaction FindReactionByGuid(string guid)
        {
            List<Reaction> reactions = InteractionsUtility.GetInteractionsSaver().reactions;
            foreach (Reaction reaction in reactions)
            {
                if (reaction.Guid.Equals(guid))
                {
                    return reaction;
                }
            }
            return null;
        }
    }
}