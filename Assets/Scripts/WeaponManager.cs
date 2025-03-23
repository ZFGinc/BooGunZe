using Photon.Pun;
using UnityEngine;

public class WeaponManager : MonoBehaviour, IPunObservable
{
    private PhotonView _photonView;
    public int idWeapon;
    public Transform isRot;
    public GameObject[] Weapons;
    public float[] SpeedsFiring;
    public Transform[] Dooly;
    public GameObject[] Bullet;
    public float Timer = 0f;
    public float GranadeTimer;
    public Transform pointSpawnGranade;
    public Animator _animator;
    public PlayerControll _player;
    public GameObject Granade;
    public GameObject GranadeTRigger;
    public GameObject[] FirePart;
    MenuController _menu;

    [Header("UI Settings")] public GameObject[] panelsWeapons;
    
    public void OnPhotonSerializeView(PhotonStream streem, PhotonMessageInfo info){
        if(streem.IsWriting){
            streem.SendNext(idWeapon);
        }
        else{
            this.idWeapon = (int)streem.ReceiveNext();
        }
    }

    private void Start()
    {
        GameObject objMenu = GameObject.Find("MainCanvas");
        _menu = objMenu.GetComponent<MenuController>();
        _photonView = GetComponent<PhotonView>();
    }
    
    void Update ()
    {
        SetCkinWeapon();
        if (!_photonView.IsMine)
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (i == idWeapon) panelsWeapons[i].SetActive(true);
                else Weapons[i].SetActive(false);
            }
        }

        if (!_photonView.IsMine || _player.isDeath) return;

        if (_menu.isPause) return;

        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw > 0) idWeapon++;
        if (mw < 0) idWeapon--;

        if (idWeapon >= Weapons.Length) idWeapon = 0;
        if (idWeapon < 0) idWeapon = Weapons.Length - 1;

        if (SpeedsFiring[idWeapon] > Timer) Timer += Time.deltaTime;
        if (GranadeTimer < 5f) GranadeTimer += Time.deltaTime;

        for (int i = 0; i < Weapons.Length; i++)
        {
            if (i == idWeapon) panelsWeapons[i].SetActive(true);
            else panelsWeapons[i].SetActive(false);
        }
        GranadeTRigger.GetComponent<AttackMan>().Killer = gameObject;
        if (Input.GetButton("Fire2") && GranadeTimer > 5f)
        {
            GranadeTimer = 0;
            PhotonNetwork.Instantiate(Granade.name, pointSpawnGranade.position, isRot.rotation);
        }

        if (Input.GetButton("Fire1") && Timer > SpeedsFiring[idWeapon]) {
            Bullet[idWeapon].GetComponent<BulletScript>().Killer = this.gameObject;
            _animator.SetBool("isShot", true);
            Timer = 0;
            Vector3 spawnPointBullets = new Vector3(Dooly[idWeapon].position.x, Dooly[idWeapon].position.y, 50);
            PhotonNetwork.Instantiate(Bullet[idWeapon].name, spawnPointBullets, isRot.rotation);
            PhotonNetwork.Instantiate(FirePart[idWeapon].name, new Vector3(Dooly[idWeapon].position.x, Dooly[idWeapon].position.y, 0),  isRot.rotation);
        }      
    }

    public void SetCkinWeapon()
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            if (i == idWeapon) Weapons[i].SetActive(true);
            else Weapons[i].SetActive(false);

        }
    }
}
