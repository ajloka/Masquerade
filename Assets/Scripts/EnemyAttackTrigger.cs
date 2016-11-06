using UnityEngine;
using System.Collections;

public class EnemyAttackTrigger : MonoBehaviour {

	bool playerInRange = false;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			playerInRange = true;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
			playerInRange = false;
	}

	public bool isPlayerInRange(){
		return playerInRange;
	}
}
