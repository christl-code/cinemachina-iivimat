using UnityEngine;

namespace iivimat
{
    public class Visibility : ReactionComponent<Renderer>
    {
        public enum VisibilityOptions
        {
            Toggle,
            Hidden,
            Visible
        }

        [Tooltip("The visibility of the object after reacting.\n" +
                    "Toggle: Hidden becomes Visible, Visible becomes Hidden\n" +
                    "Hidden: Hide object\n" +
                    "Visible: Show object")]
        public VisibilityOptions visibility = VisibilityOptions.Hidden;

        public override void OnEventRaised(Renderer rend)
        {
            if(!playOnce || !finished)
            {

                if (Targets.Contains(rend))
                {
                    switch (visibility)
                    {
                        case VisibilityOptions.Toggle:
                            rend.enabled = !rend.enabled;
                            break;
                        case VisibilityOptions.Hidden:
                            rend.enabled = false;
                            break;
                        case VisibilityOptions.Visible:
                            rend.enabled = true;
                            break;
                        default:
                            break; //throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        public override void OnEventStopped(Renderer rend)
        {
            // Debug.Log("Visibility stopped");
        }
    }
}