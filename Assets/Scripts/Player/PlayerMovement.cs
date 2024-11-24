using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    Rigidbody2D rb2D;
    float _movement = 0.0f;
    [SerializeField]float _speed = 10f;

    private void Start()
    {
        rb2D = GetComponentInChildren<Rigidbody2D>();
    }
    private void Update()
    {
        _movement = Input.GetAxisRaw("Vertical");
        if (_movement > 0 || _movement < 0)
        {
            rb2D.velocity = new Vector2(0, _movement *_speed);
        }
    }
}
