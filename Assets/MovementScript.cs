using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0) _renderer.flipX = false;
        if (horizontal < 0) _renderer.flipX = true;

        _rigidbody.velocity += new Vector2(horizontal*0.5f, 0);
        if (horizontal == 0) _rigidbody.velocity = new Vector2(_rigidbody.velocity.x / 2, _rigidbody.velocity.y);
        //_rigidbody.MovePosition(_rigidbody.position + new Vector2(horizontal * 0.1f, 0));
        if (Input.GetKeyDown("space") && Math.Abs(_rigidbody.velocity.y) <= 1e-9)
        {
            _rigidbody.velocity += new Vector2(0, 10);
        }

        if (Math.Abs(_rigidbody.velocity.x) > 6)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x > 0 ? 6 : -6, _rigidbody.velocity.y);
    }
}
