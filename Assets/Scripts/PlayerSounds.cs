using System.Collections;
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
    public AudioSource outOfBreath;
    public AudioSource typing;
    public AudioSource outside;
    public AudioSource inside;
    public AudioSource torch;

    public AudioSource jumpscare;

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

    public void PlayOutOfBreath()
    {
        outOfBreath.Play();
    }

    public void PlayTyping()
    {
        typing.Play();
    }

    public void StopTyping()
    {
        typing.Stop();
    }

    public void AmbientToInside()
    {
        outside.Stop();
        inside.Play();
    }

    public void AmbientToOutside()
    {
        inside.Stop();
        outside.Play();
    }

    public void PlayJumpscare()
    {
        jumpscare.Play();
    }

    public void PlayTorchClick()
    {
        torch.Play();
    }

}
