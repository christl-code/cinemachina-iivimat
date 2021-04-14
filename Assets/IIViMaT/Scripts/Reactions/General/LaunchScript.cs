using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq; 

namespace iivimat
{
    /// <summary>
    /// Reaction : launch for the GameObject all the method reactionToScript() present on every IReactionScript in the GameObject's components.
    /// This can be usefull to do every things IIVIMAT can't do.
    /// </summary>
    public class LaunchScript : ReactionObject<GameObject>
    {
        /// <summary>
        /// Launch the reactionToScript() method of every IreactionScript in the GameObject components
        /// </summary>
        /// <param name="current"></param>
         public override void OnEventRaised(GameObject target)
        {
            //target.SetActive(false);
            if(!playOnce || !finished)
            {
                Component[] ss = target.GetComponents(typeof(IReactionScript));

                foreach (IReactionScript s in ss) {
                    s.ReactionToScript();
                }
                setFinished(true);
            }
        }

        public override void OnEventStopped(GameObject target)
        {
            // Debug.Log("Script finished");
        }
    }
}