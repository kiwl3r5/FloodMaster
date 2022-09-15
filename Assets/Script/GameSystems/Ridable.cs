using System;
using Script.Player;
using UnityEngine;

namespace Script.GameSystems
{
    public class Ridable : MonoBehaviour,IInteractable
    {
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        [SerializeField] private GameObject Player;
        [SerializeField] private CapsuleCollider PlayerCollider;
        [SerializeField] private Transform defaultPlayerTransform;
        [SerializeField] private bool isDriving;

        private void Start()
        {
            _playerManager = PlayerManager.Instance;
            _playerLocomotion = PlayerLocomotion.Instance;
            Player = GameObject.FindGameObjectWithTag("Player");
            defaultPlayerTransform = Player.transform;
            PlayerCollider = Player.GetComponent<CapsuleCollider>();
        }

        private void Update()
        {
            if (isDriving)
            {
                var o = gameObject;
                Player.transform.position = o.transform.position;
                Player.transform.rotation = o.transform.rotation;
            }
        }

        private void IsDriving(bool isDriving)
        {
            //_playerManager.enabled = !isDriving;
            _playerLocomotion.isDriving = isDriving;
            //_playerLocomotion.enabled = !isDriving;
            PlayerCollider.enabled = !isDriving;
            this.isDriving = isDriving;
        }

        [SerializeField] private string prompt;
        public string InteractionPrompt => prompt;
        public bool Interact(Interactor interactor)
        {
            if (!isDriving)
            {
                IsDriving(true);
                return true;
            }
            IsDriving(false);
            return false;
        }
    }
}
