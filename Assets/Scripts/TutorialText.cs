﻿using UnityEngine;
using System.Collections;

public class TutorialText : MonoBehaviour
{
    public int id;
    public GameObject WASD;
    public GameObject Magia;
    public GameObject Mascara;
	private bool activado = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && activado)
        {
            DesactivarMenuPause();

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            textTrigger();
        }
    }

    private void DesactivarMenuPause()
    {
  
        Time.timeScale = 1;
        WASD.SetActive(false);
        Magia.SetActive(false);
        Mascara.SetActive(false);
        //DestroyImmediate(this.gameObject);
		this.gameObject.SetActive(false);
    }


    void textTrigger() {

		activado = true;
		WASD.GetComponentInParent<Interfaz> ().NoPausar ();
		Time.timeScale = 0;

        switch (id)
        {
            case 0:
                WASD.SetActive(true);
                break;

            case 1:
                Magia.SetActive(true);
                break;

            case 2:
                Mascara.SetActive(true);
                break;

            case 3:

                //Texto sobre la cabeza del personaje. "Oh, ese perro da miedo"

                break;

            case 4:

                //Texto sobre la cabeza del personaje. "¡Uhg! ¡Odio las ratas!"

                break;

            case 5:

                //Texto sobre la cabeza del personaje. "¡MAMI! He tenido mucho miedo, no se dond están los demás"

                break;
        }
            
        

    }
}
