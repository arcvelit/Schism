using UnityEngine;
using System.Collections.Generic;


public class ProgressGlobal : MonoBehaviour
{
    public static ProgressGlobal Instance { get; private set; }

    public HashSet<int> booksCollected;

    // Awake ensures the singleton pattern is enforced
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the instance
            booksCollected = new HashSet<int>();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }


    public void CollectBookId(int id) => booksCollected.Add(id);
    public bool FoundManuscriptId(int id) => booksCollected.Contains(id);

}
