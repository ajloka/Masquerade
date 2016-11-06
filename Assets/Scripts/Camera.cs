using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera : MonoBehaviour
    {
		private Transform player;

		public Transform parallax;
		private float parallaxSpeed = 0.5f; //From 0 to 1

		private int offsetX = 9;
		private int offsetY = 4;
		private float yCameraSpeed = 0.3f;


        private void Start()
        {
			player = GameObject.FindGameObjectWithTag ("Player").transform;
        }
			
        private void Update()
        {
			this.transform.position = new Vector3 (player.position.x + offsetX, offsetY + player.position.y*yCameraSpeed, transform.position.z);

			//background.transform.position = transform.position;

			if (parallax) 
				parallax.position = new Vector3 (this.transform.position.x * parallaxSpeed, parallax.position.y, parallax.position.z);
        }
    }
}
