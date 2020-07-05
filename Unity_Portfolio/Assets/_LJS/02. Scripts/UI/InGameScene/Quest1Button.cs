using UnityEngine;
using UnityEngine.UI;

public class Quest1Button : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = gameObject.transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        // 버튼 이미지 변경
        switch (DataManager.instance.quest1.questState)
        {
            case QuestCondition.QuestType.None:
                GetComponent<Image>().color = new Color(255, 170, 0, 1.0f);
                text.text = "ACCEPT";
                text.color = new Color(0, 0, 0, 1.0f);
                break;
            case QuestCondition.QuestType.Ongoing:
                GetComponent<Image>().color = new Color(0, 255, 255, 1.0f);
                text.text = "ONGOING";
                text.color = new Color(0, 0, 0, 1.0f);
                break;
            case QuestCondition.QuestType.Failed:
                GetComponent<Image>().color = new Color(255, 0, 255, 1.0f);
                text.text = "FAILED";
                text.color = new Color(255, 255, 255, 1.0f);
                break;
            case QuestCondition.QuestType.Completed:
                GetComponent<Image>().color = new Color(0, 0, 255, 1.0f);
                text.text = "COMPLETED";
                text.color = new Color(255, 255, 255, 1.0f);
                break;
        }
    }
}
