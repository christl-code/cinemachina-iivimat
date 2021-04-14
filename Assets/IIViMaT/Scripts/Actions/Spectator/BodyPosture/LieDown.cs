using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat{

    /// <summary>
    /// Action to lie down on an object
    /// </summary>
    public class LieDown : Action
    {

        /// <summary>
        /// Executes reactions 
        /// </summary>
        public override void OnRaise()
        {
            Debug.Log("LieDown");
        }

        /// <summary>
        /// Stops reactions
        /// </summary>
        public override void OnStop()
        {
            
        }
    }
}
