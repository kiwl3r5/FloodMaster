using Script.Manager;
using Script.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Script.Enemy
{
    public class EnemyAi : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;

        [SerializeField] public Transform player;

        [SerializeField] private LayerMask whatIsPlayer;

        //[SerializeField] private Transform HittingPoint;
        
        //[SerializeField] private Transform enemy;

        [SerializeField] private GameObject alertSprite;
        [SerializeField] private GameObject scaredSprite;
        private SpriteRenderer alertSpriteRen;
        private SpriteRenderer scaredSpriteRen;
        
        private Animator _animator;
        private FieldOfView _fieldOfView;

        [Header("##### EnemyType #####")]
        [SerializeField] private bool isDoDamage;
        //[SerializeField] private GameManager gameManager;

        //Patrolling
        [Header("##### Patrolling #####")]
        [SerializeField] private Transform[] waypoints;//
        private int _waypointIndex;//
        [SerializeField] private int startingWaypoint;
        [SerializeField] private Vector3 walkPoint;
        private bool _walkPointSet;
        //[SerializeField] private float walkPointRange;

        //Attacking
        [Header("##### Attacking #####")]
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float timeDelayBfAttacks;
        [SerializeField] private float timeDelayAtk;
        private bool _alreadyAttacked;

        //States
        [FormerlySerializedAs("sightRange")]
        [Header("##### States #####")] 
        [SerializeField] private float awareRange;
        [SerializeField] private float attackRange;
        [SerializeField] private bool playerInAwareRange,canSeePlayer,playerInAttackRange;
        private Vector3 _previousPosition;
        [SerializeField] private float curSpeed;
        //[SerializeField] private float _velocity;
        private float _distance;
        private float _normalizeDistance;
        private float _timeDistance;
        //[SerializeField] private float timeOfDistance = 1;
        private static readonly int velocity = Animator.StringToHash("Velocity");
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");
        [SerializeField] private bool alreadyAlert = false;
        public bool isFlee;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _fieldOfView = GetComponent<FieldOfView>();
            player = GameObject.Find("LookAt").transform;
            agent = GetComponent<NavMeshAgent>();
            timeDelayAtk = timeDelayBfAttacks;
            alertSpriteRen = alertSprite.GetComponent<SpriteRenderer>();
            scaredSpriteRen = scaredSprite.GetComponent<SpriteRenderer>();
            //transform.position = waypoints[startingWaypoint].position;
        }

        private void Start()
        {
            transform.position = waypoints[startingWaypoint].position;
        }

        private void FixedUpdate()
        {
            //Check for sight and attack range
            canSeePlayer = _fieldOfView.canSeePlayer;
            var position1 = transform.position;
            var position = position1;
            if (canSeePlayer || Physics.CheckSphere(position, awareRange, whatIsPlayer)) { playerInAwareRange = true; }
            else { playerInAwareRange = false; }
            playerInAttackRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);
            var positionXZ = new Vector3(position.x,0,position.z);

            //CurrentSpeed
            var positionAf = position1;
            Vector3 curMove = positionXZ - _previousPosition;
            curSpeed = curMove.magnitude / Time.deltaTime;
            curSpeed /= agent.speed;
            _previousPosition = positionAf;
            curSpeed = Mathf.Round(curSpeed * 10f) / 10f;
            if (canSeePlayer)
            {
                
            }

            if (!playerInAwareRange && !playerInAttackRange) Patrolling();
            /*switch (playerInSightRange && !isFlee || PlayerLocomotion.Instance.isDriving)
            {
                case false:
                    AlertSprite(false);
                    alreadyAlert = false;
                    break;
                case true:
                {
                    AlertSprite(true);
                    if (!alreadyAlert)
                    {
                        AudioManager.Instance.Play("Alert");
                    }
                    alreadyAlert = true;
                    break;
                }
            }*/
            if (playerInAwareRange && !playerInAttackRange && !isFlee && !PlayerLocomotion.Instance.isDriving &&!PlayerManager.Instance.isScary) ChasePlayer();
            if (playerInAwareRange && playerInAttackRange && !GameManager.Instance.gameIsLose && !isFlee && !PlayerLocomotion.Instance.isDriving &&!PlayerManager.Instance.isScary) AttackPlayer();
            if (playerInAwareRange && isFlee || PlayerLocomotion.Instance.isDriving || PlayerManager.Instance.isScary) Flee();

                _animator.SetFloat(velocity,curSpeed,0.1f,Time.deltaTime);

        }

        private void Patrolling()
        {
            if (!_walkPointSet) SearchWalkPoint();

            if (_walkPointSet)
                agent.SetDestination(walkPoint);
            /*if (curSpeed<=0.05f)
            {
                _walkPointSet = false;
            }*/
            ScaredSprite(false);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Waypoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                _walkPointSet = false;
        }
        private void SearchWalkPoint()
        {
            _waypointIndex = Random.Range(0, waypoints.Length);
            walkPoint = waypoints[_waypointIndex].position;
            AlertSprite(false);
            _walkPointSet = true;
        }

        private void ChasePlayer()
        {
            agent.SetDestination(player.position);
            ScaredSprite(false);
            AlertSprite(true);
            _walkPointSet = false;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void AttackPlayer()
        {
            //Make sure enemy doesn't move
            curSpeed = 0;
            var position = transform.position;
            agent.SetDestination(position);
            var playerPosition = player.position;
            Vector3 targetPostitionXZ = new Vector3( playerPosition.x, position.y, playerPosition.z ) ;
            
            transform.LookAt(targetPostitionXZ);
            ScaredSprite(false);

            if (!_alreadyAttacked)
            { 
                //Attack code

                _animator.SetBool(IsAttack,true);
                timeDelayAtk -= Time.deltaTime;
                if (timeDelayAtk <= 0)
                {
                    timeDelayAtk = timeDelayBfAttacks;
                    PlayerLocomotion.Instance.isStunt = true;
                    //End of attack code

                    _alreadyAttacked = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }
        }

        private void Flee()
        {
            if (!_walkPointSet) SearchWalkPoint();
            if (_walkPointSet) agent.SetDestination(walkPoint);
            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            Vector3 distanceWalkPointToPlayer = player.position - walkPoint;
            ScaredSprite(true);
            if (distanceWalkPointToPlayer.magnitude < 10f)
                _walkPointSet = false;
        }
        private void ResetAttack()
        {
            _alreadyAttacked = false;
            _animator.SetBool(IsAttack,false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var position = transform.position;
            Gizmos.DrawWireSphere(position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, awareRange);
        }
        
        private void AlertSprite(bool isEnable)
        {
            alertSpriteRen.enabled = isEnable;
            if (!alreadyAlert && isEnable)
            {
                AudioManager.Instance.Play("Alert");
            }
            alreadyAlert = isEnable;
        }
        
        private void ScaredSprite(bool isEnable)
        {
            scaredSpriteRen.enabled = isEnable;
            /*if (!alreadyAlert && isEnable)
            {
                //AudioManager.Instance.Play("Scared");
            }
            alreadyAlert = isEnable;*/
        }
    }
}
