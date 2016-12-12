using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Patrol : MonoBehaviour
{

	public bool volador = false;

    private Transform waypoint;
    //private CircleCollider2D TriggerOut;
    //private CircleCollider2D TriggerIn;
    private GameObject enemy;
	private Rigidbody2D enemyRigidbody;
	private RectTransform enemyCanvas;

    private GameObject player;
    

    bool playerOnReach = false;
    
    public float speed;
    private int index = 1;

	bool waiting = false;

    // Use this for initialization
    void Awake()
    {
        //Encuentra un waypoint para patrullar
        waypoint = transform.Find("Point"+index).GetComponent<Transform>();

        //Encuentra el objeto enemigo en si
        enemy = transform.Find("Enemy").gameObject;

        //El jugador
        player = GameObject.FindGameObjectWithTag("Player");

		enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
		enemyCanvas = enemy.GetComponentInChildren<RectTransform> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		
        //Mira si ha de esperar, o si ha de de perseguir al player o patrullar
		if (waiting) {
			return;
		}
        else if (playerOnReach)
        {
            Persigue();
        }

        else
        {
            Patrulla();
        }

        
    }

    void Patrulla()
    {
        //float step = speed * Time.deltaTime;

        //Si ya ha llegado a su destino, lo cambia
		if (Vector3.Distance(enemy.transform.position, waypoint.position) < 2)
        {
            //Hace un flip al llegar
            if (index == 2) {
				index = 1;
				//Flip();
			}
            else {
				index = 2;
				//Flip();
			}
            waypoint = transform.Find("Point" + index).GetComponent<Transform>();

            //myRigidbody.velocity = new Vector2();

        }

        //Mueve el enemigo y a los triggers con el
        //enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, waypoint.position, step);
		int rightOrLeft = enemy.transform.position.x > waypoint.position.x ? -1 : 1;
		enemyRigidbody.velocity = new Vector2 (rightOrLeft * speed, 0);

		if (volador) {
			int upOrDown = enemy.transform.position.y > waypoint.position.y ? -1 : 1;
			enemyRigidbody.velocity = new Vector2 (enemyRigidbody.velocity.x, upOrDown*speed);
		}


         
		CheckIfFlip (waypoint.transform);

    }

    void Persigue()
    {
        //float step = speed * Time.deltaTime;

        //Encuentra la posicion en X del jugador, y la suya en Y
        //var playerPos = new Vector2(player.transform.position.x, enemy.transform.position.y);
		Vector3 playerPos = player.transform.position;

        //Aplica el movimiento al enemigo y los triggers
        //enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, playerPos, step);
		int rightOrLeft = enemy.transform.position.x > playerPos.x ? -1 : 1;
		enemyRigidbody.velocity = new Vector2 (rightOrLeft * speed, 0);

		if (volador) {
			int upOrDown = enemy.transform.position.y > playerPos.y ? -1 : 1;
			enemyRigidbody.velocity = new Vector2 (enemyRigidbody.velocity.x, upOrDown*speed);
		}


        //myRigidbody.velocity = new Vector2();


        CheckIfFlip(player.transform);
    }
	/*
    private void Flip()
    {

        enemy.transform.localScale = new Vector3(enemy.transform.localScale.x * -1, enemy.transform.localScale.y, enemy.transform.localScale.z);
        
    }
    */

	private void CheckIfFlip(Transform targetToMove)
	{
		if (enemy.transform.lossyScale.x > 0 && targetToMove.position.x - enemy.transform.position.x > 0.1f
			|| enemy.transform.lossyScale.x < 0 && targetToMove.position.x - enemy.transform.position.x < -0.1f) {
			enemy.transform.localScale = new Vector3 (enemy.transform.localScale.x * -1, enemy.transform.localScale.y, enemy.transform.localScale.z);
			enemyCanvas.localScale = new Vector3 (enemyCanvas.localScale.x * -1, enemyCanvas.localScale.y, enemyCanvas.localScale.z); //para que el canvas lo gire
		}
	}

    //Funcion llamada desde los triggers
    public void setPlayerOnReach(bool aux)
    {
        playerOnReach = aux;
    }


	public void setWaiting(bool waiting){
		this.waiting = waiting;
	}


}

