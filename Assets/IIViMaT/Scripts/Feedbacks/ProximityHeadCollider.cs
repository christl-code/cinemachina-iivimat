using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Handles the execution of reaction linked to proximity of the head
    /// </summary>
    public class ProximityHeadCollider : MonoBehaviour
    {
        [SerializeField]
        private Action action;

        private void OnTriggerEnter(Collider other)
        {
            if(isHead(other)){
                if (action is Proximity)
                {
                    action.Raise();
                }        
                else if (action is Exit)
                {
                    action.Stop();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(isHead(other)){
                if (action is Proximity)
                {
                    action.Stop();
                }
                else if (action is Exit)
                {
                    action.Raise();
                }
            }
        }

        public void SetProximityEvent(Action action)
        {
            this.action = action;
        }

        public bool isHead(Collider other){
            return other.tag == "MainCamera";
        }
    }
}