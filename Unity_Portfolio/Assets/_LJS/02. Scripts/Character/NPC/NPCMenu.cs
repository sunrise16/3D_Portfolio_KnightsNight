using UnityEngine;

public class NPCMenu : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    public GameObject message;
    public GameObject menuUI;
    private AudioSource audio;
    public AudioClip[] audioClip;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 오디오 소스 컴포넌트 할당
        audio = GetComponent<AudioSource>();

        #endregion
    }

    void Update()
    {
        #region 실시간 변수값 갱신

        if (GetComponent<NPCMark>().iconActive == true)
        {
            message.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                MenuOpen();
            }
        }
        else
        {
            message.SetActive(false);
        }

        #endregion
    }

    #region NPC 메뉴 열기

    public void MenuOpen()
    {
        audio.clip = audioClip[0];
        audio.Play();
        MenuActivate(true);
        Cursor.lockState = CursorLockMode.None;
        GameObject.Find("Player").GetComponent<PlayerFSM>().playerStop = true;
    }

    #endregion

    #region NPC 메뉴 닫기

    public void MenuClose()
    {
        audio.clip = audioClip[1];
        audio.Play();
        MenuActivate(false);
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.Find("Player").GetComponent<PlayerFSM>().playerStop = false;
    }

    #endregion

    #region NPC UI 조절에 필요한 변수값 초기화

    public void MenuActivate(bool active)
    {
        menuUI.SetActive(active);
    }

    #endregion
}
