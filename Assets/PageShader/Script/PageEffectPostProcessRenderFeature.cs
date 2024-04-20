using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PageEffectShader
{
    [Serializable]
    public class PageEffectPostProcessRenderFeature : ScriptableRendererFeature
    {
        [SerializeField] private Shader _shader;
        [SerializeField] private PageEffectPostprocessTiming _timing = PageEffectPostprocessTiming.AfterPostprocess;
        [SerializeField] private bool _applyToSceneView = true;

        private PageEffectPostProcessRenderPass _postProcessPass;

        public override void Create()
        {
            _shader = Shader.Find("Hidden/PaperShader");
            _postProcessPass = new PageEffectPostProcessRenderPass(_applyToSceneView, _shader);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            _postProcessPass.Setup(renderer.cameraColorTarget, _timing);
            renderer.EnqueuePass(_postProcessPass);
        }
    }
}