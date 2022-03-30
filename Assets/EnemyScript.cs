using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private GameObject Player { get; set; }
    private Transform PlayerTransform { get; set; }

    private SpriteRenderer SpriteRenderer { get; set; }

    //private Rigidbody2D Rigidbody { get; set; }
    //private float Speed { get; set; }

    private float CurrHP { get; set; }
    private float MaxHP { get; set; }

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerTransform = Player.transform;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        MaxHP = 100;
        CurrHP = MaxHP;
        //Rigidbody = GetComponent<Rigidbody2D>();
        //Speed = 6f;
    }

    void FixedUpdate()
    {
        var dx = (PlayerTransform.position - transform.position).x;
        if (dx < 0)
            SpriteRenderer.flipX = false; // due to original flip of test asset. To be inverted.
        if (dx > 0)
            SpriteRenderer.flipX = true; // due to original flip of test asset. To be inverted.
    }

    public void TakeDamage(float dmg)
    {
        CurrHP = Mathf.Max(CurrHP- dmg, 0);
        print($"{gameObject.name} was set to {CurrHP} hp.");
        if (CurrHP==0)
            Destroy(gameObject);
    }
}