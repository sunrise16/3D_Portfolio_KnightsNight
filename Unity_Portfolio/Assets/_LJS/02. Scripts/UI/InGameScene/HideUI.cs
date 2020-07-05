using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    public GameObject[] hideObject;

    void Update()
    {
        if (GameManager.instance.isGameOver == true)
        {
            HideAllUI();
        }
    }

    void HideAllUI()
    {
        for (int i = 0; i < hideObject.Length; i++)
        {
            hideObject[i].SetActive(false);
        }
    }
}
