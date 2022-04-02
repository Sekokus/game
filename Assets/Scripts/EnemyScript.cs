using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private GameObject Player { get; set; }
    private Transform PlayerTransform { get; set; }

    private SpriteRenderer SpriteRenderer { get; set; }

    private readonly TimedTrigger _painTrigger = new TimedTrigger();

    private float CurrHP { get; set; }
    private float MaxHP { get; set; }

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerTransform = Player.transform;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        MaxHP = 100;
        CurrHP = MaxHP;
    }

    private void FixedUpdate()
    {
        _painTrigger.Step(Time.fixedDeltaTime);
        var dx = (PlayerTransform.position - transform.position).x;
        if (dx < 0)
            SpriteRenderer.flipX = false; // due to original flip of test asset. To be inverted.
        if (dx > 0)
            SpriteRenderer.flipX = true; // due to original flip of test asset. To be inverted.

        SpriteRenderer.color = _painTrigger.IsSet ? Color.red : Color.white;
    }

    public void TakeDamage(float dmg)
    {
        CurrHP = Mathf.Max(CurrHP - dmg, 0);
        //print($"{gameObject.name} was set to {CurrHP} hp.");
        if (CurrHP == 0)
            Destroy(gameObject);
        else
            _painTrigger.SetFor(0.25f);

    }
}