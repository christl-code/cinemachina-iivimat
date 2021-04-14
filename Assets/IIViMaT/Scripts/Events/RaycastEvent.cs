using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Handles every action which needs a raycast
    /// </summary>
    public class RaycastEvent : ScriptableObject
    {
        public GameObject lookedElement = null;
        private List<LookAway> looksAwayOldElement = new List<LookAway>();
        private List<LookAt> looksAtCurrentlement = new List<LookAt>();

        /// <summary>
        /// Changes lookedElement by current which is an object reached by the ray from the "EventManager"
        /// </summary>
        /// <param name="current"></param>
        public void ChangeLookedElement(GameObject current)
        {
            // Stops
            looksAwayOldElement.ForEach(element => element.Stop());
            looksAtCurrentlement.ForEach(element => element.Stop());

            // Raises
            if (lookedElement != null)
            {
                looksAwayOldElement = lookedElement.GetComponent<LocalActions>().Actions.OfType<LookAway>().ToList();
                if (looksAwayOldElement.Any())
                {
                    looksAwayOldElement.ForEach(element => element.Raise());
                }
            }
            if (current != null)
            {
                looksAtCurrentlement = current.GetComponent<LocalActions>().Actions.OfType<LookAt>().ToList();
                if (looksAtCurrentlement.Any())
                {
                    looksAtCurrentlement.ForEach(element => element.Raise());
                }
            }

            lookedElement = current;
        }
    }
}
