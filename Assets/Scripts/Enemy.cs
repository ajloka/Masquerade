using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int health = 100;
	public int attack = 5;
	public float attackDelay = 0.5f;

	private float timeLastAttack = 0;

	private Patrol patrol;
	private EnemyAttackTrigger attackTrigger;
	private Character player;

	void Awake () {
		patrol = GetComponentInParent<Patrol> ();
		attackTrigger = GetComponentInChildren<EnemyAttackTrigger> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Character> ();
	}

	void Update () {
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
		
	}

	public void receiveAttack(int damage){
		health -= damage;
		if (health <= 0)
			this.gameObject.SetActive (false);
	}

	void attackToPlayer(){
		player.receiveAttack (attack);
	}
}
