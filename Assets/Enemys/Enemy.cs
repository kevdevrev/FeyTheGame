using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Material idleMaterial;
    public Renderer materialReference;


    public void ToggleCooldown()
    {
        this.StartCoroutine(ResetAttackCooldown());
    }

    //protected gives access to children
    [SerializeField]
    protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected float waypointSpeed;
    [SerializeField]
    protected Transform pointA, pointB;


    [SerializeField] protected Vector2 detectionRadius;
    protected bool attackOnCooldown = false;
    protected float idleTimeCondition = 3f;
    protected Vector3 destination;
    [SerializeField]protected Vector2 feyDistanceAwayVector;
    
    [SerializeField]
    protected float enemyAttackRange;
    protected Animator anim;
    protected SpriteRenderer sprite;

    protected bool inCombat = false;
    public Transform feyLocation;
    
    protected Rigidbody2D rigid;
    [SerializeField]
    protected float attackCooldownTimer = 1f;

    [SerializeField] protected Transform castPoint;
    [SerializeField] protected float aggroTimeLimit;
    protected float aggroTimer;
    [SerializeField] protected bool isAggrod;
    protected bool isFacingLeft = false;
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
        if (sprite.transform.localRotation == Quaternion.Euler(0, 0, 0))
        {
            isFacingLeft = false;
        }
        else
        {
            isFacingLeft = true;
        }

        aggroTimer = aggroTimeLimit;
    }
    
    public virtual Vector3 WayPointLogic(Vector3 goal)
    {
        //way point code
        // && !anim.GetBool("InCombat") && !anim.GetBool("Chase")
        if (Vector3.Distance(transform.position,pointA.position) <= 0.2)
        {
            
            destination = new Vector3(pointB.position.x, transform.position.y, transform.position.z);
            //anim.Play("Idle");
            if (idleTimeCondition < 0)
            {
                anim.SetTrigger("Idle");
                idleTimeCondition = 3f;
            }
            sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
            isFacingLeft = false;
            //anim.SetTrigger("Move");
        }
        //&& !anim.GetBool("InCombat") && !anim.GetBool("Chase")
        else if (Vector3.Distance(transform.position,pointB.position) <= 0)
        {

            destination = new Vector3(pointA.position.x, transform.position.y, transform.position.z);
            
            sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
            isFacingLeft = true;
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
        //materialReference.material = idleMaterial;
        //check to see if Fey is far away enough to justify walking
        feyDistanceAwayVector = feyLocation.position - transform.position;
        //WayPointLogic();
        if (!isAggrod && (!CanSeePlayer() || (Mathf.Abs(feyDistanceAwayVector.x) > detectionRadius.x
                             || Mathf.Abs(feyDistanceAwayVector.y) > detectionRadius.y)))
        {
            //Debug.Log("WayPointMode");
            inCombat = false;
            anim.SetBool("InCombat", false);
            anim.SetTrigger("Move");
            anim.SetBool("WayPointMode", true);
            destination = WayPointLogic(destination);
        }
        else
        {
            
            isAggrod = true;
            aggroTimer -= Time.deltaTime;
            if (aggroTimer < 0)
            {
                aggroTimer = aggroTimeLimit;
                isAggrod = false;
            }

            if (CanSeePlayer())
            {
                anim.SetBool("WayPointMode", false);
                if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    if (feyDistanceAwayVector.x > 0)
                    {
                        sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        isFacingLeft = false;
                    }
                    else if (feyDistanceAwayVector.x < 0)
                    {
                        sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        isFacingLeft = true;
                    }
                }

                //handle chasing to get into hit distance
                //(Mathf.Abs(feyDistanceAwayVector.x) > enemyAttackRange);
                if (Mathf.Abs(feyDistanceAwayVector.x) < detectionRadius.x
                    && Mathf.Abs(feyDistanceAwayVector.y) < detectionRadius.y
                    && Mathf.Abs(feyDistanceAwayVector.x) > enemyAttackRange)
                {
                  //  Debug.Log("In Chase Mode");
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
                }
                else if (Mathf.Abs(feyDistanceAwayVector.x) < enemyAttackRange)
                {
                    anim.SetBool("Chase", false);
                    anim.SetBool("InCombat", true);

                    anim.SetTrigger("Move");
                    

                    if (!attackOnCooldown)
                        Attack();
                }
                else
                {
                    if (destination == pointA.position)
                    {
                        if (sprite.transform.localRotation != Quaternion.Euler(0, 180, 0))
                        {
                            sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            isFacingLeft = true;
                        }
                    }
                    else if (destination == pointB.position)
                    {
                        if (sprite.transform.localRotation != Quaternion.Euler(0, 0, 0))
                        {
                            sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            isFacingLeft = false;
                        }

                    }
                }
            }
        }

    }

    protected void EnemyFaceDirection()
    {
        if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            float facingDirection = feyLocation.position.x - transform.position.x;
            if (facingDirection > 0)
            {
                sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                isFacingLeft = false;
            }
            else if (facingDirection < 0)
            {
                sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                isFacingLeft = true;
            }
        }
    }

    //virtual keyword lets us overwrite this.
    protected virtual void Attack()
    {
        //Debug.Log("Hitting");
        anim.SetTrigger("AttackTrigger");
        StartCoroutine(ResetAttackCooldown());
    }

    protected virtual void Update()
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
        yield return new WaitForSeconds(attackCooldownTimer);
        attackOnCooldown = false;

    }
    public bool CanSeePlayer()
    {
        float direction;
        if (isFacingLeft)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        //Vector2 endPos = castPoint.position + direction * (Vector3.right * aggroDistance);
        //RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << 9)
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, feyLocation.position, (1 << LayerMask.NameToLayer("Fey")) | (1 << LayerMask.NameToLayer("Floor")));
        
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Fey"))
            {
                Debug.DrawLine(castPoint.position,hit.point, Color.yellow);
                return true;
            }
            else
            {
                Debug.DrawLine(castPoint.position,hit.point, Color.red);
                return false;
            }
        }
        else
        {
            Debug.DrawLine(castPoint.position, feyLocation.position, Color.blue);
        }
        
        return false;
    }
}
