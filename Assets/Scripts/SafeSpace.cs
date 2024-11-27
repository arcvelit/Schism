using UnityEngine;

public class SafeSpace : MonoBehaviour
{

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            SetSafeSpace();
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            SetSafeSpace();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            RemoveSafeSpace();
        }
    }

    void SetSafeSpace()
    {
        Vector3 center = GetComponent<BoxCollider>().bounds.center;
        MonsterGlobal.Instance.PlayerInSafeSpace(center);
        PlayerSounds.Instance.AmbientToInside();
    }

    void RemoveSafeSpace()
    {
        MonsterGlobal.Instance.PlayerExitSafeSpace();
        PlayerSounds.Instance.AmbientToOutside();
    }

}
