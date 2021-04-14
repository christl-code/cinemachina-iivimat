using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Handles the execution of reaction linked to proximity of the hand
    /// </summary>
    public class ProximityHandCollider : MonoBehaviour
    {
        [SerializeField]
        private Action action;

        private void OnTriggerEnter(Collider other)
        {
            if(isHand(other)){
                if (action is ProximityHand)
                {
                    action.Raise();
                }        
                else if (action is ExitHand)
                {
                    action.Stop();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(isHand(other)){
                if (action is ProximityHand)
                {
                    action.Stop();
                }
                else if (action is ExitHand)
                {
                    action.Raise();
                }
            }
        }

        public void SetProximityEvent(Action action)
        {
            this.action = action;
        }

        public bool isHand(Collider other){
            return other.tag == "LeftHand" || other.tag == "RightHand";
        }
    }
}