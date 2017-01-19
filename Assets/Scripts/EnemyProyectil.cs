using UnityEngine;
using System.Collections;

public class EnemyProyectil : MonoBehaviour {

	public GameObject proyectil;
	public float delay;

	private Patrol myPatrolScript;
	private Enemy myEnemyScript;

	// Use this for initialization
	void Awake () {
		myPatrolScript = GetComponentInParent<Patrol> ();
		myEnemyScript = GetComponent<Enemy> ();
	}

	void Start(){
		InvokeRepeating ("LazarProyectil", delay, delay);
	}
	
	// Update is called once per frame
	private void LazarProyectil () {
		if (!myPatrolScript.GetPlayerOnReach () || myEnemyScript.IsDead())
			return;
		
		Transform myProyectil = Instantiate (proyectil).transform;
		if (transform.localScale.x < 0)
			myProyectil.localScale = new Vector3 (myProyectil.localScale.x * -1, myProyectil.localScale.y, myProyectil.localScale.z);
		myProyectil.position = transform.position;
	}

}
