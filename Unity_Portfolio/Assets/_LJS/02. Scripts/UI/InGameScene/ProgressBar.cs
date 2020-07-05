using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Image image;
    private PlayerFSM playerFSM;

    void Start()
    {
        image = GetComponent<Image>();
        playerFSM = GameObject.Find("Player").GetComponent<PlayerFSM>();
    }

    void Update()
    {
        image.fillAmount = 1.0f - (playerFSM.comboDelay / 5.0f);
    }
}
