using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    private float angle;

    void Update()
    {
        angle += Time.deltaTime * 200;
        transform.rotation = Quaternion.Euler(new Vector3(-90, 0, angle));
        if (angle >= 360.0f)
        {
            angle = 0.0f;
        }
    }
}
