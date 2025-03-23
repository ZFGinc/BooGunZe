using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMetods : MonoBehaviour
{
    public Animator _animator;

    public void EndAnim(string value)
    {
        _animator.SetBool(value, false);
    }
}
