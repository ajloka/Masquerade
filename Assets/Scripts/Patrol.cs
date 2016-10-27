using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour
{

    private Transform waypoint;
    private CircleCollider2D TriggerOut;
    private CircleCollider2D TriggerIn;
    private GameObject enemy;

    private GameObject player;

    bool playerOnReach = false;
    
    public float speed;
    private int index = 1;

    // Use this for initialization
    void Awake()
    {
        //Encuentra un waypoint para patrullar
        waypoint = transform.Find("Point"+index).GetComponent<Transform>();

        //Encuentra el objeto enemigo en si
        enemy = transform.Find("Enemy").gameObject;

        //El jugador
        player = GameObject.FindGameObjectWithTag("Player");


        //Los dos triggers
        TriggerOut = transform.Find("TriggerOut").GetComponent<CircleCollider2D>();
        TriggerIn = transform.Find("TriggerIn").GetComponent<CircleCollider2D>();

        //Pone los triggers donde este el enemigo
        TriggerOut.transform.position = enemy.transform.position;
        TriggerIn.transform.position = enemy.transform.position;


    }

    // Update is called once per frame
    void Update()
    {
        //Mira si ha de perseguir al player o patrullar
        if (playerOnReach)
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
        float step = speed * Time.deltaTime;

        //Si ya ha llegado a su destino, lo cambia
        if (enemy.transform.position == waypoint.position)
        {
            //Hace un flip al llegar
            if (index == 2) { index = 1; Flip(); }
            else { index = 2; Flip(); }
            waypoint = transform.Find("Point" + index).GetComponent<Transform>();

        }

        //Mueve el enemigo y a los triggers con el
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, waypoint.position, step);
        TriggerOut.transform.position = enemy.transform.position;
        TriggerIn.transform.position = enemy.transform.position;

    }

    void Persigue()
    {
        float step = speed * Time.deltaTime;

        //Encuentra la posicion en X del jugador, y la suya en Y
        var playerPos = new Vector2(player.transform.position.x, enemy.transform.position.y);

        //Aplica el movimiento al enemigo y los triggers
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, playerPos, step);
        TriggerOut.transform.position = enemy.transform.position;
        TriggerIn.transform.position = enemy.transform.position;

    }

    private void Flip()
    {

        enemy.transform.localScale = new Vector3(enemy.transform.localScale.x * -1, enemy.transform.localScale.y, enemy.transform.localScale.z);
        
    }

    //Funcion llamada desde los triggers
    public void setPlayerOnReach(bool aux)
    {
        playerOnReach = aux;
    }





}

