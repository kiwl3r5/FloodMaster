using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enemy;
using Script.Player;
using UnityEngine;

public class EnemyManager : MonoBehaviour,IInteractable
{
    [SerializeField] private EnemyAi enemyAi;
    
    [Header("Stat")]
    public float healthPoint = 100;
    private float _defaultHealthPoint;
    public float damageReceived;
    public float fleeDuration = 30f;
    private float _defaultFleeDuration;

    [Header("State")]
    public bool isFlee;

    private void Start()
    {
        enemyAi = GetComponent<EnemyAi>();
        _defaultHealthPoint = healthPoint;
        _defaultFleeDuration = fleeDuration;
    }

    
    private void FixedUpdate()
    {
        if (isFlee && fleeDuration>0)
        {
            fleeDuration -= Time.deltaTime;
        }
        if (isFlee && fleeDuration<=0)
        {
            healthPoint = _defaultHealthPoint;
            fleeDuration = _defaultFleeDuration;
            IsFlee(false);
        }
    }

    private void IsFlee (bool isTrue)
    {
        isFlee = isTrue;
        enemyAi.isFlee = isTrue;
        eKeyEnable = !isTrue;
    }

    [Header("Interact")]
    [SerializeField] private bool eKeyEnable;
    [SerializeField] private string prompt;
    [SerializeField] private bool fKeyEnable;
    [SerializeField] private string prompt1;
    public string InteractionPrompt => prompt;
    public string InteractionPrompt1 => prompt1;
    public bool IsEKeyEnable => eKeyEnable;
    public bool IsFKeyEnable => fKeyEnable;
    public bool Interact(Interactor interactor)
    {
        if (healthPoint>0)
        {
            healthPoint -= damageReceived;
            var enemyPosition = transform.position;
            PlayerLocomotion.Instance.LookAtTarget(enemyPosition);
            if (!PlayerManager.Instance.isInteracting)
            {
                AnimatorManager.Instance.PlayTargetAnimation("ScareEnemy",true,0.1f);
                PlayerLocomotion.Instance.playerRigidbody.velocity = Vector3.zero;
            }
            if (healthPoint<=0)
            {
                IsFlee(true);
            }
            return true;
        }
        return false;
    }

    public bool Interact1(Interactor interactor)
    {
        return false;
    }
}
