using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Final : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
		{
			SceneManager.LoadScene (0);
		}
	}
}
