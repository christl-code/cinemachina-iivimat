using System.Collections.Generic;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Launch global coroutines
    /// </summary>
    [ExecuteInEditMode]
    public class GlobalActions : MonoBehaviour
    {
        public List<Action> actions;

        private bool launch = false;

        /// <summary>
        /// Creates a list of actions
        /// </summary>
        private void OnEnable()
        {
            if (actions == null)
            {
                actions = new List<Action>();
            }
            hideFlags = HideFlags.None;
        }

        /// <summary>
        /// Removes all the null action of the list "actions"
        /// </summary>
        public void Clean()
        {
            for (int i = 0; i < actions.Count; i++)
                if (actions[i] == null)
                    actions.RemoveAt(i);
        }

        /// <summary>
        /// Remove all actions in the list
        /// </summary>
        public void Clear(){
            actions.Clear();
        }

        public void StartAllCoroutine(){
            if(!launch){
                foreach(Action a in actions){
                    if(a is ProjectClock){
                        ProjectClock pc = (ProjectClock) a;
                        StartCoroutine(pc.clockCoroutine);
                    }
                }
                launch = true;
            }
        }
        
    }
}