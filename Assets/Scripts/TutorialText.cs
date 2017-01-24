using UnityEngine;
using System.Collections;

public class TutorialText : MonoBehaviour
{
    public int id;
    public GameObject WASD;
    public GameObject Magia;
    public GameObject Mascara;
	private bool activado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && activado)
        {
            DesactivarMenuPause();

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            textTrigger();
        }
    }

    private void DesactivarMenuPause()
    {
  
        Time.timeScale = 1;
        WASD.SetActive(false);
        Magia.SetActive(false);
        Mascara.SetActive(false);
		this.gameObject.SetActive(false);
    }


    void textTrigger() {

		activado = true;
		WASD.GetComponentInParent<Interfaz> ().NoPausar ();
		Time.timeScale = 0;

        switch (id)
        {
            case 0:
                WASD.SetActive(true);
                break;

            case 1:
                Magia.SetActive(true);
                break;

            case 2:
                Mascara.SetActive(true);
                break;
        }
    }
}
