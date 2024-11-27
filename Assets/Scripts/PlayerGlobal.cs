using UnityEngine;

public class PlayerGlobal : MonoBehaviour
{
    public static PlayerGlobal Instance { get; private set; }

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

    public Transform GetTransform() => gameObject.transform;

}
