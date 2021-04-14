using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat
{
    public class HandTeleport : Action
    {
        /// <summary>
        /// Executes reactions 
        /// </summary>
        private void OnEnable()
        {
            shallLoop = false;
        }
        public override void OnRaise()
        {
            shallLoop = false;
        }

        /// <summary>
        /// Stops reactions 
        /// </summary>
        public override void OnStop()
        {
        }
    }
}
