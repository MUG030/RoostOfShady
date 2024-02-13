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
        
        Vector2 moveMent = new Vector2(moveHorizontal, 0);
        _rb.velocity = moveMent * _speed;
    }
}
