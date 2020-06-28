using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
	animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") / 3);
        moveVelocity = moveInput.normalized * speed;

	animator.SetFloat("Speed", moveVelocity.x);
	if (moveVelocity.y > 0)
	{
	    animator.SetBool("isJumping", true);
	} else {
	    animator.SetBool("isJumping", false);
	}
	
	Vector3 characterScale = transform.localScale;
	if (moveVelocity.x >= 0f)
	{
	    characterScale.x = 8;
	}
	if (moveVelocity.x < 0f)
	{
	    characterScale.x = -8;
	}
	transform.localScale = characterScale;

    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
