using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Changes the color of a specific GameObject
    /// </summary>
    public class ShaderChange : ReactionComponent<Renderer>
    {



        [Tooltip("Shader to change")]
        public Shader shader = null;
        
        public override void OnEventRaised(Renderer rend)
        {   
            if(!playOnce || !finished){
                if (Targets.Contains(rend))
                {
                    if(shader != null){
                        rend.material.shader = shader;
                        Debug.Log("Shader added");
                    }
                    else{
                        Debug.Log("Shader is null");
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
