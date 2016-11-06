using UnityEngine;
using System.Collections;

public class TriggerScript : MonoBehaviour {

    private bool isIn;
    private CircleCollider2D Trigger;

    // Use this for initialization
    void Awake () {

        //Mira segun el nombre del objeto si es trigger IN o OUT
        if (this.gameObject.name == "TriggerIn") isIn = true;
        else isIn = false;

        Trigger = this.gameObject.GetComponent<CircleCollider2D>();
	
	}

	void OnTriggerEnter2D(Collider2D other)
    {
    
	    //Si es IN
	    if (isIn)
	    {
	        if (other.tag == "Player")
	        {
	            //Pone a ture perseguir al jugador
	            gameObject.GetComponentInParent<Patrol>().setPlayerOnReach(true);
	            
	        }
	    }

   }

    void OnTriggerExit2D(Collider2D other)
    {
        //Si es OUT
        if (!isIn)
        {
            if (other.tag == "Player")
            {
                //Deja de perseguir al jugador
                gameObject.GetComponentInParent<Patrol>().setPlayerOnReach(false);
                
            }
        }

    }





}
