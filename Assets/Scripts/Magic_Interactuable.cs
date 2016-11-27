﻿using UnityEngine;
using System.Collections;
using System;

public class Magic_Interactuable : MonoBehaviour {

    public enum MagicType { Plant, Ice, Fire};
    public MagicType MagicElement; // magicType == MagicType.Plant;

    //public Transform reciver;
    public GameObject Stair;
    public GameObject Bridge;

	private GameObject player;
	private Character playerScript;

    private string m_Type;
	private string m_MagicButton = "Fire2"; // boton derecho del raton, o tecla "R" del teclado
	private bool touching = false;
    private bool activated = false;
	private bool finished;

	private Transform invokedItem;
	private Vector3 growingDirection;
	private int growingSpeed = 3;

	private Transform origin;
	private Transform destination;

    // Use this for initialization
    void Awake () {

        player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<Character> ();

		origin = transform.Find ("Origin");
		destination = transform.Find ("Destination");
    }
	
	// Update is called once per frame
	void Update () {
		if (touching && !activated && playerScript.getMask() == Mask.MaskType.Cartón) {
			//Lee si el jugador pulsa la tecla de magia
			if (Input.GetButtonDown (m_MagicButton) && playerScript.magicAvailable()) {
				m_Type = playerScript.magic ();
				startTheMagic (m_Type);
				//playerScript.spendMagic ();
			}
		}
		else if (activated && !finished) {
			invokedItem.localScale += growingDirection * growingSpeed * Time.deltaTime;
		}
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject == player)
        {
            touching = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject == player)
        {
            touching = false;
        }
    }

    private void startTheMagic(string magicPlayerType)
    {
        if(magicPlayerType == "Plant" && MagicElement == MagicType.Plant)
        {
            //crear escalera desde el punto desde este objeto hasta el reciver
			//Instantiate(Stair, transform.position, new Quaternion());

			GameObject myStair = Instantiate(Stair);
			myStair.transform.position = origin.position;
			myStair.transform.localScale = new Vector3 (myStair.transform.lossyScale.x, 0, myStair.transform.lossyScale.z);
			/*
			float myStairLength = (destination.position.y - origin.position.y)/3;
			myStair.transform.localScale = new Vector3 (myStair.transform.lossyScale.x, myStairLength, myStair.transform.lossyScale.z);
			*/
			invokedItem = myStair.transform;
			growingDirection = new Vector3 (0, 1, 0);

			playerScript.spendMagic ();
            activated = true;
        }
        else if (magicPlayerType == "Fire" && MagicElement == MagicType.Fire)
        {
            //Destruir objeto
			GetComponent<SpriteRenderer>().color = Color.red;
            Destroy(gameObject, 1);

			playerScript.spendMagic ();
            activated = true;
			finished = true;

        }
        else if (magicPlayerType == "Ice" && MagicElement == MagicType.Ice)
        {
            //crear puente desde el punto desde este objeto hasta el reciver
			//Instantiate(Bridge, transform.position, new Quaternion());
			GameObject myBridge = Instantiate(Bridge);
			myBridge.transform.position = origin.position;
			myBridge.transform.localScale = new Vector3 (0, myBridge.transform.lossyScale.y, myBridge.transform.lossyScale.z);
			/*
			float myBridgeLength = (destination.position.x - origin.position.x)/3;
			myBridge.transform.localScale = new Vector3 (myBridgeLength, myBridge.transform.lossyScale.y, myBridge.transform.lossyScale.z);
			*/
			invokedItem = myBridge.transform;
			growingDirection = new Vector3 (1, 0, 0);

			playerScript.spendMagic ();
            activated = true;

        }

    }

	public void finishTheMagic(){
		finished = true;
	}
}
