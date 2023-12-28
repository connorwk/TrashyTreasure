using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Basic;
using UnityEngine;
using UnityEngine.UI;

namespace TrashyTreasure
{
    public class CharacterStats : MonoBehaviour
    {
        public int health;
        public Transform playerTransform;

        void Awake()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            HUDController.Instance.healthSlider.value = health;
        }
    }
}
