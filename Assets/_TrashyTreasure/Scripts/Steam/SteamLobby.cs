using Mirror;
using Steamworks;
using System;
using UnityEngine;
using TMPro;
using HammerElf.Tools.Utilities;

namespace TrashyTreasure
{
    public class SteamLobby : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttons;
        private NetworkManager networkManager;
        [SerializeField]
        private GameObject playerEntry;
        [SerializeField]
        private Transform playerDisplay;

        [SerializeField]
        private SteamTest steamTest;
        private const string HostAddressKey = "HostAddress";
        protected Callback<LobbyCreated_t> lobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> lobbyEntered;

        private void Start()
        {
            networkManager = GetComponent<NetworkManager>();
            //steamTest = GetComponent<SteamTest>();

            if (!SteamManager.Initialized) { return; }

            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public void HostLobby()
        {
            try
            {
                if (!SteamUser.BLoggedOn())
                {
                    ConsoleLog.LogWarning("Not connected to Steam.");
                    return;
                }
            }
            catch (InvalidOperationException e)
            {
                ConsoleLog.LogError("InvalidOperationException occurred at HostLobby\n" + e.Message);
                throw new Exception("InvalidOperationException occurred at HostLobby\n" + e.Message);
            }
            catch (Exception e)
            {
                ConsoleLog.LogError("Exception (" + e + ") occurred at HostLobby\n" + e.Message);
                throw new Exception("Exception (" + e + ") occurred at HostLobby\n" + e.Message);
            }

            buttons.SetActive(false);

            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);

            AddPlayerEntry();
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

            steamTest.LocalSetOutputText(SteamFriends.GetPersonaName() + " created a lobby!!!");

            ConsoleLog.Log("Lobby created successfully.");
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
            //steamTest.SetOutputText(SteamFriends.GetPersonaName() + " is joining!");
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            if (NetworkServer.active)
            {
                steamTest.LocalSetOutputText(SteamFriends.GetPersonaName() + " entered the lobby as host!");
                ConsoleLog.Log("Network server is active for : " + SteamFriends.GetPersonaName());
                return;
            }

            networkManager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
            steamTest.SetOutputText(SteamFriends.GetPersonaName() + " has joined!");
            networkManager.StartClient();
            NetworkClient.Ready();
            buttons.SetActive(false);

            AddPlayerEntry();

            steamTest.SetOutputText(SteamFriends.GetPersonaName() + " has entered a lobby!!!");
            //steamTest.UpdateOutputText();

            ConsoleLog.Log(SteamFriends.GetPersonaName() + " has entered a lobby");
        }

        private void OnConnectedToServer()
        {
            //steamTest.UpdateOutputText();
            steamTest.SetOutputText(SteamFriends.GetPersonaName() + " has connected to server!!!");

            ConsoleLog.Log(SteamFriends.GetPersonaName() + " has connected to server!!!");
            //steamTest.UpdateOutputText();
        }

        private void AddPlayerEntry()
        {
            GameObject entry = Instantiate(playerEntry, playerDisplay);
            PlayerEntry entryScript = entry.GetComponent<PlayerEntry>();
            entryScript.playerNameValue = SteamFriends.GetPersonaName();
            entryScript.updateDisplay();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
