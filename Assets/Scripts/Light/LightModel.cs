using UniRx;

public class LightModel
{
    public ReactiveProperty<float> Intensity { get; private set; }
    public ReactiveProperty<float> Angle { get; private set; }

    public LightModel()
    {
        Intensity = new ReactiveProperty<float>(1.0f);
        Angle = new ReactiveProperty<float>(0.0f);
    }

    // 角度を変動させるメソッド
    public void ChangeAngle(float deltaAngle)
    {
        Angle.Value += deltaAngle;
    }
}
