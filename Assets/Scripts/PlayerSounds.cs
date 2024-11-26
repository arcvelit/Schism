using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    // Singleton instance
    public static PlayerSounds Instance { get; private set; }

    // Reference to AudioSource
    public AudioSource takeBook;
    public AudioSource takeItem;
    public AudioSource openDoor;
    public AudioSource closeDoor;

    // Awake ensures the singleton pattern is enforced
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the instance
            DontDestroyOnLoad(gameObject); // Keep it alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void PlayItemTake()
    {
        takeItem.Play();

    }

    public void PlayBookTake()
    {
        takeBook.Play();
    }

    public void PlayOpenDoor() 
    {
        openDoor.Play();
    }

    public void PlayCloseDoor() 
    {
        closeDoor.Play();
    }

}
