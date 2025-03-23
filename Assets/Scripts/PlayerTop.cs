using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerTop : MonoBehaviour
{
    public List<ScoreScript> players = new List<ScoreScript>();
    public TextMeshProUGUI[] stringsTop;
    void Start()
    {
        for (int i = 0; i < stringsTop.Length; i++)
        {
            stringsTop[i].text = " ";
        }
    }

    private void Update()
    {
        SetTexts(players);
    }

    public void SetTexts(List<ScoreScript> pl)
    {
        ScoreScript[] top = pl
            .OrderByDescending(p => p.Score)
            .Take(5)
            .ToArray();

        for(int i = 0; i < top.Length; i++)
        {
            if (top[i] == null)
            {
                players.RemoveAt(i);
            }

            if (top[i].GetComponent<PlayerControll>() != null)
            {
                PlayerControll man = top[i].GetComponent<PlayerControll>();
                stringsTop[i].color = Color.green;
                stringsTop[i].text = top[i].Score + ": " + man._photonView.Owner.NickName;
            }
            else
            {
                MonsterControll mon = top[i].GetComponent<MonsterControll>();
                stringsTop[i].color = Color.red;
                stringsTop[i].text = top[i].Score + ": " + mon._photonViewMonster.Owner.NickName;
            }

        }
    }

    public void AddPlayerInList(ScoreScript me)
    {
        players.Add(me);
    }
}
