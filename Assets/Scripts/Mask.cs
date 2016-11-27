using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mask : MonoBehaviour
{
	public enum MaskType {Cartón, Japo, Espartano, Vikingo}
	public MaskType maskType;

	public Button myMaskButton;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
		{
			other.GetComponent<Character>().setMask(maskType);

			myMaskButton.interactable = true;
			myMaskButton.GetComponent<Image> ().color = Color.white; 

			Destroy (this.gameObject);
        }

    }
}
