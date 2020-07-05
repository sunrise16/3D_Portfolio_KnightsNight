using UnityEngine;

public class BonfireSound : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip[] audioClip;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audio.isPlaying)
        {
            audio.clip = audioClip[0];
            audio.Play();
        }
    }
}
