using UnityEngine;

public class MonsterGlobal : MonoBehaviour
{
    public static MonsterGlobal Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the instance
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }

    public void PlayerInSafeSpace(Vector3 position)
    {
        GetComponent<AIMovement>().PlayerInSafeSpace(position);
    }

    public void PlayerExitSafeSpace()
    {
        GetComponent<AIMovement>().PlayerExitSafeSpace();
    }

}
