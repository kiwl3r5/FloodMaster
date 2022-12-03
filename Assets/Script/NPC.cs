using System;
using System.Collections;
using System.Collections.Generic;
using MidniteOilSoftware;
using Script.Manager;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    private Animator _animator;
    
    [SerializeField] private GameObject wayPoints;
    [SerializeField] private float curSpeed;
    //[SerializeField] private float delay = 1f;
    //private float _defaultDelay;
    //[SerializeField] private Vector3 walkPoint;
    [SerializeField] private GameObject playerMesh;
    
    private bool _walkPointSet;
    private Vector3 _previousPosition;
    private float _defaultWalkSpeed;
    
    private static readonly int velocity = Animator.StringToHash("Velocity");
    private static readonly int IsSwimming = Animator.StringToHash("IsSwimming");

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        _defaultWalkSpeed = agent.speed;
    }

    /*private void Start()
    {
        _defaultDelay = delay;
        playerMesh.SetActive(false);
    }*/

    private void FixedUpdate()
    {
        if (GameManager.Instance.isEmptyPool)
        {
            ObjectPoolManager.DespawnGameObject(gameObject);
            ObjectPoolManager.EmptyPool();
            return;
        }
        /*var position = transform.position;
        var positionXZ = new Vector3(position.x,0,position.z);
        var positionAf = position;
        Vector3 curMove = positionXZ - _previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        curSpeed /= agent.speed;
        _previousPosition = positionAf;
        curSpeed = Mathf.Round(curSpeed * 10f) / 10f;*/
        curSpeed = agent.speed;
        
        agent.speed = FloodSystem.Instance.floodPoint switch
        {
            <= 10 => _defaultWalkSpeed,
            > 10 and <= 25 => _defaultWalkSpeed*2f,
            >= 25 => _defaultWalkSpeed*0.5f,
            _ => agent.speed
        };
        /*agent.speed = FloodSystem.Instance.floodPoint switch
        {
            >= 50 when agent.speed > 3f => 1.5f,
            < 50 when agent.speed <= 1.5f => _defaultWalkSpeed,
            _ => agent.speed
        };*/
        Fleeing();
        _animator.SetFloat(velocity,curSpeed,0.1f,Time.deltaTime);
        _animator.SetBool(IsSwimming, FloodSystem.Instance.floodPoint >= 50);
    }

    private void OnEnable()
    {
        WalkPointSetup();
    }

    private void Fleeing()
    {
        agent.SetDestination(wayPoints.transform.position);
        
        Vector3 distanceToWalkPoint = transform.position - wayPoints.transform.position;

        //Waypoint reached
        if (distanceToWalkPoint.magnitude < 1f){
            ObjectPoolManager.DespawnGameObject(gameObject);
        }
    }

    private void WalkPointSetup()
    {
        if (transform.position.x>0)
        {
            wayPoints = GameObject.FindWithTag("NPCEnd");
        }
        if (transform.position.x<0)
        {
            wayPoints = GameObject.FindWithTag("NPCEndL");
        }
    }

    /*private void ReSpawn()
    {
        transform.position = startVector3;
    }*/
    
    /*private static float RandomSystem(float range1, float range2)
    {
        var ranNum = Random.Range(range1, range2);
        return ranNum;
    }*/
}
