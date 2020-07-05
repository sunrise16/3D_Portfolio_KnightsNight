using UnityEngine;

public class InGameBGM : MonoBehaviour
{
    public static InGameBGM instance = null;
    private AudioSource audio;
    public AudioClip[] audioClip;
    private bool fadeOut;
    private bool bgmCount;

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
        bgmCount = false;
    }

    void Update()
    {
        if (!audio.isPlaying && DataManager.instance.playerCurrentHP > 0.0f)
        {
            audio.clip = audioClip[0];
            audio.Play();
        }
        else if (bgmCount == false && DataManager.instance.playerCurrentHP <= 0.0f)
        {
            audio.clip = audioClip[1];
            audio.Play();
            bgmCount = true;
        }

        if (fadeOut)
        {
            audio.volume -= (Time.deltaTime * 0.5f);
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
