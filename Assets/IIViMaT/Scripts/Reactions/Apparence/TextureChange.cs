using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Changes the color of a specific GameObject
    /// </summary>
    public class TextureChange : ReactionComponent<Renderer>
    {



        [Tooltip("Texture to change")]
        public Texture2D texture = null;
        
        public override void OnEventRaised(Renderer rend)
        {   
            if(!playOnce || !finished){
                if (Targets.Contains(rend))
                {
                    if(texture != null){
                        rend.material.mainTexture = texture;
                        Debug.Log("Texture added");
                        setFinished(true);
                    }
                    else{
                        Debug.Log("Texture is null");
                    }
                }
            }
        }

        public override void OnEventStopped(Renderer rend)
        {
            // Debug.Log("ColorChange stopped");
        }
    }
}
