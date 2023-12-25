using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

namespace TrashyTreasure
{
    public class MouseLook : MonoBehaviour
    {
        public float damping;
        Plane plane = new Plane(Vector3.down, 0);
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 worldPos = new Vector3();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            plane.distance = gameObject.transform.position.y;
            if(plane.Raycast(ray, out float distance)) {
                worldPos = ray.GetPoint(distance) - gameObject.transform.parent.gameObject.transform.position;
            }

            var rotation = Quaternion.LookRotation(worldPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }
    }
}
