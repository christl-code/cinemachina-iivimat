using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// 
    /// </summary>
    public class Transparency : ReactionComponent<Renderer>
    {
        public enum TransparencyModeOptions
        {
            Real,
            Virtual
        }

        [Range(0, 1)] public float transparency = 0.5f;

        public TransparencyModeOptions transparencyMode = TransparencyModeOptions.Real;

        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");

        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");

        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");

        public override void OnEventRaised(Renderer rend)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(rend))
                {
                    switch (transparencyMode)
                    {
                        case TransparencyModeOptions.Real:
                            rend.material.SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.One);
                            rend.material.SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                            rend.material.SetInt(ZWrite, 0);
                            rend.material.DisableKeyword("_ALPHATEST_ON");
                            rend.material.DisableKeyword("_ALPHABLEND_ON");
                            rend.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                            rend.material.renderQueue = 3000;
                            break;
                        case TransparencyModeOptions.Virtual:
                            rend.material.SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                            rend.material.SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                            rend.material.SetInt(ZWrite, 0);
                            rend.material.DisableKeyword("_ALPHATEST_ON");
                            rend.material.EnableKeyword("_ALPHABLEND_ON");
                            rend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                            rend.material.renderQueue = 3000;
                            break;
                        default:
                            break; //throw new ArgumentOutOfRangeException();
                    }
                    rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, transparency);
                    setFinished(true);
                }
            }
        }

        public override void OnEventStopped(Renderer rend)
        {
            // Debug.Log("Transparence stopped");
        }
    }
}