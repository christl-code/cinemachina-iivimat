using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat{

    /// <summary>
    /// Action to sit on an object
    /// </summary>
    public class Sit : Action
    {
        /// <summary>
        /// Executes reactions 
        /// </summary>
        public override void OnRaise()
        {
            Debug.Log("Sit");
        }

        /// <summary>
        /// Stops reactions
        /// </summary>
        public override void OnStop()
        {
            
        }
    }
}
