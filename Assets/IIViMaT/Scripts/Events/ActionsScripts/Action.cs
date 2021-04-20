using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace iivimat
{
    public enum State
    {
        Enabled,
        Disabled,
    };

    /// <summary>
    /// Action Class contains a guid, a name, a list of reactions and a list of GameObjects
    /// </summary>

    [Serializable]
    public class Action : ScriptableObject
    {
        public static int _instance = 0; 
        [SerializeField]
        public int instance;

        [SerializeField]
        private string guid;
        public string Guid { get { return guid; } private set { guid = value; } }
        [SerializeField]
        private string title;
        public string Title { get { return "" + GetType().ToString().Substring(GetType().ToString().IndexOf(".")+1) + "_" + instance; } set { title = value; } }
        public string assetName { get { return "Action_" + guid; } private set { } }

        [SerializeField]
        public float delay = 0.0f;

        [SerializeField]
        public State state = State.Enabled;


        [SerializeField]
        private List<Reaction> eventListeners;
        public List<Reaction> EventListeners
        {
            get
            {
                if (eventListeners == null)
                    eventListeners = new List<Reaction>();
                return eventListeners;
            }
        }

        [SerializeField]
        private List<string> objectIDs;
        public List<string> ObjectIDs
        {
            get
            {
                if (objectIDs == null)
                    objectIDs = new List<string>();
                return objectIDs;
            }
        }

        [SerializeField]
        private List<GameObject> objects;
        public List<GameObject> Objects
        {
            get
            {
                if (objects == null)
                    objects = new List<GameObject>();

                objects.Clear();
                foreach (string objectID in ObjectIDs)
                    objects.Add(InteractionsUtility.FindGameObjectByGuid(objectID));

                return objects;
            }
        }

        // Every attributes about loop just below
        // -----------------------------------
        public bool shallLoop = true; // Can loop infinitely or a specific number of times
        public float timeBetweenTriggers = 1f;
        public int nbTriggers = 0; // 0 = loop
                                   // -----------------------------------


        public Action()
        {
            if (String.IsNullOrEmpty(Guid)) Guid = System.Guid.NewGuid().ToString();
            instance = ++_instance;
        }

        private void OnEnable() => Clean();

        /// <summary>
        /// If this action is supposed to be triggered by a local action done to a specific object,
        /// Please make sure to call GetComponent-LocalActions-().GetActions() on the object this action is applied to before calling Raise()
        /// </summary>
        /// <returns></returns>
        public void Raise()
        {
            InteractionsUtility.GetInteractionsSaver().StartCoroutine(RaiseEveryone(this));
        }


        /// <summary>
        /// Send the signal to every reactions behind. it's a IEnumerator to allows the delay.
        /// </summary>
        /// <returns></returns>
        IEnumerator RaiseEveryone(Action action){

            if (action.state == iivimat.State.Enabled)
            {
                yield return new WaitForSeconds(action.delay);

                for (int i = action.EventListeners.Count - 1; i >= 0; i--)
                {
                    if (action.shallLoop)
                    {
                        action.EventListeners[i].AddCoroutine(action);
                    }
                    else
                    {
                        action.EventListeners[i].OnEventRaised();
                    }
                }
                action.OnRaise();
            }
        }
        public virtual void OnRaise() { }

        /// <summary>
        /// Stops every coroutine raised by the Raise method 
        /// Please make sure to call GetComponent-LocalActions-().GetActions() on the object this action is applied to before calling Stop()
        /// </summary>
        /// <returns></returns>
        public void Stop()
        {

            for (int i = EventListeners.Count - 1; i >= 0; i--)
            {
                if (shallLoop)
                {
                    EventListeners[i].RemoveCoroutine(this);
                }
                else
                {
                    EventListeners[i].OnEventStopped();
                }
            }
            OnStop();

        }

        public virtual void OnStop() { }

        public void EnterPlayMode() => OnEnterPlayMode();

        /// <summary>
        /// Usually called to hide / teardown feedbacks
        /// </summary>
        /// <returns></returns>
        public virtual void OnEnterPlayMode() { }

        public void EnterEditMode() => OnEnterEditMode();

        /// <summary>
        /// Usually called to show / setup feedbacks
        /// </summary>
        /// <returns></returns>
        public virtual void OnEnterEditMode() { }

        public void RegisterListener(Reaction listener)
        {
            // Lien Action -> Reaction
            if (!EventListeners.Contains(listener))
            {
                Clean();
                EventListeners.Add(listener);
                listener.Listen(this);
                OnRegister();
            }
        }

        public virtual void OnRegister() { }

        /// <summary>
        /// Removes listener from the list of reactions of the Action 
        /// </summary>
        /// <param name="listener"></param>

        public void UnregisterListener(Reaction listener)
        {
            // Lien Action -/> Reaction
            if (EventListeners.Contains(listener))
            {
                EventListeners.Remove(listener);
                listener.Unlisten(this);
                Clean();
                OnUnregister();
            }
        }

        public virtual void OnUnregister() { }

        /// <summary>
        /// Adds an action to the list of local actions of go
        /// and adds the guid of go to the list of guids of GameObjects of the action 
        /// </summary>
        /// <param name="go"></param>
        public void AddGameObject(GameObject go)
        {
            string guid = go.GetComponent<UniqueID>().Guid;
            if (!ObjectIDs.Contains(guid))
            {
                Clean();
                // Lien GameObject <- Action
                ObjectIDs.Add(guid);
                // Lien GameObject -> Action
                go.GetComponent<LocalActions>().AddAction(this);
                List<GameObject> lg = Objects;
                OnAddGameObject(go);
            }
        }

        /// <summary>
        /// Called when a link is done between the current action and the object given as a parameter
        /// </summary>
        /// <returns></returns>
        public virtual void OnAddGameObject(GameObject go) { }

        /// <summary>
        /// Removes an action from the list of local actions of go
        /// and removes the guid from go to the list of guids of GameObjects of the action 
        /// </summary>
        /// <param name="go"></param>
        public void RemoveGameObject(GameObject go)
        {
            if(go != null){
                string guid = go.GetComponent<UniqueID>().Guid;
                if (ObjectIDs.Contains(guid))
                {
                    // Lien GameObject </- Action
                    ObjectIDs.Remove(guid);
                    // Lien GameObject -/> Action
                    go.GetComponent<LocalActions>().RemoveAction(this);
                    Clean();
                    OnRemoveGameObject(go);
                }
            }
        }

        /// <summary>
        /// Called when a link is removed between the current action and the object given as a parameter
        /// </summary>
        /// <returns></returns>
        public virtual void OnRemoveGameObject(GameObject go) { }

        /// <summary>
        /// Removes all of the null GameObjects of the Objects list of the action
        /// and removes all of the null reactions of the EventListeners of the action
        /// this avoids null objects exception when this action is raised
        /// </summary>
        public void Clean()
        {
            for (int i = 0; i < Objects.Count; i++)
                if (Objects[i] == null)
                    Objects.RemoveAt(i);
            for (int i = 0; i < EventListeners.Count; i++)
                if (EventListeners[i] == null)
                    EventListeners.RemoveAt(i);
        }
    }
}