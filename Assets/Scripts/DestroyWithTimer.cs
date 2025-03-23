using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithTimer : MonoBehaviour
{
    public float Timer = 0;
    public float TargetTime = 1f;
    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer>TargetTime) Destroy(gameObject);
    }
}
