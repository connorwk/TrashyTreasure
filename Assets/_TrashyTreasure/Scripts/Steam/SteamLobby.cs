using Mirror;
using Steamworks;
using System;
using UnityEngine;
using TMPro;

namespace TrashyTreasure
{
    public class SteamLobby : NetworkBehaviour
    {
        [SerializeField]
        private GameObject buttons;
        [SerializeField]
        private NetworkManager networkManager;

        private SteamTest steamTest;
        private const string HostAddressKey = "HostAddress";
        protected Callback<LobbyCreated_t> lobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> lobbyEntered;

        public ulong currentLobbyId;

        private void Start()
        {
            networkManager = GetComponent<NetworkManager>();
            steamTest = GetComponent<SteamTest>();

            if (!SteamManager.Initialized) { return; }

            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public void HostLobby()
        {
            buttons.SetActive(false);

            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                buttons.SetActive(true);
                return;
            }

            networkManager.StartHost();

            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby),
                                          HostAddressKey, SteamUser.GetSteamID().ToString());

            Debug.Log("Lobby created successfully.");
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
            steamTest.outputTextValue += SteamFriends.GetPersonaName() + " is joining!\n";
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            if (NetworkServer.active) return;

            networkManager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
            steamTest.outputTextValue += SteamFriends.GetPersonaName() + " has joined!\n";
            networkManager.StartClient();
            buttons.SetActive(false);

            Debug.Log("Player joined lobby");
        }
    }
}
