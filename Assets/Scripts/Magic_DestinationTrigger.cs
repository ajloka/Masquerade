using UnityEngine;
using System.Collections;

public class Magic_DestinationTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "MagicItem") {
			GetComponentInParent<Magic_Interactuable> ().finishTheMagic ();
			//this.enabled = false;
		}
	}
}
