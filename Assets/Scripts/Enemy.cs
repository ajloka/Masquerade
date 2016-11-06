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

	void Awake () {
		patrol = GetComponentInParent<Patrol> ();
		attackTrigger = GetComponentInChildren<EnemyAttackTrigger> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Character> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		originalColor = spriteRenderer.color;
		myCollider = GetComponent<BoxCollider2D> ();
	}

	void Update () {
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
		if (health <= 0)
			die ();
	}

	public void receiveIce(){
		frozen = true;
		spriteRenderer.color = iceColor;
		myCollider.enabled = false;
		Invoke ("endIce", frozenTime);
	}

	private void endIce(){
		frozen = false;
		spriteRenderer.color = originalColor;
		myCollider.enabled = true;
	}

	private void die(){
		player.increaseMagic (magicDropped);
		this.gameObject.SetActive (false);
	}
}
