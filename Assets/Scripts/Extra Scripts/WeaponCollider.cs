using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    [SerializeField]
    private List <string> targetTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(targetTag.Contains(collision.tag))
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
