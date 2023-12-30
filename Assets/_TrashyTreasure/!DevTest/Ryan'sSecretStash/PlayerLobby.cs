using HammerElf.Tools.Utilities;
using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UnityEngine;

namespace TrashyTreasure
{
    public struct PlayerData
    {
        public bool isReady;
        public string playerNameValue;

        public PlayerData(string name, bool ready = false)
        {
            isReady = ready;
            playerNameValue = name;
        }
    }

    public class PlayerLobby : NetworkBehaviour
    {
        private readonly SyncList<PlayerData> playerEntries = new SyncList<PlayerData>();
        [SerializeField]
        private Transform playerDisplay;
        [SerializeField]
        private GameObject playerEntry;

        private void Start()
        {
            ConsoleLog.Log("Start of start");
            playerEntries.Callback += OnPlayerEntriesChanged;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            ConsoleLog.Log("Start of StartAuthority. List length: " + playerEntries.Count);
            //playerEntries.Callback += OnPlayerEntriesChanged;

            // Process initial SyncList payload
            for (int index = 0; index < playerEntries.Count; index++)
            {
                OnPlayerEntriesChanged(SyncList<PlayerData>.Operation.OP_ADD, index, new PlayerData(), playerEntries[index]);
            }
            ConsoleLog.Log("End of StartAuthority. List length: " + playerEntries.Count);
        }

        //public override void OnStartServer()
        //{
        //    playerEntries.Callback += OnPlayerEntriesChanged;

        //    AddEntry(new PlayerData(SteamFriends.GetPersonaName()));
        //}

        public void JoinParty()
        {
            ConsoleLog.Log("Authority: " + authority + " | isServer: " + isServer + " | isClient: " + isClient);

            if(authority && isServer)
            {
                playerEntries.Add(new PlayerData(SteamFriends.GetPersonaName()));
            }
            else
            {
                ConsoleLog.Log("Client attempting to add. List length: " + playerEntries.Count);
                AddEntry(new PlayerData(SteamFriends.GetPersonaName()));
            }
            ConsoleLog.Log("End of JoinParty. List length: " + playerEntries.Count);
        }

        [Command]
        public void AddEntry(PlayerData newEntry)
        {
            ConsoleLog.Log("Start of AddEntry. List length: " + playerEntries.Count);
            playerEntries.Add(newEntry);
        }

        public void OnPlayerEntriesChanged(SyncList<PlayerData>.Operation op, int index, PlayerData oldEntry, PlayerData newEntry)
        {
            ConsoleLog.Log("Start of EntriesChanged. List length: " + playerEntries.Count);
            switch (op)
            {
                case SyncList<PlayerData>.Operation.OP_ADD:
                    ConsoleLog.Log("op is OP_ADD");
                    GameObject entry = Instantiate(playerEntry, playerDisplay);
                    entry.GetComponent<PlayerEntry>().updateDisplay(newEntry.isReady, newEntry.playerNameValue);
                    break;
                default:
                    break;
            }
        }
    }
}
