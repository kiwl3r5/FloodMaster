using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Player
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private Transform interactionPoint;
        [SerializeField] private float interactionPointRadius = 0.5f;
        [SerializeField] private LayerMask whatInteractable;
        [SerializeField] private InteractionPromptUI _interactionPromptUI;
        
        private readonly Collider[] _colliders = new Collider[3];
        [SerializeField] private int numFound;

        private IInteractable _interactable;

        private void Update()
        {
            numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, _colliders,
                whatInteractable);
            
            if (numFound>0)
            {
                _interactable = _colliders[0].GetComponent<IInteractable>();

                if (_interactable != null)
                {
                    if (!_interactionPromptUI.IsDisplayed)_interactionPromptUI.Setup(_interactable.InteractionPrompt,_interactable.InteractionPrompt1,_interactable.IsEKeyEnable,_interactable.IsFKeyEnable);
                    if (_interactionPromptUI.IsDisplayed)
                    {
                        _interactionPromptUI.ReCheckPrompt(_interactable.InteractionPrompt,_interactable.InteractionPrompt1);
                        _interactionPromptUI.ReCheckEnable(_interactable.IsEKeyEnable,_interactable.IsFKeyEnable);
                    }
                    if (Keyboard.current.eKey.wasPressedThisFrame && _interactable.IsEKeyEnable)
                    {
                        _interactable.Interact(this);
                    }
                    if (Keyboard.current.fKey.wasPressedThisFrame && _interactable.IsFKeyEnable)
                    {
                        if (_colliders[0].CompareTag("manhole"))
                        {
                            PlayerManager.Instance.stormDrainManager = _colliders[0].GetComponent<StormDrainManager>();
                        }
                        _interactable.Interact1(this);
                    }
                }
            }
            else
            {
                if (_interactable != null) _interactable = null;
                if (_interactionPromptUI.IsDisplayed)_interactionPromptUI.Close();
            }

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(interactionPoint.position,interactionPointRadius);
        }
    }
}