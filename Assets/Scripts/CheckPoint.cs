using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

    private Character player;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<Character>();
            player.healPlayer();
            Destroy(this.gameObject);
}

    }
}
