using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource01;
    public AudioSource audioSource02;
    public AudioSource audioSource03;
    public AudioSource audioSource04;

    public AudioClip background;
    public AudioClip gameOver;
    public AudioClip points;
    public AudioClip win;

    private void Start()
    {
        AudioSource[] allSources =
            FindObjectsByType<AudioSource>(
                FindObjectsSortMode.None
            );

        foreach (AudioSource src in allSources)
        {
            if (src != null)
            {
                src.Stop();
            }
        }

        if (audioSource01 != null &&
            background != null)
        {
            audioSource01.clip = background;
            audioSource01.loop = true;
            audioSource01.Play();

            Debug.Log(
                "AudioManager started with clip: " +
                background.name
            );
        }
        else
        {
            Debug.LogWarning(
                "AudioManager: audioSource01 or background clip is not assigned."
            );
        }
    }
}