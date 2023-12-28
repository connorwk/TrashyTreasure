using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HammerElf.Tools.Utilities;
using UnityEngine.UI;

namespace TrashyTreasure
{
    public class HUDController : Singleton<HUDController>
    {
        public Slider healthSlider;

        protected override void Awake()
        {
            base.Awake();
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
