using System.Collections;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip[] audioClip;
    private GameObject player;
    public GameObject regenEffect;
    public GameObject skipMessage;
    private float distance;
    private float delayTime;
    private float currentTime;

    void Start()
    {
        // 타겟 오브젝트 지정 (플레이어)
        player = GameObject.Find("Player");
        delayTime = 1.0f;

        // 오디오 소스 컴포넌트 할당
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        PlayerRegen();
    }

    void PlayerRegen()
    {
        if (GameManager.instance.isWaitTime == true && distance < 8.0f)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= delayTime)
            {
                audio.clip = audioClip[0];
                audio.Play();
                StartCoroutine(RegenEffect());
                DataManager.instance.playerCurrentHP += 10;
                DataManager.instance.playerCurrentMP += 10;
                if (DataManager.instance.playerCurrentHP >= DataManager.instance.playerMaxHP)
                {
                    DataManager.instance.playerCurrentHP = DataManager.instance.playerMaxHP;
                }
                if (DataManager.instance.playerCurrentMP >= DataManager.instance.playerMaxMP)
                {
                    DataManager.instance.playerCurrentMP = DataManager.instance.playerMaxMP;
                }
                currentTime = 0.0f;
            }
        }
        else
        {
            currentTime = 0.0f;
        }

        if (GameManager.instance.isWaitTime == true && distance < 4.0f)
        {
            skipMessage.SetActive(true);

            if (GameManager.instance.timeRemaining > 1.0f && Input.GetKeyDown(KeyCode.F))
            {
                SkipTurn();
            }
        }
        else
        {
            skipMessage.SetActive(false);
        }
    }

    public void SkipTurn()
    {
        audio.clip = audioClip[1];
        audio.Play();
        GameManager.instance.timeRemaining = 1.0f;
    }

    IEnumerator RegenEffect()
    {
        // 이펙트 출력
        GameObject effect = Instantiate(regenEffect,
            new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), regenEffect.transform.rotation);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(1.0f);

        Destroy(effect);
    }
}
