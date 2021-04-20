using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Activate/Deactivate a GameObject in the Hierarchy
    /// </summary>
    public class Activation : ReactionObject<GameObject>
    {
        public enum ActivationOptions
        {
            Enabled,
            Disabled,
            Toggle
        }

        public ActivationOptions activation;

        public override void OnEventRaised(GameObject target)
        {
            if(!playOnce || !finished){
                switch(activation){
                    case ActivationOptions.Toggle:
                        target.SetActive(!target.activeSelf);
                        break;
                    case ActivationOptions.Disabled:
                        target.SetActive(false);
                        break;
                    case ActivationOptions.Enabled:
                        target.SetActive(true);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void OnEventStopped(GameObject target)
        {
            // Debug.Log("Activation stopped");
        }
    }
}