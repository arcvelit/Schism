using UnityEngine;
using System.Collections.Generic;


public class ProgressGlobal : MonoBehaviour
{
    public static ProgressGlobal Instance { get; private set; }

    private static string ERROR_SCROLL = "";
    private static HashSet<int> booksCollected = new HashSet<int>();
    private static Dictionary<int, string> scrolls = new Dictionary<int, string> { 
        { 1,  "." }, 
        { 2,  "." }, 
        { 3,  "." }, 
        { 4,  "." }, 
        { 5,  "." }, 
        { 6,  "." }, 
        { 7,  "." }, 
        { 8,  "." }, 
        { 9,  "." }, 
        { 10, "." }
    };

    // Awake ensures the singleton pattern is enforced
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the instance
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }


    public void CollectBookId(int id) => booksCollected.Add(id);
    public bool FoundManuscriptId(int id) => booksCollected.Contains(id);
    public string GetScrollContent(int id)
    {
        return scrolls.ContainsKey(id) ? scrolls[id] : ERROR_SCROLL;
    }

}
