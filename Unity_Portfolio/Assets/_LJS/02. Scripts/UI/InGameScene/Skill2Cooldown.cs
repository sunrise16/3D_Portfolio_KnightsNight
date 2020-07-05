using UnityEngine;
using UnityEngine.UI;

public class Skill2Cooldown : MonoBehaviour
{
    void Update()
    {
        GetComponent<Image>().fillAmount = DataManager.instance.skill2CooldownDelay / 10;
        if (GetComponent<Image>().fillAmount <= 0.0f)
        {
            gameObject.SetActive(false);
        }
    }
}
