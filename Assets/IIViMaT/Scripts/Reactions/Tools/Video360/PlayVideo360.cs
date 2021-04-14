using UnityEngine.Video;
using UnityEngine;
using System.Collections;

namespace iivimat{
    public class PlayVideo360 : ReactionComponent<VideoPlayer>
    {
        public bool videoLoop = true;
        public bool fixeVideoToHead = false;

        public bool videoBackToInitialPosition = false;

        private Vector3 originPosition;
        private Quaternion originRotation;

        public GameObject sphere;

        private EventManager ev;

        public override void OnEventRaised(VideoPlayer videoPlayer)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(videoPlayer))
                {
                    // Save originals values
                    sphere = videoPlayer.gameObject;
                    originPosition = sphere.transform.position;
                    originRotation = sphere.transform.rotation;

                    // if fixed to the head
                    if (fixeVideoToHead){
                        ev = InteractionsUtility.GetEventManager();
                        if(ev != null){
                            ev.video360Reaction = this;
                        }
                    }

                    if (!videoPlayer.isPlaying)     
                    {
                        videoPlayer.isLooping = videoLoop;
                        videoPlayer.Play();
                    }
                }
                videoPlayer.loopPointReached += OnVideoEnd;
                setFinished(true);

            }

        }        
        
        public override void OnEventStopped(VideoPlayer videoPlayer)
        {
            // Debug.Log("PlayVideo stopped");
        }

        /// <summary>
        /// Method attached to the event loopPointReached of the videoPlayer. 
        /// Method called at the end of a loop or at the end of the video (is loop = false) 
        /// </summary>
        public void OnVideoEnd(VideoPlayer videoPlayer){

            if(fixeVideoToHead){
                if(ev != null){
                    ev.video360Reaction = null;
                }
            }

            if(videoBackToInitialPosition){
                goBackToOrigin();
            }
        }

        /// <summary>
        /// Put the 360 sphere gameObject to his original position and rotation
        /// </summary>
        public void goBackToOrigin(){
            sphere.transform.position = originPosition;
            sphere.transform.rotation = originRotation;
        }
    }
}