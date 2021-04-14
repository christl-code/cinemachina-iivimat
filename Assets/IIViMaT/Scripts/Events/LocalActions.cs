using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// list of all actions linked to a specific GameObject
    /// </summary>
    public class LocalActions : MonoBehaviour
    {
        [SerializeField]
        private List<Action> actions;
        public List<Action> Actions
        {
            get
            {
                if (actions == null)
                    actions = new List<Action>();
                return actions;
            }
        }
        /// <summary>
        /// Removes all the null actions when this local action is called on the IIVIMAT graph system
        /// </summary>
        private void OnEnable()
        {
            Clean();
        }



        /// <summary>
        /// Adds "action" to the list "actions" 
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(Action action)
        {
            Clean();
            if (!Actions.Contains(action))
            {
                Actions.Add(action);
            }
        }

        /// <summary>
        /// Removes "action" from the list "actions"
        /// </summary>
        /// <param name="action"></param>
        public void RemoveAction(Action action)
        {
            if (Actions.Contains(action))
            {
                Actions.Remove(action);
            }
            Clean();
        }
        /// <summary>
        /// Removes all the null actions
        /// </summary>
        public void Clean()
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i] == null)
                {
                    Actions.RemoveAt(i);
                }
            }
        }

        public void Clear(){
            Actions.Clear();
        }
    }
}