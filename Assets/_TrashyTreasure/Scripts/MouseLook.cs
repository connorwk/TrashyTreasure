using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

namespace TrashyTreasure
{
    public class MouseLook : MonoBehaviour
    {
        public float gamepadDamping;
        public float mouseDamping;
        Plane plane = new Plane(Vector3.down, 0);
        
        private Vector2 aim;
        private bool isGamepad;

        private CharacterController controller;

        private InputActions inputActions;
        private PlayerInput playerInput;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            inputActions = new InputActions();
            playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void OnDeviceChange (PlayerInput pi)
        {
            isGamepad = pi.currentControlScheme.Equals("Gamepad") ? true : false;
        }

        // Update is called once per frame
        void Update()
        {
            aim = inputActions.CharControls.Aim.ReadValue<Vector2>();
            Debug.Log(aim);
            Vector3 worldPos = new Vector3();

            if (isGamepad) {
                if (Mathf.Abs(aim.x) > 0 || Mathf.Abs(aim.y) > 0) {
                    Vector3 playerDir = Vector3.right * aim.x + Vector3.forward * aim.y;

                    if (playerDir.sqrMagnitude > 0.0f) {
                        Quaternion newRot = Quaternion.LookRotation(playerDir, Vector3.up) * Quaternion.Euler(0, 45, 0);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, Time.deltaTime * gamepadDamping);
                    }
                }
            } else {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                plane.distance = gameObject.transform.position.y;
                if(plane.Raycast(ray, out float distance)) {
                    worldPos = ray.GetPoint(distance) - gameObject.transform.parent.gameObject.transform.position;
                }
                var rotation = Quaternion.LookRotation(worldPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * mouseDamping);
            }
        }
    }
}
