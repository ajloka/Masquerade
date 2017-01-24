using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour {

	public int health = 100;
	public int attack = 5;
	public float attackDelay = 0.5f;
	public int magicDropped = 40;

	public GameObject Fire;

	private Slider healthSlider;

	private float timeLastAttack = 0;

	private Patrol patrol;
	private EnemyAttackTrigger attackTrigger;
	private Character player;
	private BoxCollider2D myCollider;

	private SpriteRenderer spriteRenderer;
	private Color originalColor;

	private bool withFire = false;
	private Color fireColor = Color.red;
	private float lerpColorValue = 0;

	private bool frozen = false;
	private int frozenTime = 4;
	private Color iceColor = Color.cyan;

    AudioSource enemyAudio;
    public float m_PitchRange = 0.2f;
    private float m_OriginalPitch;
    private bool dead = false;

	void Awake () {
		patrol = GetComponentInParent<Patrol> ();
		attackTrigger = GetComponentInChildren<EnemyAttackTrigger> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Character> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		originalColor = spriteRenderer.color;
		myCollider = GetComponent<BoxCollider2D> ();

        enemyAudio = GetComponent<AudioSource>();
        m_OriginalPitch = enemyAudio.pitch;

        healthSlider = GetComponentInChildren<Slider> ();
		healthSlider.maxValue = health;
		healthSlider.value = healthSlider.maxValue;
        healthSlider.gameObject.SetActive(false);
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
        healthSlider.gameObject.SetActive(true);
        healthSlider.value = health;
        if (health <= 0)
			die ();
	}

	void attackToPlayer(){
		player.receiveAttack (attack);
	}

	public void receiveFire(int damage){
		health -= damage;
		healthSlider.gameObject.SetActive(true);
		healthSlider.value = health;
		lerpColorValue = 0;
		withFire = true;

		Transform myFire = Instantiate (Fire).transform;
		myFire.SetParent (this.transform);
		myFire.localPosition = new Vector3(0,8,0);
		Destroy (myFire.gameObject, 2);

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
		player.increaseMagicAndHealth (magicDropped);
        enemyAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
        enemyAudio.Play();

        GetComponent<Animator> ().Stop ();
		GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
		myCollider.enabled = false;
		spriteRenderer.color = Color.black;
		dead = true;
	}

	public bool IsDead(){
		return dead;
	}
}
