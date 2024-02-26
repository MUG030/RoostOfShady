using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightView : MonoBehaviour
{
    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    // 光の強さを設定する
    public void SetIntensity(float intensity)
    {
        _light.intensity = intensity;
    }
}
