using System;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
	private int maxHealth = 100;
	private int health;
	private int maxMagicAmount = 100;
	private int magicAmount;
	private int attack = 10;
	private int fireAttack = 20;

	private int speed = 0;
	private int maxSpeed = 10;
    
	public LayerMask whatIsGround;	// A mask determining what is ground to the character
	private Transform groundCheck;	// A position marking where to check if the player is grounded.
	private float groundedRadius;	// Radius of the overlap circle to determine if grounded
	private bool grounded;			// Whether or not the player is grounded.

	private Rigidbody2D myRigidbody;
	private bool facingRight = true;	// For determining which way the player is currently facing.

	private float myGravity;
	private bool onStairs = false;
	private int numStairs = 0; //number of stairs being touched at the same time
    private string m_MagicType;
	public Text magicTypeText;

	public Slider healthSlider;
	public Slider magicSlider;

    Animator anim;

    private Transform weapon;
	private Vector2 weaponSize;
	public LayerMask whatIsEnemy;
	private bool alreadyAttacked;

    private void Awake()
    {
		health = maxHealth;
		magicAmount = maxMagicAmount;

        groundCheck = transform.Find("GroundCheck");
		weapon = transform.Find("Weapon");
		weaponSize = weapon.GetComponent<BoxCollider2D> ().size;
        myRigidbody = GetComponent<Rigidbody2D>();
		groundedRadius = (GetComponent<BoxCollider2D> ().size.x * transform.lossyScale.x) / 2;//transform.localScale.x;
		myGravity = myRigidbody.gravityScale;

        anim = GetComponent<Animator>();


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

		if (magicTypeText)
			magicTypeText.text = "Magic: " + m_MagicType;
    }


    private void FixedUpdate()
    {
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != this.gameObject)
                grounded = true;
        }

		float movement = Input.GetAxisRaw ("Horizontal");
		float stairsUpDown = onStairs ? Input.GetAxisRaw ("Vertical") : 0;

        if (movement != 0 || stairsUpDown != 0)
        {
            anim.SetBool("IsRunning", true);
        }
        else { anim.SetBool("IsRunning", false); }

		Move (movement, stairsUpDown);

		bool attack = Input.GetButtonDown ("Jump");
		if (attack)
			Attack ();

		bool attackWithMagic = Input.GetButtonDown ("Fire2");
		if (attackWithMagic && magicAmount > 0)
			AttackWithMagic ();
    }


	public void Move(float movement, float stairsUpDown)
    {
        //only control the player if grounded
        if (grounded) {
            if (movement == 0) { 
            speed = 0;}
        else {
            speed += 1;
            if (speed > maxSpeed)
                speed = maxSpeed;
        }
			myRigidbody.velocity = new Vector2 (movement * speed, onStairs ? stairsUpDown * maxSpeed : myRigidbody.velocity.y);

			if (movement > 0 && !facingRight || movement < 0 && facingRight)
				Flip ();
		} else if (myRigidbody.velocity.y > 0)
			myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, 0);
    }


    private void Flip()
    {
        facingRight = !facingRight;

		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }


	public void setOnStairs(bool onStairs){
		//this.onStairs = onStairs;
		if (onStairs)
			this.numStairs++;
		else
			this.numStairs--;

		this.onStairs = numStairs > 0;

		myRigidbody.gravityScale = this.onStairs ? 0 : myGravity;
		myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, 0);
	}


    //Metodo invocable para ver si se hace algo con la magia
    public string magic()
    {   
            return m_MagicType;  
    }


	void Attack(){

		Collider2D[] enemies = Physics2D.OverlapBoxAll(weapon.position, weaponSize, 0, whatIsEnemy);

        anim.SetTrigger("Attak");

        foreach (Collider2D enemyCollider in enemies) {
			Enemy enemy = enemyCollider.GetComponent<Enemy> ();
			enemy.receiveAttack (attack);
		}
	}

	public void receiveAttack(int damage){
		health -= damage;
		healthSlider.value = health;
		if (health <= 0)
			GetComponent<SpriteRenderer> ().color = Color.red;
	}

	void AttackWithMagic(){
		if (m_MagicType == "Plant")
			return;

		Collider2D[] enemies = Physics2D.OverlapBoxAll(weapon.position, weaponSize, 0, whatIsEnemy);
		foreach (Collider2D enemyCollider in enemies) {
			Enemy enemy = enemyCollider.GetComponent<Enemy> ();

			if (m_MagicType == "Fire")
				enemy.receiveFire (fireAttack);
			else if (m_MagicType == "Ice")
				enemy.receiveIce ();
		}
			
		if (enemies.Length > 0) {
			spendMagic ();
		}
	}

	public void spendMagic(){
		magicAmount -= 20;
		if (magicAmount < 0)
			magicAmount = 0;
		magicSlider.value = magicAmount;
        anim.SetTrigger("Magic");

    }

	public void increaseMagic(int amount){
		magicAmount += amount;
		if (magicAmount > maxMagicAmount)
			magicAmount = maxMagicAmount;
		magicSlider.value = magicAmount;
       
    }

	public bool magicAvailable(){
		return magicAmount > 0;
	}
}

