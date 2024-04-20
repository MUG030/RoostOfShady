using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PageEffectShader
{
    public class CubeAnimation : MonoBehaviour
    {
        Vector3 _defaultPos;

        float _diff = 1.0f;

        // Start is called before the first frame update
        void Start()
        {
            _defaultPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(_diff * Time.deltaTime, 0, 0);
            if (Mathf.Abs(transform.position.x - _defaultPos.x) > +5)
            {
                _diff *= -1;
            }
        }
    }
}