//using Devhouse.Tools.Utilities;
using HammerElf.Tools.Utilities;
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
        [SyncVar (hook = nameof(UpdateOutputText))]
        [HideInInspector]
        public string outputTextValue = "";
        [SerializeField]
        private TextMeshProUGUI outputText;

        //public delegate void OutputTextValueChangedDelegate(string newText);
        //[SyncEvent]
        //public event OutputTextValueChangedDelegate EventOutputTextValueChanged;

        public override void OnStartClient()
        {
            base.OnStartClient();

            //outputText.text = outputTextValue;

            if (authority && (!SteamManager.Initialized || isServer)) return;

            //outputTextValue += "Client start for: " + SteamFriends.GetPersonaName() + "\n";
            SetOutputText("Client start for: " + SteamFriends.GetPersonaName());
            ConsoleLog.Log("Client start for: " + SteamFriends.GetPersonaName(), true);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            if (!SteamManager.Initialized || !isServer) return;

            outputTextValue += "Server start for: " + SteamFriends.GetPersonaName();
            //LocalSetOutputText("Server start for: " + SteamFriends.GetPersonaName());
            ConsoleLog.Log("Server start for: " + SteamFriends.GetPersonaName(), true);
        }

        //private void Update()
        //{
        //    outputText.text = outputTextValue;
        //}
        /*

        [Server]
        public void LocalSetOutputText(string newText)
        {
            outputTextValue += newText + "\n";
            ConsoleLog.Log("Updated text value locally: " + outputTextValue);
            outputText.text = outputTextValue;
            UpdateOutputText();
        }

        [ClientRpc]
        public void UpdateOutputText()
        {
            ConsoleLog.Log("Update Text");
            outputText.text = outputTextValue;
        }
        */
        [Command(requiresAuthority =false)]
        public void SetOutputText(string newText)
        {
            outputTextValue += newText + "\n";
            ConsoleLog.Log("SetOutputText value: " + outputTextValue);
        }

        private void UpdateOutputText(string oldText, string newText)
        {
            ConsoleLog.Log("Updated text value: " + outputTextValue);
            outputText.text = newText;
        }
    }
}
