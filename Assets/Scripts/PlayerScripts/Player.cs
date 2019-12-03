using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeadEventHandler();

public class Player : Character
{

    private static Player instance;

    public event DeadEventHandler Dead;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
        
    }
    
    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    
    [SerializeField]
    private bool airControl;
    
    [SerializeField]
    private float jumpForce = 400;    

    public Rigidbody2D MyRB { get; set; }
   
    public bool Jump { get; set; }
    
    public bool OnGround { get; set; }

    private bool immortal = false;
    [SerializeField]
    private float immortalTime;

    private SpriteRenderer spriteRenderer;

    private float livesCounter = 3;

    public override bool isDead
    {
        get
        {
            if(health <= 0)
            {
                onDead();
            }
            
            return health <= 0;
        }
    }

    public override void Start()
    {
        base.Start();
        faceRight = true;
        MyRB = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    void Update()
    {
        if (!TakingDamage && !isDead)
        {

            HandleInput();
        }
    }
    void FixedUpdate()
    {
        if (!TakingDamage && !isDead)
        {
            float horiz = Input.GetAxis("Horizontal");

            OnGround = IsGrounded();

            HandleMovement(horiz);

            Flip(horiz);

            HandleLayers();
        }

    }

    public void onDead()
    {
        if(Dead != null)
        {
            Dead();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MyAnimator.SetTrigger("Jump");
        }
        if (OnGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MyAnimator.SetTrigger("Attack");
                FindObjectOfType<SoundManager>().Play("Standard Attack");
            }
        } else if (!OnGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MyAnimator.SetTrigger("JumpAttack");
                FindObjectOfType<SoundManager>().Play("Dolphin Slash");
                if (OnGround)
                {
                    MyAnimator.ResetTrigger("JumpAttack");
                }
            }
            
        }

        

        if (Input.GetKeyDown(KeyCode.W) && OnGround)
        {
            MyAnimator.SetTrigger("UpAttack");
            FindObjectOfType<SoundManager>().Play("Up Attack");
        }

        if (Input.GetKeyDown(KeyCode.E) && OnGround)
        {
            MyAnimator.SetTrigger("StrongAttack");
            FindObjectOfType<SoundManager>().Play("Strong Attack");
        }
       

    }

    private void HandleMovement(float horizontal)
    {
        
        if(MyRB.velocity.y < 0)
        {
            MyAnimator.SetBool("Landing", true);
            MyAnimator.ResetTrigger("JumpAttack");
            MyAnimator.SetFloat("Speed", 0);
        }
        
        if((!Attack && !UpAttack && !StrongAttack) && (OnGround || airControl))
        {
            MyRB.velocity = new Vector2(horizontal * moveSpeed, MyRB.velocity.y);
        }

        if(Jump && MyRB.velocity.y == 0)
        {
            MyRB.AddForce(new Vector2(0, jumpForce));
        }

       

        MyAnimator.SetFloat("Speed", Mathf.Abs(horizontal));



        
    }

    private void Flip(float horizontal)
    {
        if(horizontal > 0 && !faceRight || horizontal < 0 && faceRight)
        {
            ChangeDirection();
        }
        
        
    }

    

    private bool IsGrounded()
    {
        if (MyRB.velocity.y <= 0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++){
                    if(colliders[i].gameObject != gameObject)
                    {
                       
                        return true;
                    }
                }
            }
        }
        return false;
    }

    

    private void HandleLayers()
    {
        if (!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);

        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);

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


    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            health -= 10;
            HealthBar.health -= 10;
            MyRB.AddForce(new Vector2(1f, 0f));

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
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("Death");
            }
        }        
    }

    public override void Death()
    {
        
        


        MyRB.velocity = Vector2.zero;
        MyAnimator.SetTrigger("Idle");
        health = 30;
        HealthBar.health = 30;
        transform.position = new Vector3(-17.64f, 6.74f, 0);

    }

    
}
