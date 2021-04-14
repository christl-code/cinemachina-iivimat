using System.Collections.Generic;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Reaction Class contains a guid, a title and a list of action
    /// ATTENTION, LOOP + PLAYONCE = FAIRE 2 FOIS LA MÊME ACTION
    /// </summary>
    [System.Serializable]
    public class Reaction : ScriptableObject
    {

        [SerializeField]
        private string guid;
        public string GetGuid() { return guid; }

        [SerializeField]
        private string title;
        public string GetTitle() { return title; }
        public void SetTitle(string title) { this.title = title; }
        public string assetName { get { return "Reaction_" + guid; } private set { } }
        public string GetName() { return name; }
        public void SetName(string name) { this.name = name; }

        [SerializeField]
        private List<Action> events;
        public List<Action> Events
        {
            get
            {
                if (events == null)
                    events = new List<Action>();
                return events;
            }
        }


        // For the option "Play once"
        public bool playOnce = false;
        protected bool finished = false;


        public Reaction()
        {
            if (string.IsNullOrEmpty(guid))
                guid = System.Guid.NewGuid().ToString();
        }

        private void OnEnable() => Clean();

        /// <summary>
        /// Listen link this reaction to the specified action
        /// </summary>
        public void Listen(Action action)
        {
            Clean();
            Events.Add(action);
        }

        /// <summary>
        /// Unlisten unlink this reaction to the specified action
        /// </summary>
        public void Unlisten(Action action)
        {
            Events.Remove(action);
            Clean();
        }

        /// <summary>
        /// OnEventRaised triggers this reaction for every targets
        /// </summary>
        public virtual void OnEventRaised() {

            // If we don't want to play it once
            if(!playOnce){
                OnRaised();
            }
            else{
                // If we want to play it once and it has not be done
                if(!finished){
                    OnRaised();
                }
            }
        }

        public void setFinished(bool value){
            finished = value;
        } 

        /// <summary>
        /// OnEventRaised stops this reaction for every targets
        /// </summary>
        public virtual void OnEventStopped() { }

        /// <summary>
        /// Raised all the events 
        /// </summary>
        public virtual void OnRaised(){}

        /// <summary>
        /// AddCoroutine is called when an action loops
        /// </summary>
        public virtual void AddCoroutine(Action action) { }

        /// <summary>
        /// RemoveCoroutine is called when an action finished looping
        /// </summary>
        public virtual void RemoveCoroutine(Action action) { }

        /// <summary>
        /// Clean may be used to erase null references in the list of actions & the list of targets
        /// </summary>
        public virtual void Clean()
        {
            for (int i = 0; i < Events.Count; i++)
                if (Events[i] == null)
                    Events.RemoveAt(i);
        }
    }
}