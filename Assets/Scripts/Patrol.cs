using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Patrol : MonoBehaviour
{
	public bool flying = false;

    private Transform waypoint;
    private GameObject enemy;
	private Rigidbody2D enemyRigidbody;
	private RectTransform enemyCanvas;

    private GameObject player;
    AudioSource enemyAudio;
    public float m_PitchRange = 0.2f;
    private float m_OriginalPitch;

    bool playerOnReach = false;
    
    public float speed;
    private int index = 1;

	bool waiting = false;

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

        enemyAudio = GetComponent<AudioSource>();
        m_OriginalPitch = enemyAudio.pitch;
    }

    void FixedUpdate()
    {
		if (waiting) {
			return;
		}
        else if (playerOnReach)
        {
            Pursue();
        }

        else
        {
            Patrulla();
        }

        
    }

    void Patrulla()
    {
        //if reach destinantion, change destination
        if (Vector3.Distance(enemy.transform.position, waypoint.position) < 2)
        {
            //Hace un flip al llegar
            if (index == 2) {
				index = 1;
			}
            else {
				index = 2;
			}
            waypoint = transform.Find("Point" + index).GetComponent<Transform>();
        }

        //Move
		int rightOrLeft = enemy.transform.position.x > waypoint.position.x ? -1 : 1;
		enemyRigidbody.velocity = new Vector2 (rightOrLeft * speed, 0);

		if (flying) {
			int upOrDown = enemy.transform.position.y > waypoint.position.y ? -1 : 1;
			enemyRigidbody.velocity = new Vector2 (enemyRigidbody.velocity.x, upOrDown*speed);
		}
         
		CheckIfFlip (waypoint.transform);

    }

    void Pursue()
    {
        Vector3 playerPos = player.transform.position;

        //Move
		int rightOrLeft = enemy.transform.position.x > playerPos.x ? -1 : 1;
		enemyRigidbody.velocity = new Vector2 (rightOrLeft * speed, 0);

		if (flying) {
			int upOrDown = enemy.transform.position.y > playerPos.y ? -1 : 1;
			enemyRigidbody.velocity = new Vector2 (enemyRigidbody.velocity.x, upOrDown*speed);
		}

        CheckIfFlip(player.transform);
    }

	private void CheckIfFlip(Transform targetToMove)
	{
		if (enemy.transform.lossyScale.x > 0 && targetToMove.position.x - enemy.transform.position.x > 0.1f
			|| enemy.transform.lossyScale.x < 0 && targetToMove.position.x - enemy.transform.position.x < -0.1f) {
			enemy.transform.localScale = new Vector3 (enemy.transform.localScale.x * -1, enemy.transform.localScale.y, enemy.transform.localScale.z);
			enemyCanvas.localScale = new Vector3 (enemyCanvas.localScale.x * -1, enemyCanvas.localScale.y, enemyCanvas.localScale.z); //para que el canvas lo gire
		}
	}

    //Function called from trigers
	public void SetPlayerOnReach(bool playerOnReach)
    {
		this.playerOnReach = playerOnReach;
        if (playerOnReach)
        {
            enemyAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
            enemyAudio.Play();
        }
    }

	public bool GetPlayerOnReach(){
		return playerOnReach;
	}


	public void setWaiting(bool waiting){
		this.waiting = waiting;
	}


}

