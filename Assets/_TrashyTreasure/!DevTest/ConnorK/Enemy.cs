using System.Collections;
using System.Collections.Generic;
using CMF;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

namespace TrashyTreasure
{
    public class Enemy : MonoBehaviour
    {
        public int enemyHealth = 100;
        public float timeBetweenAttacks = 0.5f;
        public float attackLookDamping = 5f;
        
        private bool alreadyAttacked, playerInAttackRange;

        private NavMeshAgent agent;
        private Transform    player;
        private CharacterStats playerStats;


        //Velocity threshold for landing animation;
        //Animation will only be triggered if downward velocity exceeds this threshold;
        public float landVelocityThreshold = 5f;

        private Animator animator;
        private float smoothingFactor = 40f;
        private Vector3 oldMovementVelocity = Vector3.zero;


        public AudioSource audioSource;

        //Footsteps will be played every time the traveled distance reaches this value (if 'useAnimationBasedFootsteps' is set to 'true');
        public float footstepDistance = 0.2f;
        float currentFootstepDistance = 0f;

        private float currentFootStepValue = 0f;

        //Volume of all audio clips;
        [Range(0f, 1f)]
        public float audioClipVolume = 0.1f;

        //Range of random volume deviation used for footsteps;
        //Footstep audio clips will be played at different volumes for a more "natural sounding" result;
        public float relativeRandomizedVolumeRange = 0.2f;

        //Audio clips;
        public AudioClip[] footStepClips;
        public AudioClip landClip;

        private void Awake()
        {
            playerStats = GameManager.Instance.playerObject.GetComponent<CharacterStats>();
            player = playerStats.playerTransform;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateAnimations();

            if (playerInAttackRange)
            {
                agent.SetDestination(transform.position);
                var rotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * attackLookDamping);
                if (!alreadyAttacked)
                {
                    alreadyAttacked = true;
                    Invoke(nameof(DealDamage), timeBetweenAttacks);
                }
            }
            else
            {
                agent.SetDestination(player.position);
            }
        }

        private void DealDamage()
        {
            if (playerInAttackRange) {
                // Todo, do damage to players in range.
                playerStats.health -= 5;
            }
            alreadyAttacked = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player")) playerInAttackRange = true;
            // Todo, check characters names in list from GameManager in range.
            //other.gameObject.GetComponent<CharacterStats>();
        }

        private void OnTriggerExit(Collider other)
        {
            // Todo, remove characters in list from in range. Only set false when all are out of range.
            if (other.gameObject.CompareTag("Player")) playerInAttackRange = false;
        }

        private void UpdateAnimations()
        {
            //Get controller velocity;
            Vector3 _velocity = GetComponent<NavMeshAgent>().velocity;

            //Split up velocity;
            Vector3 _horizontalVelocity = VectorMath.RemoveDotVector(_velocity, transform.up);
            Vector3 _verticalVelocity = _velocity - _horizontalVelocity;

            FootStepSoundUpdate(_horizontalVelocity.magnitude);

            //Smooth horizontal velocity for fluid animation;
            _horizontalVelocity = Vector3.Lerp(oldMovementVelocity, _horizontalVelocity, smoothingFactor * Time.deltaTime);
            oldMovementVelocity = _horizontalVelocity;

            animator.SetFloat("VerticalSpeed", _verticalVelocity.magnitude * VectorMath.GetDotProduct(_verticalVelocity.normalized, transform.up));
            animator.SetFloat("HorizontalSpeed", _horizontalVelocity.magnitude);

            //Pass values to animator;
            animator.SetBool("IsGrounded", Physics.Raycast(transform.position, Vector3.down, 0.1f));

            LandCheck(_velocity);
        }

        private void FootStepSoundUpdate(float _movementSpeed)
        {
            float _speedThreshold = 0.05f;
            //Get current foot step value from animator;
            float _newFootStepValue = animator.GetFloat("FootStep");

            //Play a foot step audio clip whenever the foot step value changes its sign;
            if((currentFootStepValue <= 0f && _newFootStepValue > 0f) || (currentFootStepValue >= 0f && _newFootStepValue < 0f))
            {
                //Only play footstep sound if mover is grounded and movement speed is above the threshold;
                if(Physics.Raycast(transform.position, Vector3.down, 0.1f) && _movementSpeed > _speedThreshold)
                    PlayFootstepSound(_movementSpeed);
            }
            currentFootStepValue = _newFootStepValue;
        }

        private void LandCheck(Vector3 _v)
        {
            //Only trigger animation if downward velocity exceeds threshold;
            if(VectorMath.GetDotProduct(_v, transform.up) > -landVelocityThreshold)
                return;

            animator.SetTrigger("OnLand");
            //Play land audio clip;
            audioSource.PlayOneShot(landClip, audioClipVolume);
        }

        private void PlayFootstepSound(float _movementSpeed)
        {
            int _footStepClipIndex = Random.Range(0, footStepClips.Length);
            audioSource.PlayOneShot(footStepClips[_footStepClipIndex], audioClipVolume + audioClipVolume * Random.Range(-relativeRandomizedVolumeRange, relativeRandomizedVolumeRange));
        }
    }
}
