using System;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Executes reactions when the spectator enters a proximity collider
    /// </summary>
    [Serializable]
    public class Proximity : Action
    {
        [SerializeField]
        private List<GameObject> colliders;

        public List<GameObject> Colliders
        {
            get
            {
                if (colliders == null)
                    colliders = new List<GameObject>();
                return colliders;
            }
        }

        /// <summary>
        /// Adds proximity colliders found among children GameObjects from each "go" contained in Objects
        /// </summary>
        private void OnEnable()
        {
            Colliders.Clear();
            foreach (GameObject go in Objects)
            {
                ProximityHeadCollider[] ProximityHeadColliders = go.GetComponentsInChildren<ProximityHeadCollider>();
                if (ProximityHeadColliders != null)
                {
                    foreach (ProximityHeadCollider proximityHeadCollider in ProximityHeadColliders)
                    {
                        GameObject collider = proximityHeadCollider.gameObject;
                        if (!Colliders.Contains(collider))
                        {
                            Colliders.Add(collider);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Hides all proximity colliders represented by spheres in PlayMode 
        /// </summary>
        public override void OnEnterPlayMode()
        {
            Colliders.ForEach(collider => collider.GetComponent<MeshRenderer>().enabled = false);
        }

        /// <summary>
        /// Displays all proximity colliders represented by spheres in PlayMode 
        /// </summary>
        public override void OnEnterEditMode()
        {
            Colliders.ForEach(collider => collider.GetComponent<MeshRenderer>().enabled = true);
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
            Colliders.Add(collider);
        }

        /// <summary>
        /// Removes Proximity colliders from "go" and from the list "colliders" 
        /// </summary>
        /// <param name="go"></param>
        public override void OnRemoveGameObject(GameObject go)
        {
            ProximityHeadCollider[] ProximityHeadColliders = go.GetComponentsInChildren<ProximityHeadCollider>();
            if (ProximityHeadColliders != null)
            {
                foreach (ProximityHeadCollider proximityHeadCollider in ProximityHeadColliders)
                {
                    GameObject collider = proximityHeadCollider.gameObject;
                    if (Colliders.Contains(collider))
                    {
                        Colliders.Remove(collider);
                        DestroyImmediate(collider, true);
                    }
                }
            }
        }
    }
}