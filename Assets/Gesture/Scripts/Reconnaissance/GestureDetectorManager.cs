using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// How to use it at the end of the file
namespace iivimat
{
    public class GestureDetectorManager : MonoBehaviour
    {
        #region GESTURE DETECTION VARIABLES
        // Raises different Hand Actions
        private HandEvent handEvent;

        // Skeleton of the hand
        private OVRSkeleton skeletonRight, skeletonLeft;

        // List of gestures
        public List<Gesture> gestures;

        // List of finger's bones
        private List<OVRBone> fingerBonesRight,
            fingerBonesLeft;

        // Current gestures for both hands
        private Gesture[] currentGestureLeft = new Gesture[1],
            currentGestureRight = new Gesture[1];

        // Current gestures name for both hands
        public string nameCurrentGestureLeft = "LNOTHING",
            nameCurrentGestureRight = "RNOTHING";

        // Gesture when there is no recognition
        private Gesture defaultGesture;

        // Threshold to recognize a gesture for the distance of the fingers
        public float thresholdDistance = 0.05f;
        #endregion

        #region TELEPORTATION POINT OUT VARIABLES
        private GameObject mainCamera;
        private List<Vector3> points = new List<Vector3>();
        public float nbPoints = 100.0f;
        public float initSpeed = 10.0f; // 
        private float g = -10f;
        public GameObject currentPointedOutGOTeleportation = null;

        public Vector3 hitPoint = Vector3.zero;
        private bool actionDone = false;
        int Hand_Index1 = (int)OVRSkeleton.BoneId.Hand_Index1;
        private GameObject positionningGO = null;
        #endregion

        #region INITIALISATION OF VARIABLES (START FUNCTION)
        /// <summary>
        /// Initialize variables
        /// </summary>
        void Start()
        {
            mainCamera = Camera.main.gameObject;

            fingerBonesRight = new List<OVRBone>();
            fingerBonesLeft = new List<OVRBone>();

            skeletonRight = GameObject.FindGameObjectWithTag("RightHand").GetComponent<OVRSkeleton>();
            skeletonLeft = GameObject.FindGameObjectWithTag("LeftHand").GetComponent<OVRSkeleton>();

            defaultGesture = new Gesture() { name = "Default", fingerPositionDatas = new List<Vector3>() };
            currentGestureLeft[0] = defaultGesture;
            currentGestureRight[0] = defaultGesture;
            handEvent = ScriptableObject.CreateInstance<HandEvent>();

            positionningGO = Instantiate(
                (GameObject)Resources.Load("Prefabs/Telepad/Model/Telepad"),
                Vector3.zero, Quaternion.identity);
            positionningGO.transform.localScale = Vector3.one * 20.0f;
            positionningGO.transform.Rotate(new Vector3(90, 0, 0), Space.Self);
            positionningGO.GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(RightHandRecognition());
            StartCoroutine(LeftHandRecognition());
        }
        #endregion

        #region RECOGNITION APPLIED ON RIGHT HAND
        /// <summary>
        /// Coroutine allowing recognition on the right hand
        /// </summary>
        /// <returns></returns>
        IEnumerator RightHandRecognition()
        {
            while (true)
            {
                yield return new WaitForSeconds(0f);
                if (RecognitionProcess(fingerBonesRight, skeletonRight, currentGestureRight))
                {
                    nameCurrentGestureRight = currentGestureRight[0].name;
                    PointOutRecognitionRight(skeletonRight, currentGestureRight);
                    handEvent.compareRightHandGesture(currentGestureRight[0]);
                    PointOutRecognitionTeleportation(skeletonRight, currentGestureRight, handEvent);
                }
            }
        }
        #endregion

        #region RECOGNITION APPLIED ON LEFT HAND
        /// <summary>
        /// Coroutine allowing recognition on the left hand
        /// </summary>
        /// <returns></returns>
        IEnumerator LeftHandRecognition()
        {
            while (true)
            {
                yield return new WaitForSeconds(0f);
                if (RecognitionProcess(fingerBonesLeft, skeletonLeft, currentGestureLeft))
                {
                    nameCurrentGestureLeft = currentGestureLeft[0].name;
                    handEvent.compareLeftHandGesture(currentGestureLeft[0]);
                    PointOutRecognitionLeft(skeletonLeft, currentGestureLeft);
                    // PointOutRecognitionTeleportation(skeletonLeft, currentGestureLeft, handEvent);
                }
            }
        }
        #endregion

