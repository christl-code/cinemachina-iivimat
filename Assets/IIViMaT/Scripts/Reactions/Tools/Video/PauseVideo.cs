using UnityEngine.Video;

namespace iivimat
{
    public class PauseVideo : ReactionComponent<VideoPlayer>
    {
        public override void OnEventRaised(VideoPlayer videoPlayer)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(videoPlayer))
                {
                    if (videoPlayer.isPlaying)
                        videoPlayer.Pause();
                    setFinished(true);
                }
            }
        }

        public override void OnEventStopped(VideoPlayer videoPlayer)
        {
            // Debug.Log("PauseVideo stopped");
        }
    }
}