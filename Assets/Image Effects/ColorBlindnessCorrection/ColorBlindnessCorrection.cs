using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Mimbus.SDK.Rendering
{

    public enum ColorBlindness
    {
        None,
        Protanopia,
        Deutranopia,
        Tritanopia
    }

    [Serializable]
    public sealed class ColorBlindnessParameter : VolumeParameter<ColorBlindness> { }


    [Serializable, VolumeComponentMenuForRenderPipeline("Mimbus/ColorBlindnessCorrection", typeof(UniversalRenderPipeline))]
    public class ColorBlindnessCorrection : VolumeComponent, IPostProcessComponent
    {

		[Tooltip("Color blindness type.")]
		public ColorBlindnessParameter colorBlindnessType = new ColorBlindnessParameter() { value = ColorBlindness.Protanopia };
		[Range(0f, 1f), Tooltip("Correction intensity.")]
        public ClampedFloatParameter intensity = new ClampedFloatParameter(.5f, 0, 1);

        // Tells when our effect should be rendered
        public bool IsActive() => colorBlindnessType.value != ColorBlindness.None && intensity.value > 0;

        public bool IsTileCompatible() => true;
    }
    //[Serializable]
    //public sealed class ColorBlindnessParameter : ParameterOverride<ColorBlindness> { }

    //[Serializable]
    //[PostProcess(typeof(ColorBlindnessCorrectionRenderer), PostProcessEvent.AfterStack, "Mimbus/ColorBlindnessCorrection")]
    //public sealed class ColorBlindnessCorrection : PostProcessEffectSettings
    //{
    //    [Tooltip("Color blindness type.")]
    //    public ColorBlindnessParameter colorBlindnessType = new ColorBlindnessParameter { value = ColorBlindness.Protanopia };

    //    [Range(0f, 1f), Tooltip("Correction intensity.")]
    //    public FloatParameter intensity = new FloatParameter { value = 0.5f };
    //}

    //public sealed class ColorBlindnessCorrectionRenderer : PostProcessEffectRenderer<ColorBlindnessCorrection>
    //{
    //    public override void Render(PostProcessRenderContext context)
    //    {
    //        var sheet = context.propertySheets.Get(Shader.Find("Hidden/ColorBlindnessCorrection"));
    //        sheet.properties.SetInt("_Mode", (int)settings.colorBlindnessType.value);
    //        sheet.properties.SetFloat("_Intensity", settings.intensity);
    //        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    //    }
    //}
}
