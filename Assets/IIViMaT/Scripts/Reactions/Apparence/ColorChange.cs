using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Changes the color of a specific GameObject
    /// </summary>
    public class ColorChange : ReactionComponent<Renderer>
    {
        [Tooltip("If enabled, the color will be randomized.")]
        public bool randomColor = true;

        [Tooltip("The color the object will have after reacting.")]
        public Color color;

        public override void OnEventRaised(Renderer rend)
        {
            if(!playOnce || !finished){
                if (Targets.Contains(rend))
                {
                    rend.material.color = randomColor
                            ? new Color(Random.Range(0, 10) * 25 / 255f, Random.Range(0, 10) * 25 / 255f, Random.Range(0, 10) * 25 / 255f)
                            : color;
                }
            }
        }

        public override void OnEventStopped(Renderer rend)
        {
            // Debug.Log("ColorChange stopped");
        }
    }
}