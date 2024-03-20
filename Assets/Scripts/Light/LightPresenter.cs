using UnityEngine;
using UniRx;

public class LightPresenter : MonoBehaviour
{
    private LightView _lightView;
    private LightModel _lightModel;

    private void Awake()
    {
        // LightViewのインスタンスを参照
        _lightView = GetComponent<LightView>();

        _lightModel = new LightModel();

        // Modelの値が変更されたときにViewを更新する
        _lightModel.Intensity.Subscribe(_lightView.SetIntensity).AddTo(this);
        _lightModel.Angle.Subscribe(_lightView.SetAngle).AddTo(this);
    }

    // 光の強さを時間経過によって変化させるメソッド
    public void ChangeIntensityByTime(float deltaTime, float changeSpeed)
    {
        _lightModel.Intensity.Value += deltaTime * changeSpeed;
    }

    // 角度を変動させるメソッド
    public void ChangeAngleByTime(float deltaTime, float deltaAngle)
    {
        _lightModel.ChangeAngle(deltaTime * deltaAngle);
    }
}