using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Running : MonoBehaviour
{
    
    // Start is called before the first frame update
    public Animator animator;
    public float hSpeed;
    public float vSpeed;
    
    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
      //  hSpeed = Mathf.Abs(Input.GetAxis("Horizontal"));
        // this test helps for numerical instability--sometimes a float is 0, but won't
        // be 0 (especially on an input device) and instead will be super close to zero
     //   if (hSpeed > .0001f)
      //      animator.SetFloat("Speed", hSpeed);
      //  else
       //     animator.SetFloat("Speed", 0.0f);

     
    }
}
