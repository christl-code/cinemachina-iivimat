using System.Collections.Generic;
using UnityEngine;

namespace iivimat
{
    public abstract class ReactionObject<T> : Reaction where T : Object
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

        public ReactionObject() : base() { }

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
        public override void AddCoroutine(Action action)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveCoroutine(Action action)
        {
            throw new System.NotImplementedException();
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