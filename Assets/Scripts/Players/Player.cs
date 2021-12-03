using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(Vector2 direction)
    {
        _rb.velocity=direction * 10;
    }
    public Vector2 velocity => _rb.velocity;
    public Vector3 position => transform.position;
}
