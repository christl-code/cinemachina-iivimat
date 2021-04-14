using System;
using UnityEngine;

namespace iivimat
{
    public class ActivationAction : ReactionString<String>
    {
        public State state = iivimat.State.Disabled;

        public override void OnEventRaised(String target)
        {
            if(!playOnce || !finished)
            {
                //target.SetActive(false);
                if(!playOnce || !finished)
                {
                    if(Targets.Contains(target)){

                        Action a = InteractionsUtility.FindActionByGuid(target);
                        a.state = state;
                        
                    }
                    setFinished(true);
                }
            }
        }

        public override void OnEventStopped(String target)
        {
            // Debug.Log("LoadNextScene stopped");
        }
    }
}