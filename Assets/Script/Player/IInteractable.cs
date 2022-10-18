using UnityEngine;

namespace Script.Player
{
    public interface IInteractable
    {
        public string InteractionPrompt { get; }
        public string InteractionPrompt1 { get; }
        public bool IsEKeyEnable { get; }
        public bool IsFKeyEnable { get; }
        public bool Interact(Interactor interactor);
        public bool Interact1(Interactor interactor);
    }
}