using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    [AddComponentMenu("Character Network Transform (Unreliable)")]
    public class CharacterNetworkTransformUnreliable : NetworkTransformUnreliable
    {
        [Header("Target Rotation")]
        [Tooltip("The Transform component to sync rotation. May be on on this GameObject, or on a child.")]
        public Transform targetRot;

        // get local/world rotation
        protected override Quaternion GetRotation() =>
            coordinateSpace == CoordinateSpace.Local ? targetRot.localRotation : targetRot.rotation;

        // set local/world rotation
        protected override void SetRotation(Quaternion rotation)
        {
            if (coordinateSpace == CoordinateSpace.Local)
                targetRot.localRotation = rotation;
            else
                targetRot.rotation = rotation;
        }
    }
}