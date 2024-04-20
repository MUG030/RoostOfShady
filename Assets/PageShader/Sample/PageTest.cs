using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PageEffectShader
{
    public class PageTest : MonoBehaviour
    {
        [SerializeField] Volume _volume;
        [SerializeField] Button _button;
        [SerializeField] Camera _camera1;
        [SerializeField] Camera _camera2;
        [SerializeField] float _flipTime = 1.0f;
        [SerializeField] bool _reverse = false;
        PageEffect _pageEffect;
        Camera now;

        void Start()
        {
            //VolumeのプロファイルからPageEffectを取得
            if (!_volume.profile.TryGet<PageEffect>(out _pageEffect))
            {
                _pageEffect = _volume.profile.Add<PageEffect>();
            }

            _button.onClick.AddListener(FlipPage2);
            now = _camera1;
            Camera.SetupCurrent(now);
        }

        void FlipPage2()
        {
            _volume.isGlobal = false;
            //カメラ1からカメラ2の映像に切り替え
            if (now == _camera1)
            {
                //Localのボリュームをカメラ2の位置に移動
                _volume.transform.position = _camera2.transform.position;
                //画面切り替え
                _pageEffect.FlipPage(_camera1, _camera2, _reverse, _flipTime);

                now = _camera2;
            }
            //カメラ2からカメラ1の映像に切り替え
            else
            {
                //Loacalのボリュームをカメラ1の位置に移動
                _volume.transform.position = _camera1.transform.position;
                //画面切り替え
                _pageEffect.FlipPage(_camera2, _camera1, _reverse, _flipTime);
                now = _camera1;
            }
        }

        void FlipPage1()
        {
            _volume.isGlobal = true;
            StartCoroutine(FlipPageCorutine());
        }

        IEnumerator FlipPageCorutine()
        {
            yield return StartCoroutine(_pageEffect.Set(_reverse));
            Camera.main.transform.position = new Vector3(0, 1, 100);
            //切り替え　引数：ページめくりの秒数
            _pageEffect.FlipPage(_flipTime);
            yield return new WaitForSeconds(_flipTime + 1);

            yield return StartCoroutine(_pageEffect.Set(_reverse));
            Camera.main.transform.position = new Vector3(0, 0, 3);
            _pageEffect.FlipPage(_flipTime);
        }
    }
}