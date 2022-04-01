using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public Animator animator;
    private bool _attacking;
    public Transform attackPoint;
    private float _attackRange;
    public LayerMask enemyLayers;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _attacking = false;
        _attackRange = 0.5f;
    }

    private void Update()
    {
        float attackTimer = 0;
        float attackCd = .35f;
        
        animator.SetBool("Attack", _attacking);

        if (Input.GetKeyDown("e") && Math.Abs(_rigidbody.velocity.y) <= 1e-9)
        {
            _attacking = true;
            attackTimer = attackCd;
            Attack();
        }
 
        if (_attacking) {
            if (attackTimer > 0) {
                attackTimer -= Time.deltaTime;
            } else {
                _attacking = false;
            }
        }
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, _attackRange, enemyLayers);
        foreach (var enemy in hitEnemies)
        {
            Debug.Log("We hit" + enemy.name);
        }
    }
}
