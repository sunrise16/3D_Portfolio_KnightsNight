using UnityEngine;
using UnityEngine.UI;

public class Skill1Cooldown : MonoBehaviour
{
    void Update()
    {
        GetComponent<Image>().fillAmount = DataManager.instance.skill1CooldownDelay / 8;
        if (GetComponent<Image>().fillAmount <= 0.0f)
        {
            gameObject.SetActive(false);
        }
    }
}
