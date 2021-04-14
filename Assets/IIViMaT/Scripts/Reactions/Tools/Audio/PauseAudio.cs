using UnityEngine;

namespace iivimat
{
    public class PauseAudio : ReactionComponent<AudioSource>
    {
        public override void OnEventRaised(AudioSource audioSource)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(audioSource))
                {
                    if (audioSource.isPlaying)
                        audioSource.Pause();
                    setFinished(true);
                }
            }
        }

        public override void OnEventStopped(AudioSource audioSource)
        {
            // Debug.Log("PauseAudio stopped");
        }
    }
}