using UnityEngine;

namespace iivimat
{
    public class ChangeVolumeAudio : ReactionComponent<AudioSource>
    {

        public float value = 0.0f;
        public bool audioLoop = true;

        public override void OnEventRaised(AudioSource audioSource)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(audioSource))
                {
                    audioSource.volume += value;
                }
                else
                {
                    Debug.Log("Targets doesnt contains " + audioSource);
                }
            }
        }

        public override void OnEventStopped(AudioSource audioSource)
        {
            // Debug.Log("ChangeAudioVolume stopped");
        }
    }
}