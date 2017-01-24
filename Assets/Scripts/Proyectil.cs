using UnityEngine;
using System.Collections;

public class Proyectil : MonoBehaviour {

	public bool belongsToPlayer = true;
	public int damage = 14;
	private int speed = 15;
	private int maxDistance = 15;
	private float distanciaRecorrida = 0;

	void Update () {
		int sentido = transform.localScale.x > 0 ? 1 : -1;
		transform.position += new Vector3 (Time.deltaTime * speed * sentido, 0, 0);
		distanciaRecorrida += Time.deltaTime * speed;
		if (distanciaRecorrida > maxDistance)
			Destroy (this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (belongsToPlayer) {
			Enemy enemyScript = other.GetComponent<Enemy> ();
			if (enemyScript != null) {
				enemyScript.receiveAttack (damage);
				Destroy (this.gameObject);
			}
		}
		else {
			Character playerScript = other.GetComponent<Character> ();
			if (playerScript != null) {
				
				if (playerScript.IsUsingShield ()) {
					checkIfEnemyAlreadyInside ();
					belongsToPlayer = true;
					transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
					distanciaRecorrida = 0;
				}
				else {
					playerScript.receiveAttack (damage);
					Destroy (this.gameObject);
				}
			}
		}
	}

	void checkIfEnemyAlreadyInside(){
		Collider2D[] items = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0);
		foreach (Collider2D item in items) {
			Enemy enemyScript = item.GetComponent<Enemy> ();
			if (enemyScript != null) {
				enemyScript.receiveAttack (damage);
				Destroy (this.gameObject);
			}
		}
	}
}
