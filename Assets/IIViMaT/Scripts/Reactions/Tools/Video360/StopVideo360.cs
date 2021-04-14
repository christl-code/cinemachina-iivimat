using UnityEngine.Video;

namespace iivimat
{
    public class StopVideo360 : ReactionComponent<VideoPlayer>
    {

        public bool stopFollowingHead = true;

        public bool goBackToPosition = true;

        public override void OnEventRaised(VideoPlayer videoPlayer)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(videoPlayer))
                {
                    EventManager ev = InteractionsUtility.GetEventManager();

                    if(ev != null && ev.video360Reaction != null)
                    {
                        if(goBackToPosition){
                            ev.video360Reaction.goBackToOrigin();
                        }

                        if(stopFollowingHead){
                            if(ev.video360Reaction.sphere == videoPlayer.gameObject){
                            ev.video360Reaction = null;
                            }
                        }
                    }

                    if (videoPlayer.isPlaying)
                        videoPlayer.Stop();
                }
                setFinished(true);
            }
        }

        public override void OnEventStopped(VideoPlayer videoPlayer)
        {
            // Debug.Log("StopVideo stopped");
        }
    }
}