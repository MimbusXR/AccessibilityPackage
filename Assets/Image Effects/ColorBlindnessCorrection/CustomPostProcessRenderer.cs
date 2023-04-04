using UnityEngine.Rendering.Universal;

namespace Mimbus.SDK.Rendering
{
    [System.Serializable]
    public class CustomPostProcessRenderer : ScriptableRendererFeature
    {
        CustomPostProcessPass pass;
        [UnityEngine.SerializeField]
        UnityEngine.Shader m_Shader;

        public override void Create()
        {
            pass = new CustomPostProcessPass(m_Shader);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(pass);
        }
    }
}
