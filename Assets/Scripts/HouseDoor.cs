using System.Collections;
using UnityEngine;

public class HouseDoor : MonoBehaviour
{
    public bool opened = false;
    public float side = 1;

    private static float openedRot = 80;
    private bool isRotating = false;

    private float duration = 0.5f;

    void OnMouseOver()
    {
        Vector3 diff = gameObject.transform.position - PlayerGlobal.Instance.GetTransform().position;
        if (diff.magnitude > 3f || isRotating) 
        {
            OnMouseExit();
        }
        else 
        {
            UIManager.Instance.SetLookatHouseDoor(this);
            UIManager.Instance.ShowHouseDoorInteraction();
        }
    }

    void OnMouseExit()
    {
        UIManager.Instance.RemoveInteraction();
    }

    public void Interact()
    {
        if (isRotating) return;

        if (opened) PlayerSounds.Instance.PlayCloseDoor();   
        else PlayerSounds.Instance.PlayOpenDoor(); 

        UIManager.Instance.RemoveInteraction();
        StartCoroutine(Rotate());
        opened = !opened;
    }

    public IEnumerator Rotate()
    {
        if (isRotating) yield break;
        isRotating = true;

        float elapsedTime = 0f;
        float startAngle = transform.eulerAngles.y;
        float targetAngle = startAngle + (opened ? -openedRot * side : openedRot * side);

        while (elapsedTime < duration)
        {
            float currentAngle = Mathf.Lerp(startAngle, targetAngle, elapsedTime / duration);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
        isRotating = false;
    }
}
