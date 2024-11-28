using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;


public class ProgressGlobal : MonoBehaviour
{
    public static ProgressGlobal Instance { get; private set; }

    private static string ERROR_SCROLL = "";
    private static int deposited;
    private static HashSet<int> booksCollected = new HashSet<int>();
    private static HashSet<int> difficultyLevels = new HashSet<int> { 2, 5, 7, 10 };
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


    public void CollectBookId(int id) 
    {
        booksCollected.Add(id);
        if(difficultyLevels.Contains(InventoryManager.Instance.manuscripts + 1))
        {
            MonsterGlobal.Instance.IncreaseDifficulty();
        }
    } 
    public bool FoundManuscriptId(int id) => booksCollected.Contains(id);
    public string GetScrollContent(int id)
    {
        return scrolls.ContainsKey(id) ? scrolls[id] : ERROR_SCROLL;
    }

    public void CheckProgress()
    {
        deposited++;
        // Update monster difficulty
        if (deposited == InventoryManager.PUZZLE_OBJECTIVE)
        {
            StartCoroutine(RunEndgame());
        }
    }

    public IEnumerator RunEndgame()
    {
        UIManager.Instance.Blackout();

        yield return new WaitForSeconds(2f);

        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("SuccessPanel");
    }

}
