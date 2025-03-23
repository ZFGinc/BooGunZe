using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float SpeedMove = 25f;
    public GameObject Blood;
    public string DopTag = "Monster";
    public int Damage;
    public bool isBullet = true;
    public GameObject DamageSound;
    public GameObject Killer;
    PlayerControll player;

    private void Start()
    {
        player = Killer.GetComponent<PlayerControll>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * SpeedMove);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        if (other.gameObject.tag == DopTag)
        {
	        PhotonNetwork.Instantiate(DamageSound.name, new Vector3(transform.position.x,transform.position.y,0), Quaternion.identity);
            PhotonNetwork.Instantiate(Blood.name, transform.position, this.gameObject.transform.rotation);
            if (isBullet)
            {
                other.gameObject.GetComponent<MonsterControll>().Health -= Damage;
                player.Score += 1;
            }
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
