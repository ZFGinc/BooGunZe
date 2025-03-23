using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToAnimation : MonoBehaviour
{
    public Animator _animator;
    public string _valueName;

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Monster") _animator.SetBool(_valueName, true);
    }
}
