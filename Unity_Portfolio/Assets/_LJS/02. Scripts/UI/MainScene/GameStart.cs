using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    private Image image;
    private Camera camera;
    private Animation anim;
    public GameObject fadeImage;
    private AudioSource audio;
    public AudioClip[] audioClip;
    private int alphaCount;
    private float alphaDelay;
    private float textAlpha;
    private float imageAlpha;
    private bool alphaChange;
    private bool click;
    private bool clickAudio;

    void Start()
    {
        image = GetComponent<Image>();
        camera = GameObject.Find("Camera").GetComponent<Camera>();
        anim = GetComponent<Animation>();
        audio = GetComponent<AudioSource>();
        alphaCount = 0;
        alphaDelay = 0.0f;
        textAlpha = 1.0f;
        imageAlpha = 0.0f;
        alphaChange = false;
        click = false;
    }

    void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, textAlpha);

        if (click == false)
        {
            if (alphaChange == true)
            {
                textAlpha += Time.deltaTime;
                if (textAlpha >= 1.0f)
                {
                    alphaChange = false;
                }
            }
            else
            {
                textAlpha -= Time.deltaTime;
                if (textAlpha <= 0.5f)
                {
                    alphaChange = true;
                }
            }
        }
        else
        {
            if (alphaCount <= 6)
            {
                alphaDelay += Time.deltaTime;
                if (alphaDelay >= 0.1f)
                {
                    if (alphaChange == true)
                    {
                        textAlpha = 0.0f;
                        alphaChange = false;
                    }
                    else
                    {
                        textAlpha = 1.0f;
                        alphaChange = true;
                    }
                    alphaDelay = 0.0f;
                    alphaCount++;
                }
            }
            else
            {
                FadeIn();
                MainBGM.instance.FadeOutBGM();
            }
        }
    }

    public void Click()
    {
        if (click == false)
        {
            click = true;
            alphaChange = true;
            audio.clip = audioClip[0];
            audio.Play();
            anim.enabled = false;
        }
    }

    void FadeIn()
    {
        fadeImage.SetActive(true);
        MainFadeImage.instance.alphaChange = false;
    }
}