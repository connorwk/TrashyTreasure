using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TrashyTreasure
{
    public class EnemyManager : MonoBehaviour
    {
        // Prefabs and Manager Defines
        public GameObject enemyPrefab;
        
        // Enemy Manager Info
        public List<GameObject> enemies;
        public int maxEnemies = 20;
        public float enemyDifficultyModifier = 1f;

        void Awake()
        {
            enemies = new List<GameObject>();
            Vector3 pos = GameManager.Instance.playerObject.transform.position;
            foreach(var obj in EnemySpawner.Entities)
            {
                Vector3 inter = pos - obj.transform.position;
                float d = inter.sqrMagnitude;
                Debug.Log(d);
                //if(d < dist)
                //{
                //    targ = obj;
                //    dist = d;
                //}
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void SpawnEnemy()
        {
            // Todo, select random player and spawn nearby
            enemies.Add(Instantiate(enemyPrefab, new Vector3(0,5,0), Quaternion.identity));
        }
    }
}
