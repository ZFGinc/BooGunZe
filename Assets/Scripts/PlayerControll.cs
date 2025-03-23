using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerControll : MonoBehaviour, IPunObservable
{
    public PhotonView _photonView;
    public Transform skinToFlip;
    Rigidbody2D rigidbody2D;
    public Animator _animator;
    public float SpeedMove = 12f;
    public float JumpForce = 25f;
    public GameObject playerCamera;
    public TextMeshPro NickNameText;

    float tStep = 0f;
    public GameObject StepPrefab;
    
    public int idSkin = 0;
    public Sprite[] Heads;
    public Sprite[] Bodys;
    public Sprite[] Arms;
    public Sprite[] Legs;
    public SpriteRenderer[] OneBody;
    
    public bool isFlatformFoot = false;
    public bool isDown = false;
    public bool isGround = false;
    public int Jumps = 0;
    public bool isJump = false;
    public bool isMove = false;

    public GameObject PanelDeath;
    public int Health = 250;
    public TextMeshPro HealthText;
    public bool isDeath = false;
    float Timer =0f;

    public GameObject JumpPart;
    public Transform PointJump;

    MenuController _menu;

    public int Score = 0;


    public void OnPhotonSerializeView(PhotonStream streem, PhotonMessageInfo info){
    	if(streem.IsWriting){
        	streem.SendNext(idSkin);
            streem.SendNext(Health);
            streem.SendNext(Score);
        }
        else{
        	this.idSkin = (int)streem.ReceiveNext();
            this.Health = (int)streem.ReceiveNext();
            this.Score = (int)streem.ReceiveNext();
        }
    }

    private void Start()
    {
        Health = 500;
        GameObject objMenu = GameObject.Find("MainCanvas");
        _menu = objMenu.GetComponent<MenuController>();
        idSkin = PlayerPrefs.GetInt("SkinMan");
        _photonView = GetComponent<PhotonView>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        
        NickNameText.SetText(_photonView.Owner.NickName);
        if (_photonView.IsMine)
        {
            NickNameText.color = Color.yellow;
        }
    }

    void Update()
    {
        SetSkinToPlayer(idSkin);
        HealthText.SetText(Health.ToString());
        
        if (!_photonView.IsMine) return;
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
        if (Score < 0) Score = 0;
        if (isDeath) return;

        if (Input.GetKeyUp(KeyCode.A)) { _animator.SetBool("isRun", false); isMove = false; }
        if (Input.GetKeyUp(KeyCode.D)) { _animator.SetBool("isRun", false); isMove = false; }

        if (_menu.isPause) return;

        playerCamera.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Space)) isJump = true;
        else isJump = false;
        
        if (Input.GetKeyDown(KeyCode.LeftShift)) isDown = true;
        else isDown = false;

        if (isJump && isGround && Jumps==0) Jumping();
        
        if (isFlatformFoot && isDown)
        {
            isFlatformFoot = false;
            Vector2 DownPlatform = new Vector2(transform.position.x, transform.position.y - .85f);
            transform.position = DownPlatform;
        }
	
	    if(tStep<0.3) tStep+=Time.deltaTime;
	    if(isMove && tStep> .3f && isGround)
	    {
	        tStep=0f;
            AudioSource _step = StepPrefab.GetComponent<AudioSource>();
            _step.pitch = Random.Range(0.7f, 0.9f);
            PhotonNetwork.Instantiate(StepPrefab.name, PointJump.position, Quaternion.identity);

        }

        if (Input.GetKey(KeyCode.A))
        {
	    isMove = true;
            _animator.SetBool("isRun", true);
            skinToFlip.rotation = new Quaternion(0,180,0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
	    isMove = true;
            _animator.SetBool("isRun", true);
            skinToFlip.rotation = new Quaternion(0,0,0, 0);
        }
    }
    
    void OnDie()
    {
        Score = 0;
        isDeath = true;
        _animator.SetBool("isDeath", true);
        Health = 0;
        PanelDeath.SetActive(true);
    }

    void Respawn()
    {
        GameObject objGM = GameObject.Find("GameManager");
        GameManager _gm = objGM.GetComponent<GameManager>();
        int _mapLoad = _gm.MapLoad(PhotonNetwork.CurrentRoom.Name);
        transform.position = new Vector3(_gm.PintsOfSpawn[_mapLoad*2].position.x + Random.Range(-2, 2), 0, 0);
        PanelDeath.SetActive(false);
        isDeath = false;
        _animator.SetBool("isDeath", false);
        Health = 500;
    }

    void FixedUpdate(){
    	if (!_photonView.IsMine || isDeath) return;
        Vector3 _down = Vector3.Project(rigidbody2D.velocity, transform.up);
        rigidbody2D.velocity = _down;
        if (_menu.isPause) return;
        Vector3 _hor = transform.right * 10f * SpeedMove * Input.GetAxis("Horizontal");
        rigidbody2D.velocity = _down + _hor;
    }
    private void SetSkinToPlayer(int id)
    {
        OneBody[0].sprite = Heads[id];
        OneBody[1].sprite = Bodys[id];
        OneBody[2].sprite = Arms[id];
        OneBody[3].sprite = Arms[id];
        OneBody[4].sprite = Legs[id];
        OneBody[5].sprite = Legs[id];
    }

    void Jumping()
    {
        PhotonNetwork.Instantiate(JumpPart.name, PointJump.position, Quaternion.identity);
        rigidbody2D.AddForce(Vector2.up * JumpForce * 500f);
        isGround = false;
        isJump = false;
        Jumps++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "MonsterAttack") 
        {
            Health -= other.gameObject.GetComponent<AttackMan>().Damage;
            AttackMan trig = other.gameObject.GetComponent<AttackMan>();
            MonsterControll killer = trig.Killer.GetComponent<MonsterControll>();
            killer.Score += 5;
        }
        if (other.gameObject.tag == "kislota")
        {
            Health -= other.gameObject.GetComponent<AttackMan>().Damage;
            AttackMan trig = other.gameObject.GetComponent<AttackMan>();
            MonsterControll killer = trig.Killer.GetComponent<MonsterControll>();
            killer.Score += 50;
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground" && !isGround || other.gameObject.tag == "platform" && !isGround)
        {
            Jumps=0;
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
