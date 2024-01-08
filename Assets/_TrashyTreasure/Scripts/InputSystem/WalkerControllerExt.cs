using System.Collections;
using System.Collections.Generic;
using Mirror;
using TrashyTreasure;
using Unity.VisualScripting;
using UnityEngine;

namespace CMF
{
    public class WalkerControllerExt : AdvancedWalkerController
    {
        [HideInInspector]
        public bool networkPlayer;

        public CharacterNetworkTransformUnreliable netTransform;

        protected override void Setup()
        {
            
        }

        protected override Vector3 CalculateMovementDirection()
        {
            //If no character input script is attached to this object, return;
            if(characterInput == null)
                return Vector3.zero;

            Vector3 _velocity = Vector3.zero;

            //If no camera transform has been assigned, use the character's transform axes to calculate the movement direction;
            if(networkPlayer)
            {
                // TODO figure out fucking velocity so we can maybe have animation.
                //_velocity = netTransform.velocity;
                //Debug.Log(_velocity);
            }
            else
            {
                _velocity += tr.right * characterInput.GetHorizontalMovementInput();
                _velocity += tr.forward * characterInput.GetVerticalMovementInput();
            }

            //If necessary, clamp movement vector to magnitude of 1f;
            if(_velocity.magnitude > 1f)
                _velocity.Normalize();

            return _velocity;
        }
    }
}
