using System.Collections;
using System.Collections.Generic;
using CMF;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using HammerElf.Tools.Utilities;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

namespace TrashyTreasure
{
    public class CharacterStats : NetworkBehaviour
    {
        public int health;
        public Transform playerTransform;

        [Required]
        public CharacterNetworkTransformUnreliable netTransform;
        [Required]
        public WalkerControllerExt charController;
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
        [Required]
        public AudioListener charAudioListener;

        void Awake()
        {
            charController.netTransform = netTransform;
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
            //charController.enabled = true;
            moverScript.enabled = true;
            charCamera.enabled = true;
            charPlayerInput.enabled = true;
            charMouseLook.enabled = true;
            charAudioListener.enabled = true;
            this.enabled = true;
            charController.networkPlayer = false;
        }

        private void DisableCharControl()
        {
            charRigidbody.isKinematic = true;
            //charController.enabled = false;
            moverScript.enabled = false;
            charCamera.enabled = false;
            charPlayerInput.enabled = false;
            charMouseLook.enabled = false;
            charAudioListener.enabled = false;
            this.enabled = false;
            charController.networkPlayer = true;
        }

        // Update is called once per frame
        void Update()
        {
            HUDController.Instance.healthSlider.value = health;
        }
    }
}
