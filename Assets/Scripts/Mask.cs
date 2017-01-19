using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mask : MonoBehaviour
{
	public enum MaskType {Cartón, Japo, Espartano, Caballero}
	public MaskType maskType;

	public Button myMaskButton;

	//animation
	private float speed = 0.3f;
	private float distanceMovement = 0.3f;
	private float maxY;
	private float minY;
	private bool goingUp;


	void Start (){
		maxY = transform.position.y + distanceMovement;
		minY = transform.position.y - distanceMovement;
		goingUp = true;
	}

	void Update(){
		float myY = transform.position.y;
		myY += (goingUp ? 1 : -1) * speed * Time.deltaTime;
		transform.position = new Vector3 (transform.position.x, myY, transform.position.z);

		if (goingUp && myY > maxY || !goingUp && myY < minY)
			goingUp = !goingUp;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
		{
			Character player = other.GetComponent<Character> ();
			player.newMaskObtained ();
			player.setMask(maskType);

			myMaskButton.interactable = true;
			myMaskButton.GetComponent<Image> ().color = Color.white; 

			Destroy (this.gameObject);
        }

    }
}
