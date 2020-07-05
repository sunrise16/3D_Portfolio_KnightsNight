using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameFadeImage : MonoBehaviour
{
    public static InGameFadeImage instance = null;
    private Image fadeImage;
    private float imageAlpha;
    public bool alphaChange;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        // instance에 할당된 클래스의 인스턴스가 다를 경우 새로 생성된 클래스를 의미
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        // 다른 씬으로 넘어가더라도 삭제하지 않고 유지
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        fadeImage = GetComponent<Image>();
        imageAlpha = 1.0f;
        alphaChange = true;
    }

    // Update is called once per frame
    void Update()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, imageAlpha);

        if (alphaChange == true)
        {
            imageAlpha -= Time.deltaTime;

            if (imageAlpha <= 0.0f)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            imageAlpha += Time.deltaTime;

            if (imageAlpha >= 1.0f)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 1.0f;
                SceneManager.LoadScene("ResultScene");
            }
        }
    }
}
