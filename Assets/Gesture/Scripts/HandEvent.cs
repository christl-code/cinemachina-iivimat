using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat
{

    public class HandEvent : ScriptableObject
    {
        private List<HandGestureLeft> leftHandGestures = new List<HandGestureLeft>();
        private List<HandGestureRight> rightHandGestures = new List<HandGestureRight>();
        private List<PointOutRight> pointedOutRightElements = new List<PointOutRight>();
        private List<PointOutLeft> pointedOutLeftElements = new List<PointOutLeft>();
        private List<HandTeleport> handTeleportElements = new List<HandTeleport>();
        public Gesture currentDetectedGestureL = new Gesture();
        public Gesture currentDetectedGestureR = new Gesture();
        public GameObject pointedOutGOLeft = null, pointedOutGORight = null;
        /// <summary>
        /// Changes the current  left hand gesture
        /// </summary>
        /// <param name="newGestureL"></param>
        public void ChangeLeftGesture(Gesture newGestureL)
        {
            // Stops
            leftHandGestures.ForEach(element => element.Stop());

            // Raises
            if (!currentDetectedGestureL.Equals(default))
            {
                leftHandGestures = GameObject.FindGameObjectWithTag("Meta").GetComponent<GlobalActions>().actions.OfType<HandGestureLeft>().ToList();

                if (leftHandGestures.Any())
                {
                    foreach (HandGestureLeft lefthandGesture in leftHandGestures)
                    {
                        lefthandGesture.compareHandGesture(newGestureL.name);
                    }
                }
            }

            currentDetectedGestureL = newGestureL;
        }

        /// <summary>
        /// Changes the current right gesture
        /// </summary>
        /// <param name="newGestureR"></param>

        public void ChangeRightGesture(Gesture newGestureR)
        {
            // Stops
            rightHandGestures.ForEach(element => element.Stop());

            // Raises
            if (!currentDetectedGestureR.Equals(default))
            {
                rightHandGestures = GameObject.FindGameObjectWithTag("Meta").GetComponent<GlobalActions>().actions.OfType<HandGestureRight>().ToList();
                if (rightHandGestures.Any())
                {
                    foreach (HandGestureRight righthandGesture in rightHandGestures)
                    {
                        righthandGesture.compareHandGesture(newGestureR.name);
                    }
                }
            }
            currentDetectedGestureR = newGestureR;
        }

        /// <summary>
        /// Changes the current pointed out gameobject with the right hand
        /// </summary>
        /// <param name="current"></param>
        public void ChangePointedGameObjectRight(GameObject current)
        {
            // Stops
            pointedOutRightElements.ForEach(element => element.Stop());

            // Raises
            if (current != null)
            {
                pointedOutRightElements = current.GetComponent<LocalActions>().Actions.OfType<PointOutRight>().ToList();
                if (pointedOutRightElements.Any())
                {
                    pointedOutRightElements.ForEach(element => element.Raise());
                }
            }

            pointedOutGORight = current;

        }

        /// <summary>
        /// Changes the current pointed out gameobject with the right hand
        /// </summary>
        /// <param name="current"></param>
        public void ChangePointedGameObjectLeft(GameObject current)
        {
            // Stops
            pointedOutLeftElements.ForEach(element => element.Stop());

            // Raises
            if (current != null)
            {
                pointedOutLeftElements = current.GetComponent<LocalActions>().Actions.OfType<PointOutLeft>().ToList();
                if (pointedOutLeftElements.Any())
                {
                    pointedOutLeftElements.ForEach(element => element.Raise());
                }
            }

            pointedOutGOLeft = current;

        }

        /// <summary>
        /// Teleports the maincamera to the hitpoint
        /// </summary>
        /// <param name="hitPoint"></param>
        /// <param name="current"></param>
        public void Teleport(Vector3 hitPoint, GameObject current)
        {
            if (current != null)
            {
                handTeleportElements = current.GetComponent<LocalActions>().Actions.OfType<HandTeleport>().ToList();
                if (handTeleportElements.Any())
                {
                    GameObject.FindGameObjectWithTag("Body").transform.position +=
                    ((hitPoint - GameObject.FindGameObjectWithTag("Body").transform.position));
                }
            }
        }

        /// <summary>
        /// Compares the current pointed out gameobject with the left hand with the previous one
        /// </summary>
        /// <param name="current"></param>
        public void comparePointedOutGOLeft(GameObject current)
        {
            if (this.pointedOutGOLeft != current)
            {
                this.ChangePointedGameObjectLeft(current);
            }
        }
        /// <summary>
        /// Compares the current pointed out gameobject with the right hand with the previous one
        /// </summary>
        /// <param name="current"></param>
        public void comparePointedOutGORight(GameObject current)
        {
            if (this.pointedOutGORight != current)
            {
                this.ChangePointedGameObjectRight(current);
            }
        }

        /// <summary>
        /// Compares the current left hand gesture with the previous one
        /// </summary>
        /// <param name="current"></param>
        public void compareLeftHandGesture(Gesture currentGestureL)
        {
            if (currentDetectedGestureL.name != currentGestureL.name)
            {
                ChangeLeftGesture(currentGestureL);
            }
        }
        /// <summary>
        /// Compares the current right hand gesture with the previous one
        /// </summary>
        /// <param name="current"></param>
        public void compareRightHandGesture(Gesture currentGestureR)
        {
            if (currentDetectedGestureR.name != currentGestureR.name)
            {
                ChangeRightGesture(currentGestureR);
            }
        }
    }

}
