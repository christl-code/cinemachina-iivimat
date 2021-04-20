using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// This réaction allows to teleport the spectator to a specific position represents by a transform.
    /// </summary>
    public class Teleportation : ReactionComponent<Transform>
    {
        public override void OnEventRaised(Transform transform)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(transform))
                {
                    Vector3 bodyPosition = GameObject.FindGameObjectWithTag("Body").transform.position ;
                    Vector3 headPosition = Camera.main.transform.position;

                    Vector3 offset = new Vector3(bodyPosition.x-headPosition.x, 0, bodyPosition.z-headPosition.z);

                    GameObject.FindGameObjectWithTag("Body").transform.position = transform.position + offset;
                }
            }
        }

        public override void OnEventStopped(Transform transform)
        {
            // Debug.Log("Teleportation stopped");
        }
    }
}