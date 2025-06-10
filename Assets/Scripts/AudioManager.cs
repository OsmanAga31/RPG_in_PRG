using System.Collections;
using UnityEngine;

// With the help of A.I. but changed it a little bit to suit my needs and to suit the way we learned. e.g. [SerializeField] instead of public, and I made more variables than the A.I. suggested and removed some.
public class AudioManager : MonoBehaviour
{
    [Header("Audio Setup")]
    [SerializeField] private AudioClip doorSoundOpen; // AudioSource for door opening sound
    [SerializeField] private AudioClip doorSoundClose; // AudioSource for door closing sound
    [SerializeField] private AudioClip chestSoundOpen; // AudioSource for chest opening sound
    [SerializeField] private AudioClip chestSoundWow; // AudioSource for chest wow sound

    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSource;

    [Header("Delay Settings")]
    [SerializeField] private float doorSoundDelay = 0.25f; // Delay for door sound
    [SerializeField] private float chestSoundDelay = 0.25f; // Delay for chest sound

    IEnumerator PlaySound(AudioClip sound, float delay)
    {
        // Play the sound after a specified delay
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(sound);
    }

    public void PlayDoorSound()
    {
        // Play the door sound based on whether it's opening or closing
        audioSource.PlayOneShot(doorSoundOpen);
        StartCoroutine(PlaySound(doorSoundClose, doorSoundDelay));
    }
    public void PlayChestSound()
    {
        // Play the chest opening sound
        audioSource.PlayOneShot(chestSoundOpen);
        audioSource.volume = 0.5f; // Set volume to 50% for the chest opening sound
        StartCoroutine(PlaySound(chestSoundWow, chestSoundDelay)); // Play the chest sound with a delay
    }
}
