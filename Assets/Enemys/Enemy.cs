using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //protected gives access to children
    [SerializeField]
    protected int health;
    [SerializeField]
    protected int speed;
    [SerializeField]
    protected Transform pointA, pointB;

    [SerializeField]
    protected float enemyAttackRange;
    protected Vector3 destination;
    protected Animator anim;
    protected SpriteRenderer sprite;

    protected bool inCombat = false;
    protected Transform feyLocation;
    
    protected Rigidbody2D rigid;

    [SerializeField]
    protected float attackCooldown;

    protected bool disabled;
    private void Start()
    {
        //we put our starting calls into Init so we can override it in our children.
        Init();
    }

    public virtual void Init()
    {
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        feyLocation = GameObject.FindWithTag("Player").GetComponent<Transform>();
        rigid = GetComponent<Rigidbody2D>();
        disabled = false;
    }
    
    public virtual void WayPointLogic()
    {
        //Debug.Log("In waypoint logic");
        if (transform.position == pointA.position)
        {
            sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if(transform.position == pointB.position)
        {
            sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);

        }
        //way point code
        if (transform.position == pointA.position && !anim.GetBool("InCombat") && !anim.GetBool("Chase"))
        {
            destination = pointB.position;
            anim.SetTrigger("Idle");

        }
        else if (transform.position == pointB.position && !anim.GetBool("InCombat") && !anim.GetBool("Chase"))
        {
            destination = pointA.position;
            anim.SetTrigger("Idle");

        }

        if (inCombat == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
        
        //check to see if Fey is far away enough to justify walking
        float feysDistanceAway = Vector3.Distance(feyLocation.position, transform.position);
        if (feysDistanceAway > 5f)
        {
            inCombat = false;
            anim.SetBool("InCombat", false);
        }else if (feysDistanceAway < 5f && feysDistanceAway > enemyAttackRange)
        {
            anim.SetBool("Chase", true);
            inCombat = true;
            //tells it to hit
            anim.SetBool("InCombat", false);
            //Debug.Log("Fey is nearby lets chase her");
            if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                Vector3 feyCurLocation = new Vector3(feyLocation.position.x, transform.position.y, transform.position.z);
                transform.position =
                    Vector3.MoveTowards(transform.position, feyCurLocation, speed * Time.deltaTime);
            }
        }else if (feysDistanceAway < enemyAttackRange)
        {
            anim.SetBool("Chase", false);
            anim.SetBool("InCombat", true);
            }

        if (anim.GetBool("Chase") || anim.GetBool("InCombat"))
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
        
    }

    public virtual void Update()
    {
        Debug.Log("Am I disabled?" + disabled);
        //if idle, we want to prevent movement, so we do nothing, so just return
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                return;
            }
            WayPointLogic();
        
    }
    
}
