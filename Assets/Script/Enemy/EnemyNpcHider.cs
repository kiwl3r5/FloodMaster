using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Enemy
{
    public class EnemyNpcHider : MonoBehaviour
    {
        [SerializeField] private GameObject npcMesh;
        [SerializeField] private float chance;
        private BoxCollider _boxCollider;
        private EnemyAi _enemyAi;

        private void Awake()
        {
            _enemyAi = GetComponent<EnemyAi>();
        }

        private void Start()
        {
            _boxCollider = GetComponent<BoxCollider>();
            chance = Random.value;
        }

        private void Update()
        {
            if (LevelScaling.Instance.levelScale == 1)
            {
                Hide(true);
            }
            if (LevelScaling.Instance.levelScale == 2)
            {
                if (_enemyAi.isDoDamage)
                {
                    Hide(true);
                }
                if (!_enemyAi.isDoDamage)
                {
                    if (chance<=0.5)
                    {
                        Hide(false);
                    }
                    if (chance>0.5)
                    {
                        Hide(true);
                    }
                }
            }
            if (LevelScaling.Instance.levelScale == 3)
            {
                if (_enemyAi.isDoDamage)
                {
                    if (chance<=0.2)
                    {
                        Hide(false);
                    }
                    if (chance>0.2)
                    {
                        Hide(true);
                    }
                }
                if (!_enemyAi.isDoDamage)
                {
                    Hide(false);
                }
            }

            if (LevelScaling.Instance.levelScale >= 4)
            {
                if (_enemyAi.isDoDamage)
                {
                    if (chance<=0.5)
                    {
                        Hide(false);
                    }
                    if (chance>0.5)
                    {
                        Hide(true);
                    }
                }
                if (!_enemyAi.isDoDamage)
                {
                    Hide(false);
                }
            }
        }

        private void Hide(bool isHide)
        {
            switch (isHide)
            {
                case true:
                    npcMesh.SetActive(false);
                    _boxCollider.enabled = false;
                    break;
                case false:
                    npcMesh.SetActive(true);
                    _boxCollider.enabled = true;
                    break;
            }
        }
    }
}