using System;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
	private int maxHealth = 100;
	private int health;
	private int maxMagicAmount = 100;
	private int magicAmount;
	private int attack = 1;
	private int fireAttack = 20;

	private bool wait = false;

	private Mask.MaskType myMask = Mask.MaskType.Cartón;

	private Animator myAnimator;
	public RuntimeAnimatorController BoxAnimatorController;
	public RuntimeAnimatorController JapoAnimatorController;
	public RuntimeAnimatorController EscudoAnimatorController;
	public RuntimeAnimatorController EspartanoAnimatorController;

	public GameObject Jabalina;

	private int speed = 0;
	private int maxSpeed = 10;

	private int fallingSpeedToGetHurt = 15;
    
	public LayerMask whatIsGround;	// A mask determining what is ground to the character
	private Transform groundCheck;	// A position marking where to check if the player is grounded.
	private float groundedRadius;	// Radius of the overlap circle to determine if grounded
	private bool grounded;			// Whether or not the player is grounded.
	private bool previousGrounded;

	private Rigidbody2D myRigidbody;
	private bool facingRight = true;	// For determining which way the player is currently facing.

	private float myGravity;
	private bool onStairs = false;
	private int numStairs = 0; //number of stairs being touched at the same time
    private string m_MagicType;
	public Text magicTypeText;

	public Slider healthSlider;
	public Slider magicSlider;

    private Transform weapon;
	private Vector2 weaponSize;
	public LayerMask whatIsEnemy;
	private bool alreadyAttacked;

	public GameObject gameOverScreen;
	private ParticleSystem particulasMagia;

    private void Awake()
    {
		health = maxHealth;
		magicAmount = maxMagicAmount/2;

        groundCheck = transform.Find("GroundCheck");
		weapon = transform.Find("Weapon");
		weaponSize = weapon.GetComponent<BoxCollider2D> ().size;
        myRigidbody = GetComponent<Rigidbody2D>();
		groundedRadius = (GetComponent<BoxCollider2D> ().size.x * transform.lossyScale.x) / 2;//transform.localScale.x;
		myGravity = myRigidbody.gravityScale;

        myAnimator = GetComponent<Animator>();
		particulasMagia = GetComponentInChildren<ParticleSystem> ();

        m_MagicType = "Plant";
    }

    private void Update()
    {
    
        //Lee si se cmabia el tipo de magia
        if (Input.GetKey("left"))
        {
            m_MagicType = "Ice";
			magicTypeText.color = Color.cyan;
        }
        if (Input.GetKey("right"))
        {
            m_MagicType = "Fire";
			magicTypeText.color = new Color (0.8f, 0, 0); //rojo oscuro
        }
        if (Input.GetKey("up"))
        {
            m_MagicType = "Plant";
			magicTypeText.color = Color.green;
        }

		if (magicTypeText)
			magicTypeText.text = "Magic: " + m_MagicType;
    }


    private void FixedUpdate()
    {
		previousGrounded = grounded;

		grounded = checkGrounded ();

		checkFallingHurts ();

		bool attackWithMagic = Input.GetButtonDown ("Fire2");
		if (attackWithMagic && magicAmount > 0)
			AttackWithMagic ();

		if (wait)
			return;
			

		float movement = Input.GetAxisRaw ("Horizontal");
		float stairsUpDown = onStairs ? Input.GetAxisRaw ("Vertical") : 0;

        if (movement != 0 || stairsUpDown != 0)
        {
            myAnimator.SetBool("IsRunning", true);
        }
        else { myAnimator.SetBool("IsRunning", false); }

		Move (movement, stairsUpDown);

		bool attack = Input.GetButtonDown ("Jump");
		if (attack)
			Attack ();
    }


	public void Move(float movement, float stairsUpDown)
    {
        //only control the player if grounded
        if (grounded) {
            if (movement == 0){ 
            	speed = 0;
			}
		    else {
				myAnimator.SetBool ("Defend", false);
		        speed += 1;
		        if (speed > maxSpeed)
		            speed = maxSpeed;
		    }
			myRigidbody.velocity = new Vector2 (movement * speed, onStairs ? stairsUpDown * maxSpeed : myRigidbody.velocity.y);

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

		//defender
		if (myMask == Mask.MaskType.Vikingo) {
			myAnimator.SetTrigger("Attack");
			myAnimator.SetBool ("Defend", true);
			return;
		}

		//lanzarJabalina
		if (myMask == Mask.MaskType.Espartano) {
			myAnimator.SetTrigger("Attack");
			Transform myJabalina = Instantiate (Jabalina).transform;
			if (transform.localScale.x < 0)
				myJabalina.localScale = new Vector3 (myJabalina.localScale.x * -1, myJabalina.localScale.y, myJabalina.localScale.z);
			myJabalina.position = transform.position;
			return;
		}

		Collider2D[] enemies = Physics2D.OverlapBoxAll(weapon.position, weaponSize, 0, whatIsEnemy);

        myAnimator.SetTrigger("Attack");
		startWaiting ();

        foreach (Collider2D enemyCollider in enemies) {
			Enemy enemy = enemyCollider.GetComponent<Enemy> ();
			enemy.receiveAttack (attack);
		}
	}

	public void receiveAttack(int damage){
		
		//defendiendo
		if (myMask == Mask.MaskType.Vikingo && myAnimator.GetBool("Defend")) {
			return;
		}

		health -= damage;
		healthSlider.value = health;
		if (health <= 0) {

			death ();
		}
	}

	void AttackWithMagic(){
		if (myMask != Mask.MaskType.Cartón)
			return;

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
		magicAmount -= 25;
		if (magicAmount < 0)
			magicAmount = 0;
		magicSlider.value = magicAmount;
        myAnimator.SetTrigger("Magic");
		startWaiting (1.2f);
		//GameObject newParticulasMagia = Instantiate(particulasMagia.gameObject);
		//newParticulasMagia.GetComponent<ParticleSystem> ().Play ();
		//Destroy (newParticulasMagia, 4);
		switch (m_MagicType) {
		case ("Fire"):
			particulasMagia.startColor = new Color (0.8f, 0, 0); //rojo oscuro
			break;
		case ("Plant"):
			particulasMagia.startColor = Color.green;
			break;
		case ("Ice"):
			particulasMagia.startColor = Color.cyan;
			break;
		}
		particulasMagia.Play ();
    }

	public void increaseMagic(int amount){
		magicAmount += amount;
		if (magicAmount > maxMagicAmount)
			magicAmount = maxMagicAmount;
		magicSlider.value = magicAmount;

		//el incremento de vida es fijo
		int lifeAmount = 5;
		health += lifeAmount;
		if (health > maxHealth)
			health = maxHealth;
		healthSlider.value = health;
    }

	public bool magicAvailable(){
		return magicAmount > 0;
	}

	private bool checkGrounded(){
		bool grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != this.gameObject)
				grounded = true;
		}
		return grounded;
	}

	private void checkFallingHurts(){
		if (!previousGrounded && grounded && myRigidbody.velocity.y < -15) {
			health -= Mathf.Abs((int)myRigidbody.velocity.y) * 3;
			healthSlider.value = health;
			if (health <= 0)
				death ();
		}

	}

	private void startWaiting(float delay = 0.3f){
		wait = true;
		if (grounded)
			myRigidbody.velocity = new Vector2 ();
		myAnimator.SetBool("IsRunning", false);
		Invoke ("stopWaiting", delay);
	}

	private void stopWaiting(){
		wait = false;
	}

	public Mask.MaskType getMask(){
		return myMask;
	}

	public void setMask(Mask.MaskType maskType)
    {
        //Cambiar sprites y animaciones

		myMask = maskType;

		float relativeHealth = (float) health / (float) maxHealth ;

		switch (maskType) {
		case Mask.MaskType.Cartón:
			attack = 1;
			maxHealth = 100;
			myAnimator.runtimeAnimatorController = BoxAnimatorController;
			myAnimator.speed = 1;
			magicTypeText.enabled = true;
			break;

		case Mask.MaskType.Japo:
			attack = 15;
			maxHealth = 300;
			myAnimator.runtimeAnimatorController = JapoAnimatorController;
			myAnimator.speed = 2;
			magicTypeText.enabled = false;
			break;

		case Mask.MaskType.Vikingo:
			attack = 0;
			maxHealth = 500;
			myAnimator.runtimeAnimatorController = EscudoAnimatorController;
			myAnimator.speed = 2;
			magicTypeText.enabled = false;
			break;
		
		case Mask.MaskType.Espartano:
			attack = 14;
			maxHealth = 100;
			myAnimator.runtimeAnimatorController = EspartanoAnimatorController;
			myAnimator.speed = 2;
			magicTypeText.enabled = false;
			break;
		}
			
		health = (int)(relativeHealth * maxHealth);
		healthSlider.maxValue = maxHealth;
		healthSlider.value = health;
    }

	void death(){
		GetComponent<SpriteRenderer> ().color = Color.red;
		gameOverScreen.SetActive (true);
		Time.timeScale = 0;
		gameOverScreen.GetComponentInParent<Interfaz> ().NoPausar ();
	}

    public void healPlayer()
    {
        health = maxHealth;
        healthSlider.value = health;
    }
    


}

