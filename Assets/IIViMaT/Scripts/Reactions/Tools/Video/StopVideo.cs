using UnityEngine.Video;

namespace iivimat
{
    public class StopVideo : ReactionComponent<VideoPlayer>
    {
        public override void OnEventRaised(VideoPlayer videoPlayer)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(videoPlayer))
                {
                    if (videoPlayer.isPlaying)
                        videoPlayer.Stop();
                    setFinished(true);
                }
            }
        }

        public override void OnEventStopped(VideoPlayer videoPlayer)
        {
            // Debug.Log("StopVideo stopped");
        }
    }
}