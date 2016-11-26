using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera : MonoBehaviour
    {
		private Transform player;

		public Transform parallax1;
		public float parallax1Speed = 0.5f; //From 0 to 1

		public Transform parallax2;
		public float parallax2Speed = 0.4f; //From 0 to 1

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

			if (parallax1) 
				parallax1.position = new Vector3 (this.transform.position.x * parallax1Speed, parallax1.position.y, parallax1.position.z);
			if (parallax2) 
				parallax2.position = new Vector3 (this.transform.position.x * parallax2Speed, parallax2.position.y, parallax2.position.z);
        }
    }
}
