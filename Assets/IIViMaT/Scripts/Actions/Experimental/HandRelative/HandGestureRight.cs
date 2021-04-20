using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat{
    public class HandGestureRight : Action
    {

        public enum Gesture{
            RRockeur, RSerrer
        }

        public Gesture currentGesture;
        private GlobalActions globalActions;
        
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
        
        /// <summary>
        /// Executes reactions 
        /// </summary>
        public override void OnRaise()
        {
            
        }
        /// <summary>
        /// Compares the name of hand gesture
        /// </summary>
        /// <param name="namegesture"></param>
        public void compareHandGesture(string namegesture){
            if(currentGesture.ToString() == namegesture){
                Raise();
            }
        }

        /// <summary>
        /// Stops reactions 
        /// </summary>
        public override void OnStop()
        {
        }
    }
}