        #region RECOGNITION PROCESS
        /// <summary>
        /// Allows to recognize gestures performed by one of the hands or both
        /// </summary>
        /// <param name="fingerBones"></param>
        /// <param name="skeleton"></param>
        /// <param name="currentGesture"></param>
        /// <returns></returns>
        bool RecognitionProcess(List<OVRBone> fingerBones, OVRSkeleton skeleton, Gesture[] currentGesture)
        {
            if (fingerBones.Count == 0)
            {
                fingerBones = new List<OVRBone>(skeleton.Bones);
            }

            // if the hand is visible
            if (skeleton.GetComponent<OVRHand>().IsTracked)
            {
                // Recognize the movement
                currentGesture[0] = Recognize(fingerBones, skeleton);
                return true;
            }
            return false;
        }
        #endregion

        #region POINTING OUT TELEPORTATION
        public void PointOutRecognitionTeleportation(OVRSkeleton skeleton, Gesture[] currentGesture, HandEvent handEvent)
        {
            var trackedBone = skeleton.Bones[Hand_Index1];
            if (currentGesture[0].name.Contains("Teleportation"))
            {
                for (float i = 0; i < nbPoints; i += 1.0f)
                {
                    float x = trackedBone.Transform.position.x + (trackedBone.Transform.right.x) * i,
                    z = trackedBone.Transform.position.z + (trackedBone.Transform.right.z) * i;
                    points.Add(new Vector3(
                        x,
                        0.5f * g * Mathf.Pow(
                        ((x - trackedBone.Transform.position.x) /
                        ((trackedBone.Transform.right.x) * initSpeed)), 2) +
                        ((trackedBone.Transform.right.y) * initSpeed) *
                        ((x - trackedBone.Transform.position.x) /
                        ((trackedBone.Transform.right.x) * initSpeed)) +
                        trackedBone.Transform.position.y,
                        z
                    ));
                }
                actionDone = true;
                positionningGO.GetComponent<MeshRenderer>().enabled = true;
            }

            if (points.Count == nbPoints)
            {
                RaycastHit hit;
                Vector3 lastPoint = points[points.Count - 1];
                Vector3 pointBeforeLastPoint = points[points.Count - 2];
                for (int i = 1; i < points.Count - 1; i++)
                {
                    Debug.DrawLine(points[i - 1], points[i], Color.green, 1f);
                    if (Physics.Raycast(points[i - 1], (points[i] - points[i - 1]), out hit, (points[i] - points[i - 1]).magnitude))
                    {
                        currentPointedOutGOTeleportation = hit.collider.gameObject;
                        hitPoint = hit.point;
                        break;
                    }
                }
                if (currentPointedOutGOTeleportation == null)
                {
                    if (Physics.Raycast(lastPoint, (lastPoint - pointBeforeLastPoint), out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(lastPoint, (lastPoint - pointBeforeLastPoint), Color.green, 1f);
                        hitPoint = hit.point;
                        currentPointedOutGOTeleportation = hit.collider.gameObject;
                    }
                }
                positionningGO.transform.position = hitPoint;
            }

            if (!currentGesture[0].name.Contains("Teleportation") && actionDone)
            {
                Vector3 offset = GameObject.FindGameObjectWithTag("Body").transform.position - mainCamera.transform.position;
                offset.y = 0.0f;
                handEvent.Teleport(hitPoint + offset, currentPointedOutGOTeleportation);
                points.Clear();
                actionDone = false;
                currentPointedOutGOTeleportation = null;
                hitPoint = Vector3.zero;
                positionningGO.GetComponent<MeshRenderer>().enabled = false;
            }


            if (points.Count > nbPoints)
            {
                points.Clear();
            }

        }
        #endregion

        #region POINTING OUT RIGHT
        /// <summary>
        /// Launch Point Out function when "Pointer" action is recognized
        /// </summary>
        /// <param name="skeleton"></param>
        /// <param name="currentGesture"></param>
        void PointOutRecognitionRight(OVRSkeleton skeleton, Gesture[] currentGesture)
        {
            var trackedBone = skeleton.Bones[Hand_Index1];
            if (currentGesture[0].name.Contains("Pointer"))
            {
                RaycastHit hit;
                if (Physics.Raycast(trackedBone.Transform.position, trackedBone.Transform.right, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(trackedBone.Transform.position, trackedBone.Transform.right * hit.distance, Color.blue, 0.1f);
                    handEvent.comparePointedOutGORight(hit.collider.gameObject);
                }
                else
                {
                    handEvent.comparePointedOutGORight(null);
                }
            }
            else
            {
                handEvent.comparePointedOutGORight(null);
            }

        }
        #endregion

        #region POINTING OUT LEFT
        /// <summary>
        /// Launch Point Out function when "Pointer" action is recognized
        /// </summary>
        /// <param name="skeleton"></param>
        /// <param name="currentGesture"></param>
        void PointOutRecognitionLeft(OVRSkeleton skeleton, Gesture[] currentGesture)
        {
            var trackedBone = skeleton.Bones[Hand_Index1];
            if (currentGesture[0].name.Contains("Pointer"))
            {
                RaycastHit hit;
                if (Physics.Raycast(trackedBone.Transform.position, -trackedBone.Transform.right, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(trackedBone.Transform.position, -trackedBone.Transform.right * hit.distance, Color.blue, 0.1f);
                    handEvent.comparePointedOutGOLeft(hit.collider.gameObject);
                }
                else
                {
                    handEvent.comparePointedOutGOLeft(null);
                }
            }
            else
            {
                handEvent.comparePointedOutGOLeft(null);
            }

        }
        #endregion

        #region RECOGNITION FUNCTION
        /// <summary>
        /// Recognize what is the gesture 
        /// </summary>
        /// <returns>Return the gesture which is the most similar</returns>
        Gesture Recognize(List<OVRBone> fingerBones, OVRSkeleton skeleton)
        {
            // Initialize variables
            Gesture currentGesture = defaultGesture;
            float currentMinDistance = Mathf.Infinity;

            // Browse every gesture to find the most revelant 
            foreach (Gesture gesture in gestures)
            {

                float sumDistance = 0;
                bool isDiscarded = false;

                //For every bones
                for (int i = 0; i < fingerBones.Count; i++)
                {
                    // Calcul distance
                    Vector3 currentPositionData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                    float distance = Vector3.Distance(currentPositionData, gesture.fingerPositionDatas[i]);

                    //If the distance is too high, discard this gesture
                    if (distance > thresholdDistance)
                    {
                        isDiscarded = true;
                        break;
                    }

                    sumDistance += distance;

                }

                // if the gesture is not discarded and the gesture is more revelant, save it
                if (!isDiscarded && sumDistance < currentMinDistance)
                {
                    currentMinDistance = sumDistance;
                    currentGesture = gesture;
                }


            }

            //return the most revelant gesture
            return currentGesture;
        }
        #endregion

    }

}

/*
    A VERIFIER !!!!!!!!!!
    (The headset is ready and can be use in Play Mode)

     INITIALIZATION
    - Create a new GameObject and add the GestureDetector Script to it
    - Add the OVRSkelton of the right hand in the Skeleton variable
    - Add another GestureDetector Script
    - Add the OVRSkelton of the left hand in the Skeleton variable of the second Script

    SAVE GESTURE
    - Turn "Play Mode " on
    - Do the gesture with your right hand and press the key code corresponding to the save button (spacebar by default)
    - In the inspector, Gestures has been increased by one. Change the name of the gesture  whatever you want to call it
    - Start again the two last indication to save as many gestures as you want
    - WARNING ! Before leaving the player mode, right click on the GestureDetector Script and "Copy Component"
    - Turn "Play Mode " off
    - Right click on the GestureDetector Script and " Paste Component as Values".
    - You have now your gestures in your script for the right hand! 


    - Let's do it again with the left hand. Do the "SAVE GESTURE" part with the left hand
    - You have now your gestures in your script for both hands! 
     
     */
