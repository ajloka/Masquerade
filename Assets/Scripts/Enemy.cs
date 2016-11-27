using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int health = 100;
	public int attack = 5;
	public float attackDelay = 0.5f;
	public int magicDropped = 40;

	private float timeLastAttack = 0;

	private Patrol patrol;
	private EnemyAttackTrigger attackTrigger;
	private Character player;
	private BoxCollider2D myCollider;

	private SpriteRenderer spriteRenderer;
	public Color originalColor;

	private bool withFire = false;
	private Color fireColor = Color.red;
	private float lerpColorValue = 0;

	private bool frozen = false;
	private int frozenTime = 2;
	private Color iceColor = Color.cyan;

	private bool dead = false;

	void Awake () {
		patrol = GetComponentInParent<Patrol> ();
		attackTrigger = GetComponentInChildren<EnemyAttackTrigger> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Character> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		originalColor = spriteRenderer.color;
		myCollider = GetComponent<BoxCollider2D> ();
	}

	void Update () {
		if (dead) {
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - Time.deltaTime);
			if (spriteRenderer.color.a <= 0)
				this.gameObject.SetActive (false);
			return;
		}



		if (frozen) {
			patrol.setWaiting (true);
			return;
		}


		bool playerInRange = attackTrigger.isPlayerInRange ();
		bool waiting = Time.time < timeLastAttack + attackDelay;

		if (playerInRange && !waiting) {
			attackToPlayer ();
			timeLastAttack = Time.time;
			patrol.setWaiting (true);
		}
		else {
			patrol.setWaiting (waiting);
		}

		if (withFire) {
			lerpColorValue += Time.deltaTime;
			spriteRenderer.color = Color.Lerp (fireColor, originalColor, lerpColorValue);

			if (lerpColorValue >= 1)
				withFire = false;
		}
	}

	public void receiveAttack(int damage){
		health -= damage;
		if (health <= 0)
			die ();
	}

	void attackToPlayer(){
		player.receiveAttack (attack);
	}

	public void receiveFire(int damage){
		health -= damage;
		lerpColorValue = 0;
		withFire = true;
		if (health <= 0) {
			die ();
		}
	}

	public void receiveIce(){
		frozen = true;
		spriteRenderer.color = iceColor;

		GetComponent<Animator> ().StartPlayback ();
		GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
		myCollider.enabled = false;

		Invoke ("endIce", frozenTime);
	}

	private void endIce(){
		frozen = false;
		spriteRenderer.color = originalColor;

		GetComponent<Animator> ().StopPlayback ();
		GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
		myCollider.enabled = true;
	}

	private void die(){
		player.increaseMagic (magicDropped);

		GetComponent<Animator> ().Stop ();
		GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
		myCollider.enabled = false;
		spriteRenderer.color = Color.black;
		dead = true;
		//this.gameObject.SetActive (false);
	}
}
