using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        public string InteractMessage { get; }
        public void Interact();
    }
}