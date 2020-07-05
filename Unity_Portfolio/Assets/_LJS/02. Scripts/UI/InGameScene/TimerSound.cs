using UnityEngine;

public class TimerSound : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip[] audioClip;
    private float currentTime;
    private float delayTime;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        currentTime = 0.0f;
        delayTime = 1.0f;
    }

    void Update()
    {
        audio.clip = audioClip[0];

        if (GameManager.instance.timeRemaining <= 5.0f && GameManager.instance.timeRemaining >= 1.0f)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= delayTime)
            {
                audio.Play();
                currentTime = 0.0f;
            }
        }
        else
        {
            currentTime = 0.0f;
        }
    }
}
