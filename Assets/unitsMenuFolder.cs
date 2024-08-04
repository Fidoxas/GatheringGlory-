using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitsMenuFolder : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void ChangeState()
    {
        bool actual = anim.GetBool("Folded");
        
        anim.SetBool("Folded", !actual);
    }
}
