using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackScript : MonoBehaviour
{
    private Animator _animator;
    private PlayerController _controller;

    [SerializeField] private bool attackEnabled;

    public bool AttackEnabled
    {
        get => attackEnabled;
        set => attackEnabled = value;
    }

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

    private void Start()
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
        if (!attackEnabled)
        {
            return;
        }
        _animator.SetBool("Attack", true);
    }

    private void StopAnim()
    {
        if (!attackEnabled)
        {
            return;
        }
        _animator.SetBool("Attack", false);
    }

    private void Attack(Attacks type, Radius rad)
    {
        if (!attackEnabled)
        {
            return;
        }
        var en = FindObjectsOfType<EnemyScript>()
            .Where(go => _controller.LookDirection == PlayerController.Direction.Left
                ? go.transform.position.x <= transform.position.x
                : go.transform.position.x >= transform.position.x)
            .OrderBy(go => (go.transform.position - transform.position).magnitude)
            .FirstOrDefault(go => Intersect(go.gameObject, rad));
        if (!en)
            return;
        switch (type)
        {
            case Attacks.Medium:
                en.TakeDamage(10);
                break;
            case Attacks.Strong:
                en.TakeDamage(30);
                var dir = _controller.LookDirection == PlayerController.Direction.Right ? 1 : -1;
                en.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 800 * dir);
                break;
        }
    }

    private bool Intersect(GameObject go, Radius rad, AreaType type = AreaType.Circle)
    {
        var delta = go.transform.position - transform.position;
        switch (type)
        {
            case AreaType.Circle:
                return delta.magnitude < (int)rad;
            case AreaType.Ellipse:
                return delta.x + 2 * delta.y < (int)rad;

        }

        return false;
    }
}