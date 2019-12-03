using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private Enemy enemy;

    public Animator MyAnimator;

    private SpriteRenderer spriteRenderer;

    enum state { Idle, Patrol, Follow, Attack };
    state currentState;
    

    private float idleTimer;
    private float idleDuration;
    private float patrolTimer;
    private float patrolDuration;
    private float attackTimer;
    private float attackCooldown = 2;
    private bool canAttack = true;

    private float direcTimer;
    private float direcDuration = 8; //cannot walk in the same direction for more than 8 seconds

    public GameObject Target { get; set; }

    [SerializeField]
    private float meleeRange;

    private bool immortal = false;
    [SerializeField]
    private float immortalTime;

    public bool MeleeRange
    {
        get
        {
            if(Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }

            return false;
        }
    }

    public override bool isDead
    {
        get
        {
            return health <= 0;
        }
    }
       

    public Transform groundDetection;
    public Transform groundDetectionRear;

    public override void Start()
    {
        MyAnimator = this.gameObject.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);

        currentState = state.Idle;
    }

    void Update()
    {
        if (!isDead)
        {
            LookAtTarget();

        
            if (currentState == state.Idle)
            {
                //Idle
                MyAnimator.SetFloat("Speed", 0);

                idleDuration = UnityEngine.Random.Range(1, 5);

                Debug.Log("Idling...");                

                idleTimer += Time.deltaTime;

                if (idleTimer >= idleDuration)
                {

                    currentState = state.Patrol;
                    idleTimer = 0;
                }
            }
            else if (currentState == state.Patrol)
            {
                //Patrol
                Debug.Log("Patroling...");

                patrolDuration = UnityEngine.Random.Range(1, 10);

                patrolTimer += Time.deltaTime;

                if (patrolTimer >= patrolDuration)
                {
                    currentState = state.Idle;
                    patrolTimer = 0;
                }

                Move();

                if (Target != null)
                {
                    currentState = state.Follow;
                }
            }
            else if (currentState == state.Follow)
            {
                //Follow
                Debug.Log("Following...");
                if (MeleeRange)
                {
                    currentState = state.Attack;
                }
                else if (!MeleeRange)
                {
                    currentState = state.Patrol;
                }
                else
                {
                    currentState = state.Idle;
                }
            }
            else if (currentState == state.Attack)
            {
                //Attack
                Debug.Log("Attacking...");
                MyAnimator.SetFloat("Speed", 0);
                attackTimer += Time.deltaTime;

                if (attackTimer >= attackCooldown)
                {
                    canAttack = true;
                    attackTimer = 0;
                }

                if (canAttack)
                {

                    MyAnimator.SetTrigger("Attack");
                    FindObjectOfType<SoundManager>().Play("Enemy Attack");
                    canAttack = false;
                }
                if (!MeleeRange)
                {
                    currentState = state.Follow;
                }
                else if (Target == null)
                {
                    currentState = state.Idle;
                }
            }
       
            
        }
        
    }
    
    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if( xDir < 0 && faceRight || xDir > 0 && !faceRight)
            {
                ChangeDirection();
            }
        }
    }


    public void RemoveTarget()
    {
        Target = null;
        currentState = state.Patrol;
    }
  

    public void Move()
    {
        if (!Attack)
        {
            MyAnimator.SetFloat("Speed", 1f);

            transform.Translate(GetDir() * (moveSpeed * Time.deltaTime));
        }

        
        direcTimer += Time.deltaTime;

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2f);
        RaycastHit2D groundInfo2 = Physics2D.Raycast(groundDetectionRear.position, Vector2.down, 2f);

        if (groundInfo.collider == false || groundInfo2 == false)
        {
            ChangeDirection();
        }
        if (direcTimer >= direcDuration){
            ChangeDirection();
            direcTimer = 0;
        }

    }

    public Vector2 GetDir()
    {
        return faceRight ? Vector2.right : Vector2.left;

    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;
        if (!immortal)
        {
            if (!isDead)
            {
                MyAnimator.SetTrigger("Damage");
                immortal = true;

                StartCoroutine(IndicateImmortal());

                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                MyAnimator.SetTrigger("Death");
                yield return null;
            }
        }

        
    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }


    public override void Death()
    {
        Destroy(gameObject);
    }
}
