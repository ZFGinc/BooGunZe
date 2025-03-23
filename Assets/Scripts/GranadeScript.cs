using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GranadeScript : MonoBehaviour
{
    public float Timer = 0;
    public float TargetTime = 2f;
    public GameObject PartAndColl;
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > TargetTime)
        {
            PhotonNetwork.Instantiate(PartAndColl.name, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
