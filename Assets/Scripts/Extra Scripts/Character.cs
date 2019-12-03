using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Character : MonoBehaviour
{
    
    [SerializeField]
    protected float moveSpeed;

    protected bool faceRight;

    [SerializeField]
    protected int health;

    [SerializeField]
    private EdgeCollider2D[] weaponCollider;

    [SerializeField]
    private List<string> damageSources;

    public abstract bool isDead { get; }

    public bool TakingDamage { get; set; }

    public bool Attack { get; set; }
    public bool UpAttack { get; set; }
    public bool StrongAttack { get; set; }
    public bool JumpAttack { get; set; }
    public Animator MyAnimator { get; private set; }
    public EdgeCollider2D[] WeaponCollider { get => weaponCollider; }

    // Start is called before the first frame update
    public virtual void Start()
    {
        faceRight = true;
        
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract IEnumerator TakeDamage();

    public abstract void Death();

    public void ChangeDirection()
    {
        faceRight = !faceRight;

        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        
    }

    public void MeleeAttack()
    {
        WeaponCollider[0].enabled = true;
    }

    public void UpMeleeAttack()
    {
        WeaponCollider[1].enabled = true;
    }

    public void StrongMeleeAttack()
    {
        WeaponCollider[2].enabled = true;
    }

    public void JumpMeleeAttack()
    {
        WeaponCollider[3].enabled = true;
    }


    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
    
}