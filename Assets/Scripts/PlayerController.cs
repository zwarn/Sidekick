using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private Animator _animator;
    private Input _input;
    private bool _flipped;

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
        Vector2 moveDirection = _input.InGame.Movement.ReadValue<Vector2>();
        moveDirection *= movementSpeed * Time.deltaTime;
        _animator.SetBool("moving", moveDirection.magnitude > 0.01);
        transform.Translate(moveDirection);

        checkFlip(moveDirection);


    }

    private void checkFlip(Vector2 moveDirection)
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
        _animator.SetTrigger("jump");
    }

    private void Strike()
    {
        int number = Random.Range(1, 3);
        _animator.SetTrigger("strike" + number);
        
    }
}
