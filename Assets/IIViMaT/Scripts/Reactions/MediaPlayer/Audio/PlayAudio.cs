using UnityEngine;

namespace iivimat
{
    public class PlayAudio : ReactionComponent<AudioSource>
    {
        public bool audioLoop = true;

        public override void OnEventRaised(AudioSource audioSource)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(audioSource))
                {
                    if (!audioSource.isPlaying)
                    {
                        Debug.Log("audioSource played");
                        audioSource.loop = audioLoop;
                        audioSource.Play();
                    }
                }
                else
                {
                    Debug.Log("Targets doesnt contains " + audioSource);
                }
            }
        }

        public override void OnEventStopped(AudioSource audioSource)
        {
            // Debug.Log("PlayAudio stopped");
        }
    }
}