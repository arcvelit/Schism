using System.Collections;
using Interfaces;
using UnityEngine;

namespace Objects
{
    enum DoorState { Open, Closed, Locked }

    enum DoorType { Normal, Slide }

    enum DoorSide { Left, Right }
    
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private DoorType _doorType = DoorType.Normal;
        [SerializeField] private DoorState _doorState = DoorState.Closed;
        [SerializeField] private DoorSide _doorSide = DoorSide.Left;
        [SerializeField] private float openingTime = 0.5f;
        [SerializeField] private string _interactMessage;
        
        private bool _isRotating = false;
        private static readonly float OpeningAngle = 80;
        
        public string InteractMessage => _interactMessage;
        public void Interact()
        {
            Debug.Log("Interacting with " + gameObject.name);
            if (_isRotating)
            {
                return;
            }

            if (_doorState == DoorState.Open)
            {
                StartCoroutine(RotateDoor());
                _doorState = DoorState.Closed;
            }
            else if (_doorState == DoorState.Closed)
            {
                StartCoroutine(RotateDoor());
                _doorState = DoorState.Open;
            }
            else
            {
                _doorState = DoorState.Closed;
            }
        }

        private IEnumerator RotateDoor()
        {
            if (_isRotating)
            {
                yield break;
            }
            _isRotating = true;

            float elapsedTime = 0f;
            float doorSide = (_doorSide == DoorSide.Left ? 1 : -1);
            float startAngle = transform.eulerAngles.y;
            float targetAngle = startAngle + (_doorState == DoorState.Open ? -OpeningAngle * doorSide : OpeningAngle * doorSide);
            while (elapsedTime < openingTime)
            {
                float currentAngle = Mathf.Lerp(startAngle, targetAngle, elapsedTime / openingTime);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
            _isRotating = false;
        }
    }
}