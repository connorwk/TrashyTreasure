using System.Collections;
using System.Collections.Generic;
using CMF;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using HammerElf.Tools.Utilities;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

namespace TrashyTreasure
{
    public class CharacterStats : NetworkBehaviour
    {
        public int health;
        public Transform playerTransform;

        [Required]
        public AdvancedWalkerController charController;
        [Required]
        public Mover moverScript;
        [Required]
        public Camera charCamera;
        [Required]
        public Rigidbody charRigidbody;
        [Required]
        public PlayerInput charPlayerInput;
        [Required]
        public MouseLook charMouseLook;

        void Awake()
        {
            
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            //charRigidbody.isKinematic = true;
            DisableCharControl();
        }

        public override void OnStartAuthority()
        {
            EnableCharControl();
        }

        public override void OnStopAuthority()
        {
            DisableCharControl();
        }

        private void EnableCharControl()
        {
            charRigidbody.isKinematic = false;
            charController.enabled = true;
            moverScript.enabled = true;
            charCamera.enabled = true;
            charPlayerInput.enabled = true;
            charMouseLook.enabled = true;
            this.enabled = true;
        }

        private void DisableCharControl()
        {
            charRigidbody.isKinematic = true;
            charController.enabled = false;
            moverScript.enabled = false;
            charCamera.enabled = false;
            charPlayerInput.enabled = false;
            charMouseLook.enabled = false;
            this.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            HUDController.Instance.healthSlider.value = health;
        }
    }
}
