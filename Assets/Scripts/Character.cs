using System;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
	
	private int maxSpeed = 10;
    
	public LayerMask whatIsGround;	// A mask determining what is ground to the character
	private Transform groundCheck;	// A position marking where to check if the player is grounded.
	private float groundedRadius;	// Radius of the overlap circle to determine if grounded
	private bool grounded;			// Whether or not the player is grounded.

	private Rigidbody2D myRigidbody;
	private bool facingRight = true;	// For determining which way the player is currently facing.

	private float myGravity;
	private bool onStairs = false;
    private string m_MagicType;
	public Text magicTypeText;

    private void Awake()
    {
        groundCheck = transform.Find("GroundCheck");
        myRigidbody = GetComponent<Rigidbody2D>();
		groundedRadius = (GetComponent<BoxCollider2D> ().size.x * transform.lossyScale.x) / 2;//transform.localScale.x;
		myGravity = myRigidbody.gravityScale;

        m_MagicType = "Plant";
    }

    private void Update()
    {

    
        //Lee si se cmabia el tipo de magia
        if (Input.GetKey("left"))
        {
            m_MagicType = "Ice";
        }
        if (Input.GetKey("right"))
        {
            m_MagicType = "Fire";
        }
        if (Input.GetKey("up"))
        {
            m_MagicType = "Plant";
        }

		magicTypeText.text = "Magic: " + m_MagicType;
    }


    private void FixedUpdate()
    {
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                grounded = true;
        }

		float movement = Input.GetAxisRaw ("Horizontal");
		float stairsUpDown = onStairs ? Input.GetAxisRaw ("Vertical") : 0;
		//bool attack = Input.GetAxisRaw ("Jump") == 1
		Move (movement, stairsUpDown);
    }


	public void Move(float movement, float stairsUpDown)
    {
        //only control the player if grounded
		if (grounded) {
			myRigidbody.velocity = new Vector2 (movement * maxSpeed, onStairs ? stairsUpDown * maxSpeed : myRigidbody.velocity.y);

			if (movement > 0 && !facingRight || movement < 0 && facingRight)
				Flip ();
		}
		else if (myRigidbody.velocity.y > 0)
			myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, 0);
    }


    private void Flip()
    {
        facingRight = !facingRight;

		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }


	public void setOnStairs(bool onStairs){
		this.onStairs = onStairs;
		myRigidbody.gravityScale = onStairs ? 0 : myGravity;
		myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, 0);
	}


    //Metodo invocable para ver si se hace algo con la magia
    public string magic()
    {   
            return m_MagicType;  
    }
}

