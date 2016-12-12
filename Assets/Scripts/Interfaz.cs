using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Interfaz : MonoBehaviour {

	public GameObject menuPausa;
	private Character playerScript;
	private bool noPausar = false;

	void Awake(){
		playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<Character> ();
	}


	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape) && !noPausar) {
			activarDesactivarMenuPause ();
		}
		else if (noPausar && Time.timeScale != 0) {
			noPausar = false;
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


	public void CartonMask(){
		playerScript.setMask (Mask.MaskType.Cartón);
	}
	public void JapoMask(){
		playerScript.setMask (Mask.MaskType.Japo);
	}
	public void EspartanoMask(){
		playerScript.setMask (Mask.MaskType.Espartano);
	}
	public void VikingoMask(){
		playerScript.setMask (Mask.MaskType.Vikingo);
	}

	public void NoPausar(){
		noPausar = true;
	}

}