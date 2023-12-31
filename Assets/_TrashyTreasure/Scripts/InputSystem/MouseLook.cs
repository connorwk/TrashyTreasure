using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

namespace CMF
{
    public class MouseLook : CharacterInput
    {
        public float gamepadDamping;
        public float mouseDamping;
        Plane plane = new Plane(Vector3.down, 0);
        
        private Vector2 aim;
        private Vector2 move;
        private bool isGamepad;

        private InputActions inputActions;
        private PlayerInput playerInput;
        private InputAction moveAction;
        public Transform modelTransform;

        private void Awake()
        {
            inputActions = new InputActions();
            playerInput = GetComponent<PlayerInput>();
            moveAction = playerInput.actions["Move"];
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
            Vector3 worldPos = new Vector3();
            if (isGamepad) {
                aim = Gamepad.current.rightStick.ReadValue();
                if (Mathf.Abs(aim.x) > 0 || Mathf.Abs(aim.y) > 0) {
                    Vector3 playerDir = Vector3.right * aim.x + Vector3.forward * aim.y;

                    if (playerDir.sqrMagnitude > 0.0f) {
                        Quaternion newRot = Quaternion.LookRotation(playerDir, Vector3.up);
                        modelTransform.rotation = Quaternion.RotateTowards(modelTransform.rotation, newRot, Time.deltaTime * gamepadDamping);
                    }
                }
            } else {
                aim = Mouse.current.delta.ReadValue();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                plane.distance = modelTransform.position.y;
                if(plane.Raycast(ray, out float distance)) {
                    worldPos = ray.GetPoint(distance) - transform.position;
                }
                var rotation = Quaternion.LookRotation(worldPos, Vector3.up);
                modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, rotation, Time.deltaTime * mouseDamping);
            }
        }

        // TODO Move this elsewhere?
        public override float GetHorizontalMovementInput()
        {
            return moveAction.ReadValue<Vector2>().x;
        }

        public override float GetVerticalMovementInput()
        {
            return moveAction.ReadValue<Vector2>().y;
        }

        public override bool IsJumpKeyPressed()
        {
            return false;
        }
    }
}
