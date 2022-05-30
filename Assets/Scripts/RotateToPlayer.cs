using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Player;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    private PlayerCore player;
    [SerializeField] private float rotationSpeed = 5;

    private void Awake()
    {
        Container.Get<PlayerFactory>().WhenPlayerAvailable(playerCore => player = playerCore);
    }

    private void Rotate()
    {
        Vector2 direction = player.Transform.position - transform.position;
        var euler = transform.eulerAngles;
        euler.z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = euler;
    }

    private void Update()
    {
        Rotate();
    }
}