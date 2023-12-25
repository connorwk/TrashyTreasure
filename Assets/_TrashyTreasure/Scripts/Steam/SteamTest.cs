//using Devhouse.Tools.Utilities;
using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TrashyTreasure
{
    public class SteamTest : NetworkBehaviour
    {
        [SyncVar]
        [HideInInspector]
        public string outputTextValue;
        [SerializeField]
        private TextMeshProUGUI outputText;

        //public delegate void OutputTextValueChangedDelegate(string newText);
        //[SyncEvent]
        //public event OutputTextValueChangedDelegate EventOutputTextValueChanged;

        private void Start()
        {
            if (!SteamManager.Initialized) return;

            Debug.Log(SteamFriends.GetPersonaName());
        }

        private void Update()
        {
            outputText.text = outputTextValue;
        }

        [Server]
        public void SetOutputText(string newText)
        {
            outputTextValue = newText;
        }
    }
}
