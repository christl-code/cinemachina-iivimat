using System.Collections.Generic;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReactionComponent<T> : Reaction where T : Component
    {
        public List<T> targets;
        public List<T> Targets
        {
            get
            {
                if (targets == null)
                    targets = new List<T>();
                return targets;
            }
        }

        public ReactionComponent() : base() { }

        public override void OnRaised() { 
            Targets.ForEach(target => this.OnEventRaised(target)); 
        }

        /// <summary>
        /// OnEventRaised triggers this reaction for a specific target
        /// </summary>
        public abstract void OnEventRaised(T target);

        public override void OnEventStopped() { Targets.ForEach(target => this.OnEventStopped(target)); }

        /// <summary>
        /// OnEventStopped stops this reaction for a specific target
        /// </summary>
        public abstract void OnEventStopped(T target);

        // Every methods about loop just below
        // -----------------------------------
        // -----------------------------------
        
        /// <summary>
        /// Adds a coroutine to the queue with action
        /// </summary>
        /// <param name="action"></param>
        public override void AddCoroutine(Action action)
        {
            foreach (T target in Targets)
            {
                GameObject go = target.gameObject;
                if (go.GetComponent<LocalCoroutineHandler>() != null)
                {
                    go.GetComponent<LocalCoroutineHandler>().AddCoroutine(action, this, target);
                }
            }
        }

        /// <summary>
        /// Removes a coroutine to the queue with action
        /// </summary>
        /// <param name="action"></param>
        public override void RemoveCoroutine(Action action)
        {
            foreach (T target in Targets)
            {
                GameObject go = target.gameObject;
                if (go.GetComponent<LocalCoroutineHandler>() != null)
                {
                    go.GetComponent<LocalCoroutineHandler>().RemoveCoroutine(action, this);
                }
            }
        }
        // -----------------------------------
        // -----------------------------------

        /// <summary>
        /// Removes all the null targets
        /// </summary>
        public override void Clean()
        {
            base.Clean();
            for (int i = 0; i < Targets.Count; i++)
                if (Targets[i] == null)
                    Targets.RemoveAt(i);
        }
    }
}