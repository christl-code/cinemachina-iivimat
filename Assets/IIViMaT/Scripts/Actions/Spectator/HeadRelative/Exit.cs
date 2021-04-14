using System.Collections.Generic;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Executes reactions when the spectator leaves the proximity collider
    /// </summary>
    public class Exit : Action
    {
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
        }

        List<GameObject> colliders = new List<GameObject>();

        /// <summary>
        /// Hides all proximity colliders represented by spheres in PlayMode 
        /// </summary>
        public override void OnEnterPlayMode()
        {
            colliders.ForEach(collider => collider.GetComponent<MeshRenderer>().enabled = false);
        }

        /// <summary>
        /// Displays all proximity colliders represented by spheres in EditMode
        /// </summary>
        public override void OnEnterEditMode()
        {
            colliders.ForEach(collider => collider.GetComponent<MeshRenderer>().enabled = true);
        }

        /// <summary>
        /// Instantiates a Proximity collider at the position of "go"
        /// and adds it to the list "colliders"
        /// </summary>
        /// <param name="go"></param>
        public override void OnAddGameObject(GameObject go)
        {
            GameObject collider = Instantiate(Resources.Load<GameObject>("Prefabs/Feedbacks/ProximityHeadCollider"), go.transform.position, Quaternion.identity, go.transform);
            collider.GetComponent<ProximityHeadCollider>().SetProximityEvent(this);
            colliders.Add(collider);
        }

        /// <summary>
        /// Removes Proximity colliders from "go" and from the list "colliders" 
        /// </summary>
        /// <param name="go"></param>
        public override void OnRemoveGameObject(GameObject go)
        {
            ProximityHeadCollider[] exitColliders = go.GetComponentsInChildren<ProximityHeadCollider>();
            if (exitColliders != null)
            {
                foreach (ProximityHeadCollider exitCollider in exitColliders)
                {
                    GameObject collider = exitCollider.gameObject;
                    if (colliders.Contains(collider))
                    {
                        colliders.Remove(collider);
                        DestroyImmediate(collider, true);
                    }
                }
            }
        }
    }
}