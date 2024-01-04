using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HammerElf.Tools.Utilities;

namespace TrashyTreasure
{
    public class GameManager : Singleton<GameManager>
    {
        // Todo, make this a LIST of players, based on how many players are in the lobby.
        public GameObject playerObject;


        protected override void Awake()
        {
            base.Awake();
            if (GameObject.Find("MainCharacter") == null) {
                playerObject = Instantiate(playerObject, new Vector3(0,5,0), Quaternion.identity);
                playerObject.name = "MainCharacter";
            } else {
                playerObject = GameObject.Find("MainCharacter");
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
