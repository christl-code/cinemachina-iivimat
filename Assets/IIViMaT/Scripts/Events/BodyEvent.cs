using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Handles every action which needs a raycast
    /// </summary>
    public class BodyEvent : ScriptableObject
    {
        public SpectatorVariables.Posture currentPosition = SpectatorVariables.Posture.LieDown;
        public GameObject currentGO = null;
        private List<StandUp> StandUpOnElement = new List<StandUp>();
        private List<Sit> SitOnElement = new List<Sit>();
        private List<Crouch> CrouchOnElement = new List<Crouch>();
        private List<LieDown> LieDownOnElement = new List<LieDown>();
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        public void PositionOnElement(SpectatorVariables.Posture position, GameObject current)
        {
            // Stops
            StandUpOnElement.ForEach(element => element.Stop());
            SitOnElement.ForEach(element => element.Stop());
            CrouchOnElement.ForEach(element => element.Stop());
            LieDownOnElement.ForEach(element => element.Stop());

            // Raises
            if (current != null)
            {
                switch (position)
                {
                    case SpectatorVariables.Posture.StandUp:
                        StandUpOnElement = current.GetComponent<LocalActions>().Actions.OfType<StandUp>().ToList();
                        if (StandUpOnElement.Any())
                        {
                            StandUpOnElement.ForEach(element => element.Raise());
                        }   
                    break;

                    case SpectatorVariables.Posture.Crouch:
                        CrouchOnElement = current.GetComponent<LocalActions>().Actions.OfType<Crouch>().ToList();
                        if (CrouchOnElement.Any())
                        {
                            CrouchOnElement.ForEach(element => element.Raise());
                        }
                    break;

                    case SpectatorVariables.Posture.Sit:
                        SitOnElement = current.GetComponent<LocalActions>().Actions.OfType<Sit>().ToList();
                        if (SitOnElement.Any())
                        {
                            SitOnElement.ForEach(element => element.Raise());
                        }
                    break;

                    case SpectatorVariables.Posture.LieDown:
                        LieDownOnElement = current.GetComponent<LocalActions>().Actions.OfType<LieDown>().ToList();
                        if (LieDownOnElement.Any())
                        {
                            LieDownOnElement.ForEach(element => element.Raise());
                        }
                    break;
                    
                    default:
                        
                    break;
                }
            }
            currentGO = current;
            currentPosition = position;
        }
    }
}
