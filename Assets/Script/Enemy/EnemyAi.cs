using Script.Manager;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Script.Enemy
{
    public class EnemyAi : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;

        [SerializeField] private Transform player;

        [SerializeField] private LayerMask whatIsPlayer;

        [SerializeField] private Transform shootPoint;
        
        [SerializeField] private Transform enemy;

        [SerializeField] private GameObject alertSprite;
        private SpriteRenderer alertSpriteRen;
        
        private Animator _animator;
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
        public GameObject projectile;

        //States
        [Header("##### States #####")] 
        [SerializeField] private float sightRange;
        [SerializeField] private float attackRange;
        [SerializeField] private bool playerInSightRange, playerInAttackRange;
        private Vector3 _previousPosition;
        [SerializeField] private float curSpeed;
        //[SerializeField] private float _velocity;
        private float _distance;
        private float _normalizeDistance;
        private float _timeDistance;
        [SerializeField] private float timeOfDistance = 1;
        private static readonly int velocity = Animator.StringToHash("Velocity");
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");
        [SerializeField] private bool alreadyAlert = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            player = GameObject.Find("LookAt").transform;
            agent = GetComponent<NavMeshAgent>();
            timeDelayAtk = timeDelayBfAttacks;
            alertSpriteRen = alertSprite.GetComponent<SpriteRenderer>();
            //transform.position = waypoints[startingWaypoint].position;
        }

        private void Start()
        {
            transform.position = waypoints[startingWaypoint].position;
        }

        private void FixedUpdate()
        {
            //Check for sight and attack range
            var position1 = transform.position;
            var position = position1;
            playerInSightRange = Physics.CheckSphere(position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);
            var positionXZ = new Vector3(position.x,0,position.z);

            //CurrentSpeed
            var positionAf = position1;
            Vector3 curMove = positionXZ - _previousPosition;
            curSpeed = curMove.magnitude / Time.deltaTime;
            curSpeed /= agent.speed;
            _previousPosition = positionAf;
            curSpeed = Mathf.Round(curSpeed * 10f) / 10f;

            if (!playerInSightRange && !playerInAttackRange) Patrolling();
            switch (playerInSightRange)
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
            }
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange && !GameManager.Instance.gameIsLose) AttackPlayer();

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

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Waypoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                _walkPointSet = false;
        }
        private void SearchWalkPoint()
        {
            _waypointIndex = Random.Range(0, waypoints.Length);
            walkPoint = waypoints[_waypointIndex].position;
            _walkPointSet = true;
        }

        private void ChasePlayer()
        {
            agent.SetDestination(player.position);
            _walkPointSet = false;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void AttackPlayer()
        {
            //Make sure enemy doesn't move
            curSpeed = 0;
            var position = transform.position;
            agent.SetDestination(position);
            _distance = Vector3.Distance (enemy.transform.position, player.transform.position);
            _normalizeDistance = _distance / attackRange;
            _timeDistance = _normalizeDistance * timeOfDistance;

            var playerPosition = player.position;
            Vector3 targetPostition = new Vector3( playerPosition.x, playerPosition.y, playerPosition.z ) ;
            Vector3 targetPostitionXZ = new Vector3( playerPosition.x, position.y, playerPosition.z ) ;
            Vector3 vo = CalculateVelocity(targetPostition, shootPoint.position, _timeDistance);
            
            //shootPoint.rotation = Quaternion.LookRotation(Vo);
            shootPoint.LookAt(vo);
            transform.LookAt(targetPostitionXZ);


            if (!_alreadyAttacked)
            { 
                //Attack code

                _animator.SetBool(IsAttack,true);
                timeDelayAtk -= Time.deltaTime;
                if (timeDelayAtk <= 0)
                {
                    Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
                    rb.velocity = vo;
                    
                    timeDelayAtk = timeDelayBfAttacks;
                    //End of attack code

                    _alreadyAttacked = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }
        }
        private void ResetAttack()
        {
            _alreadyAttacked = false;
            _animator.SetBool(IsAttack,false);
        }

        private static Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
        {
            Vector3 distance = target - origin;
            Vector3 distanceXZPlane = distance;
            distanceXZPlane.y = 0f;

            float distanceY = distance.y;
            float distanceXZ = distanceXZPlane.magnitude;

            float vxz = distanceXZ / time;
            float vy = distanceY / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

            Vector3 result = distanceXZPlane.normalized;
            result *= vxz;
            result.y = vy;

            return result;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var position = transform.position;
            Gizmos.DrawWireSphere(position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, sightRange);
        }
        
        private void AlertSprite(bool isHide)
        {
            alertSpriteRen.enabled = isHide;
        }
    }
}
