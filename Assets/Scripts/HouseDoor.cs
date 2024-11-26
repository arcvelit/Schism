using System.Collections;
using UnityEngine;

public class HouseDoor : MonoBehaviour
{
    public bool opened = false;
    public float side = 1;

    private static float openedRot = 80;
    private bool isRotating = false;

    public float duration = 1.0f;

    void OnMouseOver()
    {
        Debug.Log("This is a door");
        UIManager.Instance.SetLookatHouseDoor(this);
        UIManager.Instance.ShowHouseDoorInteraction();
    }

    void OnMouseExit()
    {
        UIManager.Instance.RemoveInteraction();
    }

    public void Interact()
    {
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
