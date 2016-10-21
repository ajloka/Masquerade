﻿using UnityEngine;
using System.Collections;
using System;

public class Magic_Interactuable : MonoBehaviour {

    public enum MagicType { Plant, Ice, Fire};
    public MagicType MagicElement; // magicType == MagicType.Plant;

    public Transform reciver;
    GameObject player;
    public GameObject Stair;
    public GameObject Bridge;

    private string m_Type;
    private string m_MagicButton;
    private bool tocando;
    private bool activated;

	private Transform origin;
	private Transform destination;

    // Use this for initialization
    void Awake () {

        player = GameObject.FindGameObjectWithTag("Player");
        m_MagicButton = "Fire2"; // boton derecho del raton
        tocando = false;
        activated = false;

		origin = transform.Find ("Origin");
		destination = transform.Find ("Destination");
    }
	
	// Update is called once per frame
	void Update () {

        if (tocando && !activated)
        {
            //Lee si el jugador pulsa la tecla de magia
            if (Input.GetButtonDown(m_MagicButton))
            {
                m_Type = player.GetComponent<Character>().magic();
                startTheMagic(m_Type);

            }
        }
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject == player)
        {
            tocando = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject == player)
        {
            tocando = false;
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
			float myStairLength = (destination.position.y - origin.position.y)/3;
			myStair.transform.localScale = new Vector3 (myStair.transform.lossyScale.x, myStairLength, myStair.transform.lossyScale.z);

            activated = true;
        }
        else if (magicPlayerType == "Fire" && MagicElement == MagicType.Fire)
        {
            //Destruir objeto
            Destroy(gameObject, 2);
            activated = true;

        }
        else if (magicPlayerType == "Ice" && MagicElement == MagicType.Ice)
        {
            //crear puente desde el punto desde este objeto hasta el reciver
			//Instantiate(Bridge, transform.position, new Quaternion());
			GameObject myBridge = Instantiate(Bridge);
			myBridge.transform.position = origin.position;
			float myBridgeLength = (destination.position.x - origin.position.x)/3;
			myBridge.transform.localScale = new Vector3 (myBridgeLength, myBridge.transform.lossyScale.y, myBridge.transform.lossyScale.z);
            activated = true;

        }

    }
}
