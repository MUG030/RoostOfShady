using UnityEngine;
using UniRx;

public class LightManager : MonoBehaviour
{
    [SerializeField] private LightPresenter _lightPresenter;
    [SerializeField] private float _changeLightIntensitySpeed = 1.0f; // 光の強さが変化する速度
    [SerializeField] private float _changeLightAngleSpeed = 30.0f; // 光の角度が変化する速度

    private void Start()
    {
        // 毎フレームの更新を購読
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                _lightPresenter.ChangeIntensityByTime(Time.deltaTime, _changeLightIntensitySpeed);
                _lightPresenter.ChangeAngleByTime(Time.deltaTime, _changeLightAngleSpeed);
            })
            .AddTo(this);
    }
}
