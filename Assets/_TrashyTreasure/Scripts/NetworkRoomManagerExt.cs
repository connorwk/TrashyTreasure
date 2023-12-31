using UnityEngine;
using Mirror;
using Steamworks;
using HammerElf.Tools.Utilities;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

namespace TrashyTreasure
{
    [AddComponentMenu("")]
    public class NetworkRoomManagerExt : NetworkRoomManager
    {
        public static new NetworkRoomManagerExt singleton { get; private set; }

        private const string HostAddressKey = "HostAddress";
        protected Callback<LobbyCreated_t> lobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> lobbyEntered;

        private CSteamID lobbyID;
        /// <summary>
        /// Runs on both Server and Client
        /// Networking is NOT initialized when this fires
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            singleton = this;

            if (!SteamManager.Initialized)
            {
                ConsoleLog.LogError("Steam manager not initialized!", true);
                return;
            }

            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public override void OnRoomStartHost()
        {
            base.OnRoomStartHost();
            ConsoleLog.Log("OnRoomStartHost");
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, maxConnections);
        }

        public override void OnRoomStartClient()
        {
            base.OnRoomStartClient();
            ConsoleLog.Log("OnRoomStartClient");
        }

        public override void OnRoomStopHost()
        {
            base.OnRoomStopHost();
            ConsoleLog.Log("OnRoomStopHost");
            if (lobbyID.IsValid()) SteamMatchmaking.LeaveLobby(lobbyID);
            lobbyID.Clear();
        }

        public override void OnRoomStopClient()
        {
            base.OnRoomStopClient();
            ConsoleLog.Log("OnRoomStopClient");
            if (lobbyID.IsValid()) SteamMatchmaking.LeaveLobby(lobbyID);
            lobbyID.Clear();
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                ConsoleLog.LogError("Result not OK...");
                return;
            }
            lobbyID = new CSteamID(callback.m_ulSteamIDLobby);
            SteamMatchmaking.SetLobbyData(lobbyID,
                                          HostAddressKey, SteamUser.GetSteamID().ToString());
            ConsoleLog.Log(SteamFriends.GetPersonaName() + " created a lobby!!!\n");
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
            ConsoleLog.Log(SteamFriends.GetPersonaName() + " is joining!");
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            if (NetworkServer.active)
            {
                ConsoleLog.Log(SteamFriends.GetPersonaName() + " entered the lobby as host!\n");
                return;
            }
            lobbyID = new CSteamID(callback.m_ulSteamIDLobby);
            networkAddress = SteamMatchmaking.GetLobbyData(lobbyID, HostAddressKey);
            ConsoleLog.Log(SteamFriends.GetPersonaName() + " has joined!\n");
            StartClient();
        }

        /// <summary>
        /// This is called on the server when a networked scene finishes loading.
        /// </summary>
        /// <param name="sceneName">Name of the new scene.</param>
        public override void OnRoomServerSceneChanged(string sceneName)
        {
            // spawn the initial batch of Rewards
            if (sceneName == GameplayScene)
                Debug.Log("Gameplay Scene");
                //Spawner.InitialSpawn();
        }

        /// <summary>
        /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
        /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
        /// into the GamePlayer object as it is about to enter the Online scene.
        /// </summary>
        /// <param name="roomPlayer"></param>
        /// <param name="gamePlayer"></param>
        /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            //PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
            //playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
            return true;
        }

        /*
            This code below is to demonstrate how to do a Start button that only appears for the Host player
            showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
            all players are ready, but if a player cancels their ready state there's no callback to set it back to false
            Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
            Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
            is set as DontDestroyOnLoad = true.
        */

        bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
            // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
            if (Utils.IsHeadless())
            {
                base.OnRoomServerPlayersReady();
            }
            else
            {
                showStartButton = true;
            }
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                // set to false to hide it in the game scene
                showStartButton = false;

                ServerChangeScene(GameplayScene);
            }
        }
    }
}
