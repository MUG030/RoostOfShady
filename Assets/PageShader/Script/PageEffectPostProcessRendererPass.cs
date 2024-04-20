using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum PageEffectPostprocessTiming
{
    AfterOpaque,
    BeforePostprocess,
    AfterPostprocess
}

namespace PageEffectShader
{
    public class PageEffectPostProcessRenderPass : ScriptableRenderPass
    {
        private const string RenderPassName = nameof(PageEffectPostProcessRenderPass);
        private const string ProfilingSamplerName = "SrcToDest";

        private readonly bool _applyToSceneView;
        private readonly int _mainTexPropertyId = Shader.PropertyToID("_MainTex");
        private readonly Material _material;
        private readonly ProfilingSampler _profilingSampler;
        private readonly int _beforeTexPropertyId = Shader.PropertyToID("_BeforeTex");
        private readonly int _paperTexPropertyId = Shader.PropertyToID("_PaperTex");
        private readonly int _flipPropertyId = Shader.PropertyToID("_Flip");
        private readonly int _reversePropertyIs = Shader.PropertyToID("_Reverse");

        private RenderTargetIdentifier _cameraColorTarget;
        private RenderTargetHandle _tempRenderTargetHandle;
        private PageEffect _volume;

        public PageEffectPostProcessRenderPass(bool applyToSceneView, Shader shader)
        {
            if (shader == null)
            {
                return;
            }

            _applyToSceneView = applyToSceneView;
            _profilingSampler = new ProfilingSampler(ProfilingSamplerName);
            _tempRenderTargetHandle.Init("_TempRT");

            // マテリアルを作成
            _material = CoreUtils.CreateEngineMaterial(shader);
        }

        public void Setup(RenderTargetIdentifier cameraColorTarget, PageEffectPostprocessTiming timing)
        {
            _cameraColorTarget = cameraColorTarget;

            renderPassEvent = GetRenderPassEvent(timing);

            // Volumeコンポーネントを取得
            var volumeStack = VolumeManager.instance.stack;
            _volume = volumeStack.GetComponent<PageEffect>();
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_material == null)
            {
                return;
            }

            if (!_volume.IsActive.GetValue<bool>())
            {
                return;
            }

            // カメラのポストプロセス設定が無効になっていたら何もしない
            if (!renderingData.cameraData.postProcessEnabled)
            {
                return;
            }

            // カメラがシーンビューカメラかつシーンビューに適用しない場合には何もしない
            if (!_applyToSceneView && renderingData.cameraData.cameraType == CameraType.SceneView)
            {
                return;
            }


            var source = renderingData.cameraData.renderer.cameraColorTarget;

            // コマンドバッファを作成
            var cmd = CommandBufferPool.Get(RenderPassName);
            cmd.Clear();

            // Cameraのターゲットと同じDescription（Depthは無し）のRenderTextureを取得する
            var tempTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            tempTargetDescriptor.depthBufferBits = 0;
            cmd.GetTemporaryRT(_tempRenderTargetHandle.id, tempTargetDescriptor);

            using (new ProfilingScope(cmd, _profilingSampler))
            {
                // Volumeからプロパティを反映
                _material.SetFloat(_flipPropertyId, _volume.Flip.value);
                _material.SetTexture(_paperTexPropertyId, _volume.PaperTex.value);
                _material.SetTexture(_beforeTexPropertyId, _volume.BeforeTex.value);
                _material.SetInt(_reversePropertyIs, _volume.Reverse.value ? 1 : 0);

                // 元のテクスチャから一時的なテクスチャにエフェクトを適用しつつ描画
                cmd.Blit(renderingData.cameraData.renderer.cameraColorTarget, _tempRenderTargetHandle.Identifier());
            }

            // 一時的なテクスチャから元のテクスチャに結果を書き戻す
            cmd.Blit(_tempRenderTargetHandle.Identifier(), source, _material);

            // 一時的なRenderTextureを解放する
            cmd.ReleaseTemporaryRT(_tempRenderTargetHandle.id);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        private static RenderPassEvent GetRenderPassEvent(PageEffectPostprocessTiming postprocessTiming)
        {
            switch (postprocessTiming)
            {
                case PageEffectPostprocessTiming.AfterOpaque:
                    return RenderPassEvent.AfterRenderingSkybox;
                case PageEffectPostprocessTiming.BeforePostprocess:
                    return RenderPassEvent.BeforeRenderingPostProcessing;
                case PageEffectPostprocessTiming.AfterPostprocess:
                    return RenderPassEvent.AfterRendering;
                default:
                    throw new ArgumentOutOfRangeException(nameof(postprocessTiming), postprocessTiming, null);
            }
        }
    }
}