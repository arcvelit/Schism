using UnityEngine;
using System.Collections;

public class OOB : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OOB")) 
        {
            Debug.Log("tamer");
            StartCoroutine(BackToSpawn());
        }
    }

    IEnumerator BackToSpawn() 
    {
        yield return StartCoroutine(UIManager.Instance.IncreaseAlpha(1f, 1.0f));
        yield return StartCoroutine(UIManager.Instance.WriteMessage("You got lost and died lmao."));
        yield return new WaitForSeconds(1.0f); 

        // Teleport
        gameObject.transform.position = GetComponent<Movement>().spawnPoint.position;

        UIManager.Instance.ResetMessage();   

        yield return StartCoroutine(UIManager.Instance.IncreaseAlpha(0f, 1.0f));
    }

}
