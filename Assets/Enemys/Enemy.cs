using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    

    public void ToggleCooldown()
    {
        this.StartCoroutine(ResetAttackCooldown());
    }

    //protected gives access to children
    [SerializeField]
    protected int health;
    [SerializeField] protected int speed;
    [SerializeField] protected int waypointSpeed;
    [SerializeField]
    protected Transform pointA, pointB;

    [SerializeField] public float maxXDistanceAway = 5f;
    [SerializeField] public float maxYDistanceAway = 1f;
    [SerializeField] private Vector2 detectionRadius;
    protected bool attackOnCooldown = false;
    private float idleTimeCondition = 3f;
    [SerializeField] public float yDistanceCondition = 3f;
    private Vector3 destination;
    [SerializeField] private Vector2 feyDistanceAwayVector;
    
    [SerializeField]
    protected float enemyAttackRange;
    protected Animator anim;
    protected SpriteRenderer sprite;

    protected bool inCombat = false;
    protected Transform feyLocation;
    
    protected Rigidbody2D rigid;
    [SerializeField]
    protected float attackCooldownTimer = 1f;

    protected bool disabled;
    private void Start()
    {
        //we put our starting calls into Init so we can override it in our children.
        Init();
    }

    public virtual void Init()
    {
        destination = pointA.position;
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        feyLocation = GameObject.FindWithTag("Fey").GetComponent<Transform>();
        rigid = GetComponent<Rigidbody2D>();
        disabled = false;
    }
    
    public virtual Vector3 WayPointLogic(Vector3 goal)
    {
        //way point code
        // && !anim.GetBool("InCombat") && !anim.GetBool("Chase")
        if (Vector3.Distance(transform.position,pointA.position) <= 0.2)
        {
            Debug.Log("Heading to + " + pointB.position);
            
            destination = pointB.position;
            //anim.Play("Idle");
            if (idleTimeCondition < 0)
            {
                anim.SetTrigger("Idle");
                idleTimeCondition = 3f;
            }
            sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //anim.SetTrigger("Move");
        }
        //&& !anim.GetBool("InCombat") && !anim.GetBool("Chase")
        else if (Vector3.Distance(transform.position,pointB.position) <= 0)
        {
            Debug.Log("Heading to + " + pointA.position);

            destination = pointA.position;
            
            sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
            if (idleTimeCondition < 0)
            {
                anim.SetTrigger("Idle");
                idleTimeCondition = 3f;
            }

        }

        if (inCombat == false && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, waypointSpeed * Time.deltaTime);
            
        }
        idleTimeCondition -= Time.deltaTime;

        return destination;
    }
    
    private void MovementLogic()
    {
        //check to see if Fey is far away enough to justify walking
        feyDistanceAwayVector = feyLocation.position - transform.position;
        //WayPointLogic();
        //feyDistanceVector = feyDistanceVector * new Vector2(1f, 0.5f);
        //feyDistanceVector.x > maxXDistanceAway && feyDistanceVector.y < maxYDistanceAway)
        Debug.Log(Mathf.Abs(feyDistanceAwayVector.x) > detectionRadius.x);
        if ((Mathf.Abs(feyDistanceAwayVector.x) > detectionRadius.x
            || Mathf.Abs(feyDistanceAwayVector.y) > detectionRadius.y)
            || (feyDistanceAwayVector.y > yDistanceCondition
            && Mathf.Abs(feyDistanceAwayVector.y) > detectionRadius.y)
            )
        {
            Debug.Log("WayPointMode");
            inCombat = false;
            anim.SetBool("InCombat", false);
            anim.SetTrigger("Move");
            anim.SetBool("WayPointMode", true);
            destination = WayPointLogic(destination);
        }
        else
        {
            Debug.Log("IN ELSE");
            anim.SetBool("WayPointMode", false);
            if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if (feyDistanceAwayVector.x > 0)
                {
                    sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (feyDistanceAwayVector.x < 0)
                {
                    sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }
            //handle being inside attack range
            /*if (Mathf.Abs(feyDistanceAwayVector.x) < enemyAttackRange
                     && feyDistanceAwayVector.y < yDistanceCondition)*/
            //handle chasing to get into hit distance
            Debug.Log(Mathf.Abs(feyDistanceAwayVector.x) > enemyAttackRange);
            if (Mathf.Abs(feyDistanceAwayVector.x) < detectionRadius.x
                && Mathf.Abs(feyDistanceAwayVector.y) < detectionRadius.y
                && Mathf.Abs(feyDistanceAwayVector.x) > enemyAttackRange)
            {
                Debug.Log("In Chase Mode");
                /*else if (xDistanceAway < maxXDistanceAway && xDistanceAway > enemyAttackRange &&
                         yDistanceAway < maxYDistanceAway)
                {*/
                anim.SetBool("Chase", true);
                inCombat = true;
                //tells it to hit
                anim.SetBool("InCombat", false);
                //Debug.Log("Fey is nearby lets chase her");
                if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    Vector2 targetLocation = new Vector2(feyLocation.position.x, transform.position.y);
                    transform.position =
                        Vector2.MoveTowards(transform.position, targetLocation, speed * Time.deltaTime);
                }
            }else if (Mathf.Abs(feyDistanceAwayVector.x) < enemyAttackRange)
            {
                Debug.Log("In Attack Mode");
                anim.SetBool("Chase", false);
                anim.SetBool("InCombat", true);

                anim.SetTrigger("Move");

                Debug.Log("Attack on cooldown: " + attackOnCooldown);

                if (!attackOnCooldown)
                    Attack();
            }
            else
            {
                if (destination == pointA.position)
                {
                    if(sprite.transform.localRotation != Quaternion.Euler(0, 180, 0))
                        sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else if (destination == pointB.position)
                {
                    if(sprite.transform.localRotation != Quaternion.Euler(0, 0, 0))
                        sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);

                }   
            }
        }

    }

    private void EnemyFaceDirection()
    {
        if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            float facingDirection = feyLocation.position.x - transform.position.x;
            if (facingDirection > 0)
            {
                sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (facingDirection < 0)
            {
                sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    //virtual keyword lets us overwrite this.
    public virtual void Attack()
    {
        Debug.Log("Hitting");
        anim.SetTrigger("AttackTrigger");
        StartCoroutine(ResetAttackCooldown());
    }

    public virtual void Update()
    {
        if (!disabled)
        {
            //if idle, we want to prevent movement, so we do nothing, so just return
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                return;
            }

            MovementLogic();
        }
    }
    IEnumerator ResetAttackCooldown()
    {
        attackOnCooldown = true;
        //anim.ResetTrigger("AttackTrigger");
        Debug.Log("In coruten");
        yield return new WaitForSeconds(attackCooldownTimer);
        attackOnCooldown = false;

    }
}
