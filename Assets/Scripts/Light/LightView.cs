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

    // 光の角度を設定する
    public void SetAngle(float angle)
    {
        _light.transform.rotation = Quaternion.Euler(90 + angle, angle, 0);
    }
}
