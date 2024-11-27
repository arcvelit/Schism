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
    public AudioSource bookClose;
    public AudioSource monument;
    public AudioSource chased;

    public AudioSource jumpscare;

    private bool playerinside;

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
        if (playerinside) return;
        playerinside = true;
        outside.Pause();
        inside.Play();
    }

    public void AmbientToOutside()
    {
        if (!playerinside) return;
        playerinside = false;
        inside.Pause();
        outside.Play();
    }

    public void PlayJumpscare()
    {
        jumpscare.Play();
    }

    public bool JumpscarePlaying() => jumpscare.isPlaying;

    public void PlayTorchClick()
    {
        torch.Play();
    }

    public void PlayBookClose()
    {
        bookClose.Play();
    }

    public void PlayMonumentCollect()
    {
        monument.Play();
    }

    public void PlayChased()
    {
        chased.Play();
    }

    public void StopChased()
    {
        chased.Stop();
    }

}
