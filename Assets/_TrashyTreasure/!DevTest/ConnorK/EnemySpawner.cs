using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashyTreasure
{
    public class EnemySpawner : MonoBehaviour
    {
        public static readonly HashSet<EnemySpawner> Entities = new HashSet<EnemySpawner>();

        public int spawnRadius = 5;
        
        private Transform spawnLoc;

        void Awake()
        {
            Entities.Add(this);
        }

        void OnDestroy()
        {
            Entities.Remove(this);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        Transform GetRandSpawnLoc()
        {
            spawnLoc.position = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) * spawnRadius;
            spawnLoc.position = spawnLoc.position + transform.position;
            spawnLoc.rotation = new Quaternion(0, Random.rotation.y, 0, 0);
            return spawnLoc;
        }

        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, spawnRadius + 1); // Add 1 to help visualize that enemies spawn INSIDE the circle.
        }
    }
}
