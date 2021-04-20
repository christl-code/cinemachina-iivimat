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
                        
                        foreach(Action a in InteractionsUtility.GetInteractionsSaver().actions){
                            if(a.Title == target){
                                a.state = state;
                            }
                        }
                        
                    }
                }
            }
        }

        public override void OnEventStopped(String target)
        {
            // Debug.Log("LoadNextScene stopped");
        }
    }
}