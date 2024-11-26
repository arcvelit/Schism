using UnityEngine;
using System.Collections;

public class OOB : MonoBehaviour
{

    private static string OOB_MESSAGE = "You wandered away from the village and got lost...";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OOB")) 
        {
            StartCoroutine(BackToSpawn());
        }
    }

    IEnumerator BackToSpawn() 
    {
        yield return StartCoroutine(UIManager.Instance.IncreaseAlpha(1f, 1.0f));
        yield return StartCoroutine(UIManager.Instance.WriteMessage(OOB_MESSAGE));
        yield return new WaitForSeconds(1.0f); 

        // Teleport
        GetComponent<Movement>().BackToSpawn();

        UIManager.Instance.ResetMessage();   

        yield return StartCoroutine(UIManager.Instance.IncreaseAlpha(0f, 1.0f));
    }

}
