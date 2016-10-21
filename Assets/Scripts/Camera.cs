using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera : MonoBehaviour
    {
        private Transform target;

		private int offsetX = 4;
		private int offsetY = 2;


        private void Start()
        {
			target = GameObject.FindGameObjectWithTag ("Player").transform;
        }
			
        private void Update()
        {
			transform.position = new Vector3 (target.position.x + offsetX, target.position.y + offsetY, transform.position.z);
        }
    }
}
