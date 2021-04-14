using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace iivimat{
    
    /// <summary>
    /// This class checks the state of the VideoPlayer which are in OnVideoEnd actions.
    /// If a VideoPlayer ends, raise the OnVideoEnd action with this VideoPlayer in input.
    ///
    /// This class checks the state of the AudioSource which are in OnAudioEnd actions.
    /// If an AudioSource ends, raise the OnAudioEnd action with this AudioSource in input.
    /// </summary>
    public class EventEndHandler : MonoBehaviour
    {


        // List of VideoPlayer which are in an OnVideoEnd action
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


        // List of AudioSource which are in an OnAudioEnd action
        [SerializeField]
        private List<AudioSource> audioSources;
        public List<AudioSource> AudioSources
        {
            get
            {
                if (audioSources == null)
                    audioSources = new List<AudioSource>();
                return audioSources;
            }
        }

        void Update(){
            CheckVideoHasEnded();
            CheckAudioHasEnded();
        }

        //////////////////////////////////////////////////
        /// Video related
        //////////////////////////////////////////////////

        
        /// <summary>
        /// Add a VideoPlayer to the VideoPlayers list
        /// </summary>
        public void AddVideoPlayer(VideoPlayer vp){
            if(!VideoPlayers.Contains(vp)){
                VideoPlayers.Add(vp);
            }
        }

        /// <summary>
        /// Remove a VideoPlayer to the VideoPlaye List
        /// </summary>
        public void RemoveVideoPlayer(VideoPlayer vp){
            if(VideoPlayers.Contains(vp)){
                VideoPlayers.Remove(vp);

            }
        }

        /// <summary>
        /// At the end of a video (or a loop), get all the actions, if the action is an OnVideoEnd action, check if its contains the VideoPlayer which has ended.
        /// Raise this action.
        /// </summary>
        private void CheckVideoHasEnded(){
            foreach(VideoPlayer vp in VideoPlayers){
                long playerFrameCount = System.Convert.ToInt64(vp.frameCount);
                long playerCurrentFrame = vp.frame + 2;

                // If the video is ended
                if((playerCurrentFrame % playerFrameCount) == 0)
                {
                    List<Action> actions = InteractionsUtility.GetInteractionsSaver().actions;

                    // If the action is an OnVideoEnd action
                    foreach(Action a in actions){

                        if(a is OnVideoEnd){
                            OnVideoEnd ove = (OnVideoEnd) a;

                            // If the action contains this video player, raise the event
                            if(ove.VideoPlayers.Contains(vp)){
                                ove.Raise();
                            }
                        }
                    }
                }
            }
        }

        //////////////////////////////////////////////////
        /// Audio related
        //////////////////////////////////////////////////


        /// <summary>
        /// Add an AudioSource to the AudioSources list
        /// </summary>
        public void AddAudioSource(AudioSource a){
            if(!AudioSources.Contains(a)){
                AudioSources.Add(a);
            }
        }

        /// <summary>
        /// Remove a AudioSource to the AudioSources List
        /// </summary>
        public void RemoveAudioSource(AudioSource a){
            if(AudioSources.Contains(a)){
                AudioSources.Remove(a);

            }
        }

        /// <summary>
        /// At the end of an audio clip (or a loop), get all the actions, if the action is an OnAudioEnd action, check if its contains the AudioSOurce which has ended.
        /// Raise this action.
        /// </summary>
        private void CheckAudioHasEnded(){
            foreach(AudioSource s in AudioSources){
                int audioCurrentSample = s.timeSamples + 1000;
                int audioSamplesCount = s.clip.samples;

                // If the audio is ended
                if((audioCurrentSample % audioSamplesCount) < 1000)
                {
                    List<Action> actions = InteractionsUtility.GetInteractionsSaver().actions;

                    // If the action is an OnAudioEnd action
                    foreach(Action a in actions){

                        if(a is OnAudioEnd){
                            OnAudioEnd oae = (OnAudioEnd) a;

                            // If the action contains this audio clip, raise the event
                            if(oae.AudioSources.Contains(s)){
                                oae.Raise();
                            }
                        }
                    }
                }
            }
        }


    }
}
