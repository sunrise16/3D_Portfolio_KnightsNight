using UnityEngine;

public class QuestReward : MonoBehaviour
{
    // 오디오 소스 컴포넌트
    private AudioSource audio;
    public AudioClip[] audioClip;
    // 퀘스트 보상 버튼
    public GameObject quest1Reward;
    public GameObject quest2Reward;

    void Start()
    {
        // 오디오 소스 컴포넌트 할당
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 퀘스트1 보상 버튼 활성, 비활성화
        if (DataManager.instance.quest1.getReward == true)
        {
            quest1Reward.SetActive(true);
        }
        else
        {
            quest1Reward.SetActive(false);
        }

        // 퀘스트2 보상 버튼 활성, 비활성화
        if (DataManager.instance.quest2.getReward == true)
        {
            quest2Reward.SetActive(true);
        }
        else
        {
            quest2Reward.SetActive(false);
        }
    }

    public void Quest1GetReward()
    {
        audio.clip = audioClip[0];
        audio.Play();
        DataManager.instance.totalScore += 5000;
        DataManager.instance.quest1.getReward = false;
    }

    public void Quest2GetReward()
    {
        audio.clip = audioClip[0];
        audio.Play();
        DataManager.instance.playerGold += 50;
        DataManager.instance.quest2.getReward = false;
    }
}
