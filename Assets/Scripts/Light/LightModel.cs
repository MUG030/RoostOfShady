using UniRx;

public class LightModel
{
   public ReactiveProperty<float> Intensity { get; private set; }

   public LightModel(float initialIntensity)
   {
       Intensity = new ReactiveProperty<float>(initialIntensity);
   }
}
