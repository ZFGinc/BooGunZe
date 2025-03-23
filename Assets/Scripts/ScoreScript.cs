using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public bool Monster = false;
    public MonsterControll _monster;
    public PlayerControll _player;
    public int Score;

    private void Start()
    {
        FindObjectOfType<PlayerTop>().AddPlayerInList(this);
    }
    void Update()
    {
        if (Monster) Score = _monster.Score;
        else Score = _player.Score;
    }
}
