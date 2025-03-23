using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalChestSpawner : MonoBehaviour
{
    [Header("Medical Spawner")]
    private float Timer = 0;
    public float TargetTime = 5f;
    public Transform[] PointForMedical;
    public GameObject MedicalChest;

    private void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= TargetTime)
        {
            Timer = 0;
            PhotonNetwork.Instantiate(MedicalChest.name, PointForMedical[Random.Range(0, PointForMedical.Length)].position, Quaternion.identity);
        }
    }
}
