using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class CustomPostProcessRenderer : ScriptableRendererFeature
{
    public Shader shader;
    public bool renderInEditor;
    CustomPostProcessPass pass;

    public override void Create()
    {
        pass = new CustomPostProcessPass(shader, renderInEditor);
    }
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}