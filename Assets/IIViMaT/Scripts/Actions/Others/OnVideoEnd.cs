using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace iivimat
{
    /// <summary>
    /// Executes reactions when one of the video in inputs end
    /// </summary>
    [Serializable]
    public class OnVideoEnd : Action
    {
        [SerializeField]
        private List<VideoPlayer> videoPlayers;

        public List<VideoPlayer> VideoPlayers
        {
            get
            {
                if (videoPlayers == null)
                    videoPlayers = new List<VideoPlayer>();
                return videoPlayers;
            }
        }

        /// <summary>
        /// Get all VideoPlayer in the GameObject in input
        /// </summary>
        private void OnEnable()
        {
            VideoPlayers.Clear();
            foreach (GameObject go in Objects)
            {
                VideoPlayer vp  = go.GetComponent<VideoPlayer>();
                if(vp != null){
                    videoPlayers.Add(vp);
                }
            }
        }

        /// <summary>
        /// Add the VideoPlayer of the game object, if it contains a VideoPlayer, to the EventEndHandler 
        /// </summary>
        /// <param name="go"> GameObject with an VideoPlayer</param>
        public override void OnAddGameObject(GameObject go)
        {
            VideoPlayer vp  = go.GetComponent<VideoPlayer>();
            if(vp != null){
                InteractionsUtility.GetEventEndHandler().AddVideoPlayer(vp);
            }
        }

        /// <summary>
        /// Remove the VideoPlayer of the game object, if it contains a VideoPlayer, to the EventEndHandler 
        /// </summary>
        /// <param name="go"> GameObject with an VideoPlayer</param>
        public override void OnRemoveGameObject(GameObject go)
        {
            VideoPlayer vp  = go.GetComponent<VideoPlayer>();
            if(vp != null){
                InteractionsUtility.GetEventEndHandler().RemoveVideoPlayer(vp);
            } 
        }
    }
}