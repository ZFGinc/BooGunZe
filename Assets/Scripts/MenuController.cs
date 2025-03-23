using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public LobbyManager _lobby;
    public bool isMenu = true;
    public bool isPause = false;
    public bool isGame = false;
    [Space]
    [Header("Переменные классов и скинов")]
    [Range(0,1)] public int isClass;
    [Range(0, 5)] public int isSkinMan;
    [Range(0, 2)] public int isSkinMonster;

    [Header("Объекты взяаимодействия")]
    public GameObject[] TextsClasses;
    public GameObject[] ClassPlayer;
    public GameObject[] SkinMan;
    public GameObject[] SkinMonster;
    public GameObject[] MapLoadImg;

    [Header("Music and Effects")]
    [Range(-80, 0)] public float musicVolume;
    [Range(-80, 0)] public float effectVolume;
    [Space]
    public Slider musicSlider;
    public Slider effectSlider;
    [Space]
    public AudioMixerGroup music;
    public AudioMixerGroup effects;
    [Space]
    public GameObject PausePanel;
    public GameObject SettingsPanel;
    [Space]
    public GameObject[] FonsMainMenu;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume")) musicVolume = PlayerPrefs.GetFloat("musicVolume");
        else musicVolume = 0f;

        if (PlayerPrefs.HasKey("effectVolume")) effectVolume = PlayerPrefs.GetFloat("effectVolume");
        else effectVolume = 0f;

        musicSlider.value = musicVolume;
        effectSlider.value = effectVolume;

        int RandFon = Random.Range(0, 6);
        if (isMenu) FonsMainMenu[RandFon].SetActive(true);

        if (PlayerPrefs.HasKey("ClassSelect")) isClass = PlayerPrefs.GetInt("ClassSelect");
        if (PlayerPrefs.HasKey("SkinMan")) isSkinMan = PlayerPrefs.GetInt("SkinMan");
        if (PlayerPrefs.HasKey("SkinMonster")) isSkinMonster = PlayerPrefs.GetInt("SkinMonster");
    }

    void Update()
    {
        musicVolume = musicSlider.value;
        effectVolume = effectSlider.value;

        music.audioMixer.SetFloat("musicVolume", musicVolume);
        effects.audioMixer.SetFloat("effectVolume", effectVolume);

        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("effectVolume", effectVolume);

        if (isGame) 
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isPause)
            {
                isPause = true;
                PausePanel.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isPause)
            {
                isPause = false;
                PausePanel.SetActive(false);
                SettingsPanel.SetActive(false);
            }
        }

        if (!isMenu) return;

        PlayerPrefs.SetInt("ClassSelect", isClass);
        PlayerPrefs.SetInt("SkinMan", isSkinMan);
        PlayerPrefs.SetInt("SkinMonster", isSkinMonster);

        for (int i =0; i<ClassPlayer.Length; i++)
        {
            if (isClass == i)
            {
                ClassPlayer[i].SetActive(true);
                TextsClasses[i].SetActive(true);
            }
            else 
            {
                TextsClasses[i].SetActive(false);
                ClassPlayer[i].SetActive(false); 
            }
                
        }

        for (int i = 0; i < SkinMan.Length; i++)
        {
            if (isSkinMan == i) SkinMan[i].SetActive(true);
            else SkinMan[i].SetActive(false);
        }

        for (int i = 0; i < MapLoadImg.Length; i++)
        {
            if (_lobby.MapValue == i) MapLoadImg[i].SetActive(true);
            else MapLoadImg[i].SetActive(false);
        }

        for (int i = 0; i < SkinMonster.Length; i++)
        {
            if (isSkinMonster == i) SkinMonster[i].SetActive(true);
            else SkinMonster[i].SetActive(false);
        }
    }

    public void ArrowButton(bool isRight)
    {
        if(isClass == 0)
        {
            if (isRight && isSkinMan < 5) isSkinMan++;
            else if (isRight && isSkinMan == 5) isSkinMan=0;

            if (!isRight && isSkinMan > 0) isSkinMan--;
            else if (!isRight && isSkinMan == 0) isSkinMan = 5;
        }

        if (isClass == 1)
        {
            if (isRight && isSkinMonster < 2) isSkinMonster++;
            else if (isRight && isSkinMonster == 2) isSkinMonster = 0;

            if (!isRight && isSkinMonster > 0) isSkinMonster--;
            else if (!isRight && isSkinMonster == 0) isSkinMonster = 2;
        }
    }

    public void ArrowButtonMap(bool isRight)
    {
        if (isRight && _lobby.MapValue < 6) _lobby.MapValue++;
        else if (isRight && _lobby.MapValue == 6) _lobby.MapValue = 0;

        if (!isRight && _lobby.MapValue > 0) _lobby.MapValue--;
        else if (!isRight && _lobby.MapValue == 0) _lobby.MapValue = 6;
    }

    public void RandomMapLoad()
    {
        _lobby.MapValue = Random.Range(0,6);
    }

    public void SelectClass()
    {
        if (isClass == 0) isClass = 1;
        else isClass = 0;
    }

    public void PauseButton(bool value)
    {
        isPause = value;
    }
}
