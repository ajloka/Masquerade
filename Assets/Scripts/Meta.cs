using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour {

	public GameObject cargandoText;
	public int nextSceneIndex = 2;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
			cargandoText.SetActive (true);
			Invoke ("ChangeScene", 0.1f);
        }

    }

	private void ChangeScene(){
		SceneManager.LoadScene (nextSceneIndex);
	}
}
