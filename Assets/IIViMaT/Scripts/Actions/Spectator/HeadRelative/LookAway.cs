
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Executes reactions when the spectator does not look at a specific GameObject
    /// </summary>
    public class LookAway : Action
    {
        /// <summary>
        /// Executes reactions
        /// </summary>        
        public override void OnRaise()
        {
        }

        /// <summary>
        /// Stops reactions
        /// </summary>
        public override void OnStop()
        {
        }
    }
}