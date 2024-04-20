using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace PageEffectShader
{
    [Serializable]
    [VolumeComponentMenu("Custom/PageEffect")]
    public class PageEffect : VolumeComponent
    {
        public TextureParameter PaperTex = new TextureParameter(null);
        public BoolParameter IsActive = new BoolParameter(false);
        public BoolParameter Reverse = new BoolParameter(false);
        public ClampedFloatParameter Flip = new ClampedFloatParameter(0, 0, 1);
        public VolumeParameter<Texture> BeforeTex = new VolumeParameter<Texture>();

        Camera _beforeCamera;
        Camera _afterCamera;
        RenderTexture _prev;
        bool _switching = false;
        Coroutine _flipCoroutine;


        void SetOverrideState()
        {
            Reverse.overrideState = true;
            BeforeTex.overrideState = true;
            Flip.overrideState = true;
            PaperTex.overrideState = true;
            IsActive.overrideState = true;
        }

        public IEnumerator Set(bool reverse)
        {
            if (_switching && _flipCoroutine != null) CoroutineHandler.StopStaticCoroutine(_flipCoroutine);
            yield return new WaitForEndOfFrame();
            Texture2D screenShot = ScreenCapture.CaptureScreenshotAsTexture();

            Texture2D newScreenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            newScreenShot.SetPixels(screenShot.GetPixels());
            newScreenShot.Apply();
            BeforeTex.value = newScreenShot;

            SetOverrideState();
            IsActive.value = true;
            Flip.value = reverse ? 0.0f : 1.0f;

            Reverse.value = reverse;
        }


        public void FlipPage(float time)
        {
            if (_switching) InterruptFlip();
            _flipCoroutine = CoroutineHandler.StartStaticCoroutine(FlipAnim1(time));
        }

        public void FlipPage(Camera before, Camera after, bool reverse, float time)
        {
            if (before == null)
            {
                throw new ArgumentNullException("before");
            }

            if (after == null)
            {
                throw new ArgumentNullException("after");
            }

            if (_switching) InterruptFlip();
            SetOverrideState();
            IsActive.value = true;

            _beforeCamera = before;
            _afterCamera = after;

            RenderTexture beforeRenderTexture = new RenderTexture(Screen.width, Screen.height, 24);

            _prev = _beforeCamera.targetTexture;
            before.targetTexture = beforeRenderTexture;
            BeforeTex.value = beforeRenderTexture;
            Flip.value = reverse ? 0.0f : 1.0f;

            Reverse.value = reverse;
            SwicthCamera(before, after);

            _flipCoroutine = CoroutineHandler.StartStaticCoroutine(FlipAnim2(time));
        }

        public void ReleaseTex()
        {
            if (BeforeTex.value is RenderTexture)
            {
                ((RenderTexture)BeforeTex.value).Release();
            }

            IsActive.value = false;
        }

        IEnumerator FlipAnim2(float time)
        {
            if (_switching) yield break;
            _switching = true;
            float timer = 0;
            float diff = Reverse.value ? 0.01f / time : -0.01f / time;
            float fliped = Reverse.value ? 0 : 1;
            while (timer < time)
            {
                fliped += diff;
                Flip.value = Mathf.Clamp(fliped, 0, 1);
                timer += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            if (_beforeCamera != null)
                _beforeCamera.targetTexture = _prev;
            BeforeTex.value = null;

            Release();
            _switching = false;
            IsActive.value = false;

        }

        IEnumerator FlipAnim1(float time)
        {
            if (_switching) yield break;
            _switching = true;
            float timer = 0;
            float diff = Reverse.value ? 0.01f / time : -0.01f / time;
            float fliped = Reverse.value ? 0 : 1;
            while (timer < time)
            {
                fliped += diff;
                Flip.value = Mathf.Clamp(fliped, 0, 1);
                timer += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            BeforeTex.value = null;

            _switching = false;
        }

        void SwicthCamera(Camera before, Camera after)
        {
            float beforeDepth = before.depth;
            float afterDepth = after.depth;
            before.depth = Mathf.Min(beforeDepth, afterDepth);
            after.depth = Mathf.Max(beforeDepth, afterDepth);
        }


        void InterruptFlip()
        {
            CoroutineHandler.StopStaticCoroutine(_flipCoroutine);
            if (_beforeCamera != null)
                _beforeCamera.targetTexture = _prev;
            BeforeTex.value = null;
            Release();
            _switching = false;
            IsActive.value = false;

        }
    }
}
