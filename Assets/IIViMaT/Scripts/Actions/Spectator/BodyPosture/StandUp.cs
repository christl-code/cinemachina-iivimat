using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat{

    /// <summary>
    /// Action to stand up above an object
    /// </summary>
    public class StandUp : Action
    {
        /// <summary>
        /// Executes reactions 
        /// </summary>
        public override void OnRaise()
        {
            Debug.Log("StandUp");
        }

        /// <summary>
        /// Stops reactions
        /// </summary>
        public override void OnStop()
        {
            
        }
    }
}

