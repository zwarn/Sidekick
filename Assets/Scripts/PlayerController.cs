using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private Animator animator;
    private Input _input;
    private bool _flipped;
    private bool _busy = false;

    private void Awake()
    {
        _input = new Input();
        _input.InGame.Jump.performed += context => Jump();
        _input.InGame.Strike.performed += context => Strike();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        handleMovement();
    }

    private void handleMovement()
    {
        if (_busy)
        {
            return;
        }

        Vector2 moveDirection = _input.InGame.Movement.ReadValue<Vector2>();
        moveDirection *= movementSpeed * Time.deltaTime;
        animator.SetBool("moving", moveDirection.magnitude > 0.01);
        transform.Translate(moveDirection);

        handleFlip(moveDirection);
    }

    private void handleFlip(Vector2 moveDirection)
    {
        if (moveDirection.x != 0)
        {
            if (_flipped != moveDirection.x < 0)
            {
                _flipped = moveDirection.x < 0;
                var transformLocalScale = transform.localScale;
                transformLocalScale.x *= -1;
                transform.localScale = transformLocalScale;
            }
        }
    }

    private void Jump()
    {
        if (!_busy)
        {
            animator.SetTrigger("jump");
            _busy = true;
        }
    }

    private void Strike()
    {
        if (!_busy)
        {
            int number = Random.Range(1, 4);
            animator.SetTrigger("strike" + number);
            // animator.SetTrigger("strike1");
            _busy = true;
        }
    }

    private void Recover()
    {
        _busy = false;
    }

    private void StrikeHappens()
    {
        float radius = 0.5f;
        float distance = 0.5f;
        var colliderPosition = transform.position + new Vector3(_flipped ? -distance : distance, 0, 0);
        var hitColliders = Physics2D.OverlapCircleAll(colliderPosition, radius);
        foreach (var collider in hitColliders)
        {
            var hitableObject = collider.gameObject.GetComponent<HitableObject>();
            if (hitableObject != null)
            {
                hitableObject.Hit();
            }
        }
    }
}