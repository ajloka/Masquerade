using UnityEngine;
using System.Collections;

public class Proyectil : MonoBehaviour {

	public bool delPlayer = true;
	public int damage = 14;
	private int speed = 10;
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
		if (delPlayer) {
			Enemy enemyScript = other.GetComponent<Enemy> ();
			if (enemyScript != null) {
				enemyScript.receiveAttack (damage);
				Destroy (this.gameObject);
			}
		}
		else {
			Character playerScript = other.GetComponent<Character> ();
			if (playerScript != null) {
				playerScript.receiveAttack (damage);
				Destroy (this.gameObject);
			}
		}
	}
}
