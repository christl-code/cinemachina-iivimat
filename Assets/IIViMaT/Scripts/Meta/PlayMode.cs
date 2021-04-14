using System.Collections.Generic;
using UnityEngine;

namespace iivimat
{
    /**
     * PlayMode is used to clear the way when the game / experimentation begins
     * Concretely, for now, it hides proximity feedbacks (and make them visible once game / exp. is quit)
     */
    public class PlayMode : MonoBehaviour
    {
        private List<GameObject> feedbacks = new List<GameObject>();

        void Awake()
        {
            feedbacks.AddRange(GameObject.FindGameObjectsWithTag("Feedback"));
        }

        void OnEnable()
        {
            // actions.ForEach(element => element.EnterPlayMode());
            feedbacks.ForEach(go => go.GetComponent<Renderer>().enabled = false);
        }

        void OnApplicationQuit()
        {
            // actions.ForEach(element => element.EnterEditMode());
            feedbacks.ForEach(go => go.GetComponent<Renderer>().enabled = true);
        }
    }
}