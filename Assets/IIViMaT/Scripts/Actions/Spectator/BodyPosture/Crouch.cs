using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat{

    /// <summary>
    /// Action to crouch above an object
    /// </summary>
    public class Crouch : Action
    {
        /// <summary>
        /// Executes reactions 
        /// </summary>
        public override void OnRaise()
        {
            Debug.Log("Crouch");
        }

        /// <summary>
        /// Stops reactions
        /// </summary>
        public override void OnStop()
        {
            
        }
    }
}
