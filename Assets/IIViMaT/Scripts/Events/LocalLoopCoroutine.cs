using System.Collections;
using UnityEngine;

namespace iivimat
{
    public class LocalLoopCoroutine<T> : ILocalLoopCoroutine where T : Component
    {
        public Action action;
        public ReactionComponent<T> reaction;
        public T target;
        private IEnumerator coroutine;

        /// <summary>
        /// create a coroutine with specific action, reaction and target  
        /// </summary>
        /// <param name="action"></param>
        /// <param name="reaction"></param>
        /// <param name="target"></param>
        public LocalLoopCoroutine(Action action, ReactionComponent<T> reaction, T target)
        {
            this.action = action;
            this.reaction = reaction;
            this.target = target;
            coroutine = CreateCoroutine();
        }

        /// <summary>
        /// Creates a kind of process 
        /// and executes reaction indefinitly if wanted or not
        /// </summary>
        /// <returns></returns>
        public IEnumerator CreateCoroutine()
        {
            if (action.nbTriggers == 0)
            {
                while (true)
                {
                    reaction.OnEventRaised(target);
                    yield return new WaitForSeconds(action.timeBetweenTriggers);
                }
            }
            else
            {
                for (int i = 0; i < action.nbTriggers; i++)
                {
                    reaction.OnEventRaised(target);
                    yield return new WaitForSeconds(action.timeBetweenTriggers);
                }
            }
        }

        public Action GetAction()
        {
            return action;
        }

        public IEnumerator GetCoroutine()
        {
            return coroutine;
        }
    }
}
