using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaMove2D : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        
        // 入力をカメラの向きに変換
        Vector3 baseDirection = Camera.main.transform.right;
        Vector3 moveDirection = baseDirection * moveHorizontal;

        _rb.velocity = moveDirection * _speed;
    }
}
