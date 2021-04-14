using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace iivimat
{
    public class LoadNextScene : ReactionString<String>
    {
        public override void OnEventRaised(String target)
        {
            if(!playOnce || !finished)
            {
                try
                {
                    SceneManager.LoadScene(target);
                    setFinished(true);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Could not load scene " + target + " : " + e.Message);
                }
            }
        }

        public override void OnEventStopped(String target)
        {
            // Debug.Log("LoadNextScene stopped");
        }
    }
}