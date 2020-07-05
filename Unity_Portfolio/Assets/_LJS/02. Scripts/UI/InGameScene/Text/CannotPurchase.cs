using UnityEngine;
using UnityEngine.UI;

public class CannotPurchase : MonoBehaviour
{
    public float textAlpha;

    void Start()
    {
        textAlpha = 0.0f;
    }

    void Update()
    {
        textAlpha -= Time.deltaTime;

        if (textAlpha <= 0.0f)
        {
            textAlpha = 0.0f;
        }

        gameObject.GetComponent<Text>().color = new Color(255, 255, 255, textAlpha);
    }
}
