using UnityEngine;
using UniRx;

public class LightManager : MonoBehaviour
{
    [SerializeField] private LightPresenter _lightPresenter;
    [SerializeField] private float _changeSpeed = 1.0f; // 光の強さが変化する速度

    private float _currentIntensity = 1.0f; // 現在の光の強さ

    private void Start()
    {
        // 毎フレームの更新を購読
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                // 光の強さを時間に基づいて変化させる
                _currentIntensity += Time.deltaTime * _changeSpeed;
                _lightPresenter.ChangeIntensity(_currentIntensity);
            })
            .AddTo(this);
    }
}
