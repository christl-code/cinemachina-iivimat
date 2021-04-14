using UnityEngine.Video;

namespace iivimat
{
    public class PauseVideo360 : ReactionComponent<VideoPlayer>
    {
        public bool stopFollowingHead = true;

        public bool goBackToPosition = false;

        public override void OnEventRaised(VideoPlayer videoPlayer)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(videoPlayer))
                {
                    if(stopFollowingHead){
                        if(InteractionsUtility.GetEventManager().video360Reaction.sphere == videoPlayer.gameObject){
                            InteractionsUtility.GetEventManager().video360Reaction = null;
                        }
                    }

                    if (videoPlayer.isPlaying)
                        videoPlayer.Pause();
                }
                setFinished(true);
            }
        }

        public override void OnEventStopped(VideoPlayer videoPlayer)
        {
            // Debug.Log("PauseVideo stopped");
        }
    }
}