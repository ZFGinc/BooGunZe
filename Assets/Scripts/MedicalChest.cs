using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalChest : MonoBehaviour
{
    public int HealthFoMonster = 500;
    public int HealthForMan = 250;

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Monster")
        {
            MonsterControll player = coll.gameObject.GetComponent<MonsterControll>();
            if (player.Health <= 2000)
            {
                PhotonNetwork.Destroy(gameObject);
                player.Health += HealthFoMonster;
            }
            else if(player.Health > 2000 && player.Health < 2500)
            {
                PhotonNetwork.Destroy(gameObject);
                player.Health = 2500;
            }
        }

        if (coll.gameObject.tag == "Player")
        {
            PlayerControll player = coll.gameObject.GetComponent<PlayerControll>();
            if (player.Health <= 250)
            {
                PhotonNetwork.Destroy(gameObject);
                player.Health += HealthForMan;
            }
            else if (player.Health > 250 && player.Health < 500)
            {
                PhotonNetwork.Destroy(gameObject);
                player.Health = 500;
            }
        }
    }
}
