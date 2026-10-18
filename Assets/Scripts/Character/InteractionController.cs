using Interfaces;
using UnityEngine;

namespace Character
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactionDistance;

        private IInteractable _currentInteractable;

        private Ray _lookAtVector;

        public void RequestInteract()
        {
            _currentInteractable?.Interact();
        }

        private void UpdateCurrentInteractable()
        {
            _lookAtVector = playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            Physics.Raycast(_lookAtVector, out RaycastHit hitInfo, interactionDistance);
            
            _currentInteractable = hitInfo.collider?.GetComponent<IInteractable>();
        }

        private void UpdateInteractionText()
        {
            if (_currentInteractable == null)
            {
                // Set interaction text to ""
                return;
            }
            
            // Set interaction text to the serialized field value
        }

        private void Awake()
        {
            if (!playerCamera)
            {
                Debug.LogError("No player camera found in the InteractionController of " + gameObject.name + ". Make sure to initialize one");
            }
        }
        
        private void Update()
        {
            UpdateCurrentInteractable();
            UpdateInteractionText();
        }
    }
}