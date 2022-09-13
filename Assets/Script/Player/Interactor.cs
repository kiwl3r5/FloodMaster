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
                    if (!_interactionPromptUI.IsDisplayed)_interactionPromptUI.Setup(_interactable.InteractionPrompt);

                    if (Keyboard.current.eKey.wasPressedThisFrame) _interactable.Interact(this);
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