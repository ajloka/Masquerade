using UnityEngine;
using System.Collections;

public class Mask : MonoBehaviour
{

    private GameObject player;
    private Character playerScript;

    public int maskType; //1 Para Japo, 2 para Vikingo, 3 para espartano

    private int daño;
    private int vida;

    // Use this for initialization
    void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Character>();

        if(maskType == 1)
        {
            daño = 10;
            vida = 100;
        }
        if (maskType == 2)
        {
            //Cambiar Valores
            daño = 10;
            vida = 100;
        }
        if (maskType == 3)
        {
            //Cambiar Valores
            daño = 10;
            vida = 100;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject == player)
        {
            playerScript.setMask(maskType,daño, vida);
        }

    }
}
