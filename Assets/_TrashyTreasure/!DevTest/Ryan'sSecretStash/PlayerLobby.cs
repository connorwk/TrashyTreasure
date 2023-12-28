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

        public PlayerData(bool ready, string name)
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

        private void OnStartClient()
        {
            playerEntries.Callback += OnPlayerEntriesChanged;

            // Process initial SyncList payload
            for (int index = 0; index < playerEntries.Count; index++)
                OnPlayerEntriesChanged(SyncList<PlayerData>.Operation.OP_ADD, index, new PlayerData(), playerEntries[index]);
        }

        [Command]
        public void AddEntry(PlayerData newEntry)
        {
            playerEntries.Add(newEntry);
        }

        public void OnPlayerEntriesChanged(SyncList<PlayerData>.Operation op, int index, PlayerData oldEntry, PlayerData newEntry)
        {
            switch(op)
            {
                case SyncList<PlayerData>.Operation.OP_ADD:
                    GameObject entry = Instantiate(playerEntry, playerDisplay);
                    entry.GetComponent<PlayerEntry>().updateDisplay(newEntry.isReady, newEntry.playerNameValue);
                    break;
                default:
                    break;
            }
        }
    }
}
