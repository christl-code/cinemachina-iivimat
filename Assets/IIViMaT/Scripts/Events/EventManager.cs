using UnityEngine;
using UnityEngine.Video;
using System.Collections;

namespace iivimat
{
    /// <summary>
    /// Handles the detection of actions which need an update method
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        private RaycastEvent raycastEvent;
        private BodyEvent bodyEvent;

        /// Camera
        protected Camera mainCamera;

        // Layer to ignore rayCast
        private int layerMask;

        public SpectatorVariables spectatorVariables;
    

        // Sphere 360 which is following the head, only one at once (because why more ?)
        public PlayVideo360 video360Reaction;
    
        void Awake()
        {
            raycastEvent = ScriptableObject.CreateInstance<RaycastEvent>();
            bodyEvent = ScriptableObject.CreateInstance<BodyEvent>();
            // spectatorVariables.SetInitialPosition(transform.position);
            // Bit shift the index of the layer (2) to get a bit mask
            layerMask = 1 << 2;

            // This would cast rays only against colliders in layer 2.
            // But instead we want to collide against everything except layer 2. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;
        }

        /// <summary>
        /// Load the SpectatorVariables from ressources and init the body height
        /// </summary>
        void Start() {
            spectatorVariables = Resources.Load<SpectatorVariables>("IIVIMATAssets/IIVIMATSpectVar");
            StartCoroutine(HeightInit());
            mainCamera = Camera.main;

            InteractionsUtility.GetGlobalActions().StartAllCoroutine();
        }

        
        // Every frame, one casts a ray from the head of the spectator toward "infinite" 
        // and change lookElement by the current looked GameObject
        // For the recognition of the spectator's posture, one checks the spectator's height
        // and a ray is cast from the head to the ground to check where the spectator does a posture
        
        /// <summary>
        /// Checks every action which one has to watch such as lookAt, lookAway, every posture of the body  
        /// </summary>
        void Update()
        {
            FollowHead360Sphere();
            UpdateBody();
        }

        /// <summary>
        /// if a sphere is attached to the head, follow it  
        /// </summary>
        private void FollowHead360Sphere(){
            if(video360Reaction != null){
                video360Reaction.sphere.transform.position = mainCamera.transform.position;
            }
        }

        /// <summary>
        /// Update for the body
        /// </summary>
        public void UpdateBody(){

            // Look At and Look Away
            RaycastHit hitElement;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward), out hitElement, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward) * hitElement.distance, Color.yellow);
                // If the user look at a new object
                if (raycastEvent.lookedElement != hitElement.collider.gameObject)
                {
                    raycastEvent.ChangeLookedElement(hitElement.collider.gameObject);
                }
            }
            else
            {
                // If the user look at the sky
                if (raycastEvent.lookedElement != null)
                {
                    raycastEvent.ChangeLookedElement(null);
                }
            }

            // Body Postures, update every values about body (position, rotation and speed)

            // If the raycast from the head to the ground hit an object
            RaycastHit hitElementBodyPosture;
            if (Physics.Raycast(mainCamera.transform.position, Vector3.down, out hitElementBodyPosture, 2 * spectatorVariables.initialPosition.y, layerMask))
            {
                Debug.DrawRay(mainCamera.transform.position, Vector3.down * hitElementBodyPosture.distance, Color.blue);
                spectatorVariables.updateBody(mainCamera.transform,hitElementBodyPosture.distance, hitElementBodyPosture.collider.gameObject);

                // Check if it's a new posture of the body
                spectatorVariables.ComputePosition(hitElementBodyPosture.distance);

                // If the GameObject under the body is different or the position has changed, raised the event
                if((bodyEvent.currentGO != hitElementBodyPosture.collider.gameObject) || (bodyEvent.currentPosition != spectatorVariables.posture)){
                    bodyEvent.PositionOnElement(spectatorVariables.posture, hitElementBodyPosture.collider.gameObject);
                }
            }
        }

        ///<summary>
        /// At the beginning of the app, wait 1 second and initialise the person with his height
        ///</summary>
        private IEnumerator HeightInit()
        {
            spectatorVariables.SetInitialPosition(new Vector3(0,0,0));
            yield return new WaitForSeconds(1);    
            spectatorVariables.SetInitialPosition(mainCamera.transform.position);
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear(){
            video360Reaction = null;
        }
    }
}