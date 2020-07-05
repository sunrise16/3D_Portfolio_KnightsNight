using System.Collections;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class CoinLoot : MonoBehaviour
{
    public GameObject rootEffect;
    public GameObject lightEffect;
    private AudioSource audio;
    public AudioClip[] audioClip;
    private GameObject player;
    private float distance;
    private bool lootCheck;
    private float coinRemainTime;
    private bool coinBlink;
    private float coinBlinkTime;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        lootCheck = false;
        coinRemainTime = 0.0f;
        coinBlink = false;
        coinBlinkTime = 0.0f;
    }

    void Update()
    {
        // 코인 지속시간 감소
        if (lootCheck == false)
        {
            coinRemainTime += Time.deltaTime;

            // 지속시간이 얼마 남지 않았을 경우 깜빡임 효과
            if (coinRemainTime >= 17.0f)
            {
                coinBlinkTime += Time.deltaTime;
                if (coinBlinkTime >= 0.1f)
                {
                    coinBlinkTime = 0.0f;
                    if (coinBlink == false)
                    {
                        coinBlink = true;
                        GetComponent<MeshRenderer>().enabled = false;
                    }
                    else
                    {
                        coinBlink = false;
                        GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }

            // 코인 지속시간 초과 시 제거
            if (coinRemainTime >= 20.0f)
            {
                Destroy(gameObject);
            }
        }

        // 플레이어와의 거리값 계산
        distance = Vector3.Distance(transform.position, player.transform.position);

        // 플레이어가 접근했을 경우 코인 루팅
        if (lootCheck == false && Mathf.Abs(distance) <= 2.5f)
        {
            audio.clip = audioClip[0];
            audio.Play();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            StartCoroutine(LootEffect());
            StartCoroutine(LightEffect());
            StartCoroutine(Destroy());
            DataManager.instance.playerGold++;
            lootCheck = true;
        }
    }

    IEnumerator LootEffect()
    {
        // 이펙트 출력
        GameObject effect = Instantiate(rootEffect, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), transform.rotation);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(4.0f);

        Destroy(effect);
    }

    IEnumerator LightEffect()
    {
        // 이펙트 출력
        GameObject effect = Instantiate(lightEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(4.0f);

        Destroy(effect);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4.0f);

        Destroy(gameObject);
    }
}
