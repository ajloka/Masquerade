using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Interfaz : MonoBehaviour {

	public GameObject menuPausa;


	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)){
			activarDesactivarMenuPause ();
		}

	}


	public void ResumeButton(){
		activarDesactivarMenuPause ();
	}

	public void RestartButton(){
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void activarDesactivarMenuPause(){
		//activar
		if (Time.timeScale != 0) {
			Time.timeScale = 0;
			menuPausa.SetActive (true);
		}
		//desactivar
		else {
			Time.timeScale = 1;
			menuPausa.SetActive (false);
		}
	}

}
