using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackScript : MonoBehaviour
{
    private Animator _animator;
    private PlayerController _controller;

    private enum Attacks
    {
        Medium,
        Strong
    }

    private enum Radius
    {
        Short = 2,
        Long = 4
    }

    private enum AreaType
    {
        Circle,
        Ellipse
    }

    void Start()
    {
        AddAction("<Mouse>/leftButton", "hold(duration=0.01)", StartAnim,
            () => Attack(Attacks.Medium, Radius.Long), StopAnim);
        AddAction("<Mouse>/rightButton", "hold(duration=2)", () => print("StartAnim"),
            () => Attack(Attacks.Strong, Radius.Short), () => print("StopAnim"));
        _animator = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
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

    private void StartAnim()
    {
        _animator.SetBool("Attack", true);
        print("StartAnim");
    }
    
    private void StopAnim()
    {
        _animator.SetBool("Attack", false);
        print("StopAnim");
    }

    private void Attack(Attacks type, Radius rad)
    {
        //EnemyScript en;
        var en = GameObject
            .FindGameObjectsWithTag("Enemy")
            .Where(go => _controller.LookDirection == PlayerController.Direction.Left
                ? go.transform.position.x <= transform.position.x
                : go.transform.position.x >= transform.position.x)
            .OrderBy(go => (go.transform.position - transform.position).magnitude)
            .FirstOrDefault(go => Intersect(go, rad));
        if (!en)
            return;
        var escr = en.GetComponent<EnemyScript>();
        switch (type)
        {
            case Attacks.Medium:
                escr.TakeDamage(10);
                break;
            case Attacks.Strong:
                escr.TakeDamage(30);
                var dir = Mathf.Abs(transform.eulerAngles.y) < 1 ? -1 : 1;
                en.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 800 * dir);
                break;
        }
    }

    private bool Intersect(GameObject go, Radius rad, AreaType type=AreaType.Circle)
    {
        var delta = go.transform.position - transform.position;
        switch (type)
        {
            case AreaType.Circle:
                return delta.magnitude < (int) rad;
            case AreaType.Ellipse:
                return delta.x + 2 * delta.y < (int) rad;
            
        }

        return false;
    }
}