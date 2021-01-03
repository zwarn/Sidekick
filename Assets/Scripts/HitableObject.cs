using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class HitableObject : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private int hits = 3;

    public void Hit()
    {
        hits -= 1;
        if (hits < 1)
        {
            Destroy();
        }
        else
        {
            Animator.SetTrigger("hit");
        }
    }
    
    
    public void Destroy()
    {
        var collider = GetComponent<Collider2D>();
        collider.enabled = false;
        Animator.SetTrigger("destroy");
        GetComponent<SpriteRenderer>().sortingLayerName = "LayingOnGround";
    }
}
