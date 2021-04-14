using System.Collections;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    ///Executes reactions X time after the launch of the app
    /// </summary>
    public class ProjectClock : Action
    {
        public float triggerTime = 1f;
        private float timePlayMode;

        public IEnumerator clockCoroutine;
        private GlobalActions globalActions;

        /// <summary>
        /// Executes a reaction triggerTime after the beginning
        /// </summary>
        /// <returns></returns>
        private IEnumerator ClockCoroutine()
        {
            timePlayMode = Time.time;
            yield return new WaitWhile(() => Time.time < (timePlayMode + triggerTime));
            Raise();
        }

        /// <summary>
        /// Adds this action when one calls it on the IIVIMAT graph system, if it does not belong
        /// to the actions of globalActions
        /// </summary>
        private void OnEnable()
        {
            if (globalActions == null)
                globalActions = FindObjectOfType<GlobalActions>();
            if (!globalActions.actions.Contains(this))
                globalActions.actions.Add(this);
                clockCoroutine = ClockCoroutine();

        }

        /// <summary>
        /// Removes this action from the list of actions of globalActions
        /// </summary>
        private void OnDisable()
        {
            if (globalActions.actions.Contains(this))
            {
                globalActions.actions.Remove(this);
                globalActions.Clean();
            }
        }
    }
}