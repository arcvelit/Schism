using UnityEngine;
using System.Collections;

public class OOB : MonoBehaviour
{

    private static string OOB_MESSAGE = "You wandered away from the village and got lost...";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OOB")) 
        {
            StartCoroutine(UIManager.Instance.GameMessage(OOB_MESSAGE, 1f, GetComponent<Movement>()));
        }
    }

}
