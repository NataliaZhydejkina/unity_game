using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int lives = 5;
    [SerializeField]
    private float speed = 3.0F;
    [SerializeField]
    private float jumpForce = 15.0F;
    private bool isGrounded = false;

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private CharState State {
        get { return (CharState)animator.GetInteger("State");  }
        set { animator.SetInteger("State", (int) value); }
    }


    private void Awake()
    {
       
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if(isGrounded) State = CharState.Idle;
        if (isGrounded && Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;
        if(isGrounded) State = CharState.Run;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F);

        isGrounded = colliders.Length > 1;
        if(!isGrounded) State = CharState.Jump;
    }

    private void Jump()
    {
        
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
}


public enum CharState
{
    Idle,
    Jump,
    Run
}