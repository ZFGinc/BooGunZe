using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterControll : MonoBehaviour, IPunObservable
{
    public PhotonView _photonViewMonster;
    public Transform skinToFlip;
    Rigidbody2D rigidbody2D;
    public Animator[] _animator;
    public float SpeedMove = 12f;
    public float JumpForce = 25f;
    public GameObject playerCamera;
    public TextMeshPro NickNameText;
    public bool isDown = false;
    public bool isGround = false;
    public bool isJump = false;
    public int id_monster_skin;
    public GameObject[] Skins;
    public int Jumps = 0;
    public bool isFlatformFoot = false;
    public GameObject PanelDeath;
    public int Health = 250;
    public TextMeshPro HealthText;
    private bool isDeath = false;
    float Timer =0f;
    public GameObject[] colliderAttack;
    public Transform spawnTrigger;
    public float[] SpeedAttack;
    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    public GameObject JumpPart;
    public Transform PointJump;
    public Transform KislotaPoint;
    public GameObject Kislota;
    public float KislotaTimer;
    float tStep = 0f;
    public GameObject StepPrefab;
    public bool isMove = false;
    public float TimerAttack = 0f;
    public int Score = 0;

    MenuController _menu;


    public void OnPhotonSerializeView(PhotonStream streem, PhotonMessageInfo info){
        if(streem.IsWriting){
            streem.SendNext(id_monster_skin);
            streem.SendNext(Health);
            streem.SendNext(Score);
        }
        else{
            this.id_monster_skin = (int)streem.ReceiveNext();
            this.Health = (int)streem.ReceiveNext();
            this.Score = (int)streem.ReceiveNext();
        }
    }
    void Start()
    {
        Health = 2500;
        GameObject objMenu = GameObject.Find("MainCanvas");
        _menu = objMenu.GetComponent<MenuController>();
        id_monster_skin = PlayerPrefs.GetInt("SkinMonster");
        _photonViewMonster = GetComponent<PhotonView>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        NickNameText.SetText(_photonViewMonster.Owner.NickName);
        if (_photonViewMonster.IsMine)
        {
            NickNameText.color = Color.yellow;
        }
    }

    void Update()
    {
        SetSkinMonster();
        HealthText.SetText(Health.ToString());
        
        if (!_photonViewMonster.IsMine) return;
        if (Health < 0) Health = 0;
        
        if (Health < 1 && !isDeath)
        {
            OnDie();
        }

        if (isDeath)
        {
            Timer+= Time.deltaTime;
            if (Timer > 1.5f)
            {
                Respawn();
                Timer = 0;
            }
        }
        
        if (isDeath) return;
        playerCamera.SetActive(true);
        if (Score < 0) Score = 0;

        if (Input.GetKeyUp(KeyCode.A))
        {
            isMove = false;
            _animator[id_monster_skin].SetBool(IsRun, false);

        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            isMove = false;
            _animator[id_monster_skin].SetBool(IsRun, false);
        }

        if (_menu.isPause) return;

        if (Input.GetKeyDown(KeyCode.Space)) isJump = true;
        else isJump = false;
        
        if (Input.GetKeyDown(KeyCode.LeftShift)) isDown = true;
        else isDown = false;

        if (isJump && isGround && Jumps==0)
        {
            PhotonNetwork.Instantiate(JumpPart.name, PointJump.position, Quaternion.identity);
            isJump = false;
            Jumps++;
            rigidbody2D.AddForce(Vector2.up * JumpForce * 500f);
            isGround = false;
        }
        if (isFlatformFoot && isDown)
        {
            isFlatformFoot = false;
            Vector2 DownPlatform = new Vector2(transform.position.x, transform.position.y - .85f);
            transform.position = DownPlatform;
        }

        if (tStep < 0.3) tStep += Time.deltaTime;
        if (isMove && tStep > .3f && isGround)
        {
            tStep = 0f;
            AudioSource _step = StepPrefab.GetComponent<AudioSource>();
            _step.pitch = Random.Range(0.7f, 0.9f);
            PhotonNetwork.Instantiate(StepPrefab.name, PointJump.position, Quaternion.identity);

        }

        if (Input.GetKey(KeyCode.A))
        {
            _animator[id_monster_skin].SetBool(IsRun, true);
	    isMove = true;
            skinToFlip.rotation = new Quaternion(0,180,0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
	    isMove = true;
            _animator[id_monster_skin].SetBool(IsRun, true);
            skinToFlip.rotation = new Quaternion(0,0,0, 0);
        }

        if (SpeedAttack[id_monster_skin] > TimerAttack) TimerAttack += Time.deltaTime;

        if (Input.GetButton("Fire1") && TimerAttack >= SpeedAttack[id_monster_skin]) 
        {
            colliderAttack[id_monster_skin].GetComponent<AttackMan>().Killer = this.gameObject;
            _animator[id_monster_skin].SetBool(IsAttack, true);
            PhotonNetwork.Instantiate(colliderAttack[id_monster_skin].name, spawnTrigger.position, Quaternion.identity);
            TimerAttack = 0;
        }  
        
        if (KislotaTimer < 5f) KislotaTimer += Time.deltaTime;

        if (Input.GetButton("Fire2") && KislotaTimer > 5f)
        {
            Kislota.GetComponent<AttackMan>().Killer = this.gameObject;
            KislotaTimer = 0;
            PhotonNetwork.Instantiate(Kislota.name, KislotaPoint.position, skinToFlip.rotation);
        }
    }

    void FixedUpdate(){
        if (!_photonViewMonster.IsMine || isDeath) return;
        Vector3 down = Vector3.Project(rigidbody2D.velocity, transform.up);
        rigidbody2D.velocity = down;
        if (_menu.isPause) return;
        Vector3 hor = transform.right * 10f * SpeedMove * Input.GetAxis("Horizontal");
        rigidbody2D.velocity = hor + down;
    }

    void OnDie()
    {
        Score = 0;
        isDeath = true;
        for (int i = 0; i < _animator.Length; i++) _animator[i].SetBool("isDeath", true);
        Health = 0;
        PanelDeath.SetActive(true);
    }

    void Respawn()
    {
        GameObject objGM = GameObject.Find("GameManager");
        GameManager _gm = objGM.GetComponent<GameManager>();
        int _mapLoad = _gm.MapLoad(PhotonNetwork.CurrentRoom.Name);
        transform.position = new Vector3(_gm.PintsOfSpawn[(_mapLoad*2)+1].position.x + Random.Range(-2, 2), 4, 0);
        PanelDeath.SetActive(false);
        isDeath = false;
        _animator[id_monster_skin].SetBool("isDeath", false);
        Health = 2500;
    }

    public void SetSkinMonster()
    {
        for (int i = 0; i < Skins.Length; i++)
        {
            if(id_monster_skin==i) Skins[i].SetActive(true);
            else Skins[i].SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Granade")
        {
            Health -= other.gameObject.GetComponent<AttackMan>().Damage;
            AttackMan trig = other.gameObject.GetComponent<AttackMan>();
            PlayerControll killer = trig.Killer.GetComponent<PlayerControll>();
            killer.Score += 25;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground" && !isGround || other.gameObject.tag == "platform" && !isGround)
        {
            Jumps = 0;
            isGround = true;
        }

        if (other.gameObject.tag == "platform")
        {
            isFlatformFoot = true;
        }
        if(other.gameObject.tag == "Lava" && !isDeath) OnDie();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "platform")
        {
            Jumps++;
            isGround = false;
        }
        
        if (other.gameObject.tag == "platform")
        {
            isFlatformFoot = false;
        }
    }
}
