using UnityEngine;
using UniRx;

public class LightPresenter : MonoBehaviour
{
    [SerializeField] private LightView _lightView;
    private LightModel _lightModel;

    private void Awake()
    {
        // 初期値として1.0を設定
        _lightModel = new LightModel(1.0f);

        // Modelの値が変更されたときにViewを更新する
        _lightModel.Intensity.Subscribe(_lightView.SetIntensity).AddTo(this);
    }

    // 光の強さを変更するメソッド
    public void ChangeIntensity(float newIntensity)
    {
        _lightModel.Intensity.Value = newIntensity;
    }
}
