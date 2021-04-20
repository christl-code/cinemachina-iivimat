using UnityEngine;

namespace iivimat
{
    public class StopAudio : ReactionComponent<AudioSource>
    {
        public override void OnEventRaised(AudioSource audioSource)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(audioSource))
                {
                    if (audioSource.isPlaying)
                        audioSource.Stop();
                }
            }
        }

        public override void OnEventStopped(AudioSource audioSource)
        {
            // Debug.Log("PauseAudio stopped");
        }
    }
}