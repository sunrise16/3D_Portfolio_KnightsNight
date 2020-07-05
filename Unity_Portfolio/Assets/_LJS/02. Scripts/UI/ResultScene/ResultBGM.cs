using UnityEngine;

public class ResultBGM : MonoBehaviour
{
    public static ResultBGM instance = null;
    private AudioSource audio;
    public AudioClip[] audioClip;
    private bool fadeOut;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        audio = GetComponent<AudioSource>();
        fadeOut = false;
    }

    void Update()
    {
        if (!audio.isPlaying)
        {
            audio.clip = audioClip[0];
            audio.Play();
        }

        if (fadeOut)
        {
            audio.volume -= (Time.deltaTime * 0.4f);
            if (audio.volume <= 0.0f)
            {
                audio.volume = 0.0f;
            }
        }
    }

    public void FadeOutBGM()
    {
        fadeOut = true;
    }
}
