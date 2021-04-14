using System;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Executes reactions when one of the audio clip in inputs end
    /// </summary>
    [Serializable]
    public class OnAudioEnd : Action
    {
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

        /// <summary>
        /// Get all AudioSource in the GameObject in input
        /// </summary>
        private void OnEnable()
        {
            AudioSources.Clear();
            foreach (GameObject go in Objects)
            {
                AudioSource[] components  = go.GetComponents<AudioSource>();
                AudioSources.AddRange(components);
            }
        }

        /// <summary>
        /// Add the AudioSource of the game object, if it contains an AudioSource, to the EventEndHandler 
        /// </summary>
        /// <param name="go"> GameObject with an AudioSource</param>
        public override void OnAddGameObject(GameObject go)
        {
            AudioSource[] components  = go.GetComponents<AudioSource>();
            foreach (AudioSource s in components)
            {
                InteractionsUtility.GetEventEndHandler().AddAudioSource(s);
            }
        }

        /// <summary>
        /// Remove the AudioSource of the game object, if it contains an AudioSource, to the EventEndHandler 
        /// </summary>
        /// <param name="go">GameObject with an AudioSource</param>
        public override void OnRemoveGameObject(GameObject go)
        {
            AudioSource[] components  = go.GetComponents<AudioSource>();
            foreach (AudioSource s in components)
            {
                InteractionsUtility.GetEventEndHandler().RemoveAudioSource(s);
            }
        }
    }
}