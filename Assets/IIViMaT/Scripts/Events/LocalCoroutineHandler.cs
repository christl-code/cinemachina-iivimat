using System;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Handles all of the actions within a specific GameObject
    /// </summary>
    public class LocalCoroutineHandler : MonoBehaviour
    {
        private Dictionary<Type, List<ILocalLoopCoroutine>> queues = new Dictionary<Type, List<ILocalLoopCoroutine>>();

        /// <summary>
        /// Adds a coroutine which will execute "reaction" every time "action" is done on "target"
        /// </summary>
        /// <param name="action"></param>
        /// <param name="reaction"></param>
        /// <param name="target"></param>
        /// <typeparam name="T"></typeparam>
        public void AddCoroutine<T>(Action action, ReactionComponent<T> reaction, T target) where T : Component
        {
            // When adding a new coroutine, we first want to check if there already is a list of all running / pending coroutines for a specific reaction
            Type t = reaction.GetType();
            List<ILocalLoopCoroutine> l;
            if (!queues.ContainsKey(t))
            {
                // If there is no list, we create one
                l = new List<ILocalLoopCoroutine>();
                queues.Add(t, l);
            }
            else
            {
                // Otherwise we retrieve it
                l = queues[t];
            }
            // Then we create the new coroutine and add it to the list
            LocalLoopCoroutine<T> loopCoroutine = new LocalLoopCoroutine<T>(action, reaction, target);
            l.Add(loopCoroutine);
            // And if the newly added coroutine is the only one in the list, we run it
            if (l.Count == 1)
            {
                StartCoroutine(l[0].GetCoroutine());
            }
        }

        /// <summary>
        /// Removes the coroutine which is executed when "action" is performed with reaction as the result  
        /// </summary>
        /// <param name="action"></param>
        /// <param name="reaction"></param>
        /// <typeparam name="T"></typeparam>
        public void RemoveCoroutine<T>(Action action, ReactionComponent<T> reaction) where T : Component
        {
            // When removing a coroutine, we first check if there is a list for the specific reaction
            Type t = reaction.GetType();
            List<ILocalLoopCoroutine> l;
            if (queues.ContainsKey(t))
            {
                l = queues[t];
                // Then we find the coroutine that has been added thanks to a specific action
                for (int i = 0; i < l.Count; i++)
                {
                    ILocalLoopCoroutine c = l[i];
                    if (c.GetAction().Equals(action))
                    {
                        // And we remove it from the list
                        // But we also want to stop it if it was running, and run a new one if so
                        bool wasRunning = false;
                        if (l[0].Equals(c))
                        {
                            wasRunning = true;
                            StopCoroutine(c.GetCoroutine());
                        }
                        l.Remove(c);
                        if (l.Count > 0 && wasRunning)
                        {
                            StartCoroutine(l[0].GetCoroutine());
                        }
                    }
                }
            }
        }

        public void Clear(){
            queues.Clear();
        }
    }
}