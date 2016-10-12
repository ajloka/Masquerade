using System;
using UnityEngine;

public class Character : MonoBehaviour
{
	
	private int MaxSpeed = 10;
    
	public LayerMask WhatIsGround;	// A mask determining what is ground to the character
	private Transform GroundCheck;	// A position marking where to check if the player is grounded.
	private float GroundedRadius;	// Radius of the overlap circle to determine if grounded
	private bool Grounded;			// Whether or not the player is grounded.

	private Rigidbody2D Rigidbody2D;
	private bool FacingRight = true;	// For determining which way the player is currently facing.

    private void Awake()
    {
        GroundCheck = transform.Find("GroundCheck");
        Rigidbody2D = GetComponent<Rigidbody2D>();
		GroundedRadius = transform.localScale.x / 2;
    }


    private void FixedUpdate()
    {
        Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                Grounded = true;
        }

		float movement = Input.GetAxisRaw ("Horizontal");
		//bool attack = Input.GetAxisRaw ("Jump") == 1
		Move (movement);
    }


    public void Move(float movement)
    {
        //only control the player if grounded
        if (Grounded)
        {
            Rigidbody2D.velocity = new Vector2(movement*MaxSpeed, Rigidbody2D.velocity.y);

			if (movement > 0 && !FacingRight || movement < 0 && FacingRight)
                Flip();
        }
    }


    private void Flip()
    {
        FacingRight = !FacingRight;

		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}

