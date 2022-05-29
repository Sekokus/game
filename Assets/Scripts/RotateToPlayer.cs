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
        Vector2 direction = (player.Transform.position - transform.position).normalized;
        var rotationSpeedPerFrame = rotationSpeed * Time.deltaTime;
        transform.right =  rotationSpeedPerFrame * direction + (1 - rotationSpeedPerFrame) * (Vector2) transform.right;
    }
    private void Update()
    {
        Rotate();
    }
}
