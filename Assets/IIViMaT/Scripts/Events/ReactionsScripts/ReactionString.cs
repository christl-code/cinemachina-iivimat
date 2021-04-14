using System.Collections.Generic;

namespace iivimat
{
    public abstract class ReactionString<String> : Reaction
    {
        public List<String> targets;
        public List<String> Targets
        {
            get
            {
                if (targets == null)
                    targets = new List<String>();
                return targets;
            }
        }

        public ReactionString() : base() { }

        public override void OnRaised() { 
            Targets.ForEach(target => this.OnEventRaised(target)); 
        }

        /// <summary>
        /// OnEventRaised triggers this reaction for a specific target
        /// </summary>
        public abstract void OnEventRaised(String target);

        public override void OnEventStopped() { Targets.ForEach(target => this.OnEventStopped(target)); }

        /// <summary>
        /// OnEventStopped stops this reaction for a specific target
        /// </summary>
        public abstract void OnEventStopped(String target);

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