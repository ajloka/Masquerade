using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour {

	private int playerCollidersInside = 0; // Number of colliders of the player that are inside our stairs

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			if (playerCollidersInside == 0) other.GetComponent<Character> ().setOnStairs (true);
			playerCollidersInside++;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			playerCollidersInside--;
			if (playerCollidersInside == 0) other.GetComponent<Character> ().setOnStairs (false);
		}
	}
}
