using HammerElf.Tools.Utilities;
using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;

namespace TrashyTreasure
{
    public class CustomNetworkManager : NetworkManager
    {
        //[SyncVar]
        //[HideInInspector]
        //public string outputTextValue = "";
        //[SerializeField]
        //private TextMeshProUGUI outputText;

        ////public delegate void OutputTextValueChangedDelegate(string newText);
        ////[SyncEvent]
        ////public event OutputTextValueChangedDelegate EventOutputTextValueChanged;

        //public override void OnClientConnect()
        //{
        //    base.OnClientConnect();

        //    if (!SteamManager.Initialized) return;
        //    ConsoleLog.Log(SteamFriends.GetPersonaName() + " connected to the server!", true);
        //}

        ////private void Start()
        ////{
        ////    if (!SteamManager.Initialized) return;

        ////    ConsoleLog.Log(SteamFriends.GetPersonaName(), true);
        ////}

        ////private void Update()
        ////{
        ////    outputText.text = outputTextValue;
        ////}

        //[Server]
        //public void SetOutputText(string newText)
        //{
        //    outputTextValue += newText + "\n";
        //    outputText.text = outputTextValue;
        //}
    }
}
