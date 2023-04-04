using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Mimbus.SDK.Rendering
{
    [System.Serializable]
    public class CustomPostProcessPass : ScriptableRenderPass
    {
        // Used to render from camera to post processings
        // back and forth, until we render the final image to
        // the camera
        RenderTargetIdentifier source;
        RenderTargetIdentifier destinationA;
        RenderTargetIdentifier destinationB;
        RenderTargetIdentifier latestDest;

        readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");
        readonly int temporaryRTIdB = Shader.PropertyToID("_TempRTB");

        Shader m_BlitShader;
        Material m_BlitMaterial;

        public CustomPostProcessPass(Shader blitShader)
        {
            // Set the render pass event
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
			if (blitShader == null)
			{
                Debug.LogWarning("No shader assigned, trying to find default");
				blitShader = Shader.Find("Hidden/ColorBlindnessCorrection");
			}

			m_BlitShader = blitShader;
            m_BlitMaterial = new Material(m_BlitShader);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Grab the camera target descriptor. We will use this when creating a temporary render texture.
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;

            var renderer = renderingData.cameraData.renderer;
            source = renderer.cameraColorTarget;

            // Create a temporary render texture using the descriptor from above.
            cmd.GetTemporaryRT(temporaryRTIdA, descriptor, FilterMode.Bilinear);
            destinationA = new RenderTargetIdentifier(temporaryRTIdA);
            cmd.GetTemporaryRT(temporaryRTIdB, descriptor, FilterMode.Bilinear);
            destinationB = new RenderTargetIdentifier(temporaryRTIdB);
        }

        // The actual execution of the pass. This is where custom rendering occurs.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // Skipping post processing rendering inside the scene View
            if (renderingData.cameraData.isSceneViewCamera)
                return;

            // Here you get your materials from your custom class
            // (It's up to you! But here is how I did it)
            var materials = ColorBlindnessMaterials.Instance;
            if (materials == null)
            {
                Debug.LogError("Custom Post Processing Materials instance is null");
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get("Custom Post Processing");
            cmd.Clear();

            // This holds all the current Volumes information
            // which we will need later
            var stack = VolumeManager.instance.stack;

            #region Local Methods

            // Swaps render destinations back and forth, so that
            // we can have multiple passes and similar with only a few textures
            void BlitTo(Material mat, int pass = 0)
            {
                var first = latestDest;
                var last = first == destinationA ? destinationB : destinationA;
                Blit(cmd, first, last, mat, pass);

                latestDest = last;
            }

            #endregion

            // Starts with the camera source
            latestDest = source;

            //---Custom effect here---
            var customEffect = stack.GetComponent<ColorBlindnessCorrection>();
            // Only process if the effect is active
            if (customEffect.IsActive())
            {
                var material = materials.customEffect;
                // P.s. optimize by caching the property ID somewhere else
                material.SetInt(Shader.PropertyToID("_Mode"), (int)customEffect.colorBlindnessType.value);
                material.SetFloat(Shader.PropertyToID("_Intensity"), customEffect.intensity.value);

                //        var sheet = context.propertySheets.Get(Shader.Find("Hidden/ColorBlindnessCorrection"));

                BlitTo(material);
            }

			// Add any other custom effect/component you want, in your preferred order
			// Custom effect 2, 3 , ...


			// DONE! Now that we have processed all our custom effects, applies the final result to camera
			Blit(cmd, latestDest, source);

			context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        //Cleans the temporary RTs when we don't need them anymore
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(temporaryRTIdA);
            cmd.ReleaseTemporaryRT(temporaryRTIdB);
        }
    }
}
