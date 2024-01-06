using UnityEngine;
using UnityEngine.AI;

namespace TrashyTreasure
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class RandomWander : MonoBehaviour
    {
        private NavMeshAgent agent;

        [Range(0f, 100f), SerializeField]
        private float speed;
        [Range(1f, 500f), SerializeField]
        private float walkRadius;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            if(agent != null )
            {
                speed = agent.speed;
                agent.SetDestination(RandomNavMeshLocation());
            }
        }

        private void Update()
        {
            if( agent != null && agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.SetDestination(RandomNavMeshLocation());
            }
        }

        private Vector3 RandomNavMeshLocation()
        {
            Vector3 finalPosition = Vector3.zero;
            Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
            randomPosition += transform.position;
            if(NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }
    }
}
