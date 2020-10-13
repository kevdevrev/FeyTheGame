using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimation : MonoBehaviour
{
    protected Animator anim;

    public void StopAnimating()
    {
        anim = GetComponent<Animator>();

        anim.speed = 0;
    }
}
