using UnityEngine.Video;

namespace iivimat
{
    public class PlayVideo : ReactionComponent<VideoPlayer>
    {
        public bool videoLoop = true;

        public override void OnEventRaised(VideoPlayer videoPlayer)
        {   
            if(!playOnce || !finished)
            {
                if (Targets.Contains(videoPlayer))
                {
                    if (!videoPlayer.isPlaying)     
                {
                    videoPlayer.isLooping = videoLoop;
                    videoPlayer.Play();
                }

                videoPlayer.loopPointReached += OnVideoEnd;
                }
            }
        }

        public override void OnEventStopped(VideoPlayer videoPlayer)
        {
            // Debug.Log("PlayVideo stopped");
        }

        /// <summary>
        /// Method called at the end of the video (or at the end of every loop)
        /// </summary>
        public virtual void OnVideoEnd(VideoPlayer videoPlayer){
            setFinished(true);
        }
    }
}