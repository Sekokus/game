using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackScript : MonoBehaviour
{
    private SpriteRenderer Rend { get; set; }
    
    private enum Attacks
    {
        Medium,
        Strong
    }
    private enum Radius
    {
        Short = 2,
        Long= 4
    }

    void Start()
    {
        AddAction("<Mouse>/leftButton", "hold(duration=2)", ()=>print("StartAnim"), ()=> Attack(Attacks.Medium, Radius.Long), () => print("StopAnim"));
        AddAction("<Mouse>/rightButton", "hold(duration=5)", ()=>print("StartAnim"), ()=> Attack(Attacks.Strong, Radius.Short), () => print("StopAnim"));

        Rend = GetComponent<SpriteRenderer>();
    }

    private void AddAction(string path, string interactions, Action start, Action perform, Action end)
    {
        var action = new InputAction();
        
        action.AddBinding(path).WithInteractions(interactions);

        action.started += _ => start();
        action.performed += _ => perform();
        action.canceled += _ => end();

        action.Enable();
    }

    private void Attack(Attacks type, Radius rad)
    {
        //EnemyScript en;
        var en = GameObject
            .FindGameObjectsWithTag("Enemy")
            .Where(go => Rend.flipX
                ? go.transform.position.x >= transform.position.x
                : go.transform.position.x <= transform.position.x)
            .OrderBy(go => (go.transform.position - transform.position).magnitude)
            .FirstOrDefault(go => (go.transform.position - transform.position).magnitude < (int)rad);
        if (!en)
            return;
        var escr = en.GetComponent<EnemyScript>();
        switch (type)
        {
            case Attacks.Medium:
                escr.TakeDamage(10);
                break;
            case Attacks.Strong:
                escr.TakeDamage(20);
                break;
        }
    }
}
