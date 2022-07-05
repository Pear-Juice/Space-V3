using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Custom/CustomEffectComponent")]
public class CustomEffectComponent : VolumeComponent, IPostProcessComponent
{
    public BoolParameter isActive = new BoolParameter(true);
    public NoInterpColorParameter color = new NoInterpColorParameter(Color.cyan);
    public NoInterpFloatParameter thickness = new NoInterpFloatParameter(0, true);
    public NoInterpFloatParameter radius = new NoInterpFloatParameter(0, true);
    public bool IsActive() => isActive.value;
    public bool IsTileCompatible() => true;
}
