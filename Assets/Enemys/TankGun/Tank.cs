using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Tank : MonoBehaviour, IDamage
{
    [SerializeField] protected Material idleMaterial;
    [SerializeField] protected Material moveMaterial;
    [SerializeField] protected Material hitMaterial;
    [SerializeField] protected Material liberatedMaterial;
    private Light2D _tankLight;
    public SpriteRenderer tank_sprite;
    public Renderer materialReference;
    
    
    
    
    [SerializeField] public int bulletDamage;
    [SerializeField] public int bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootCooldown = 1f;
    private float shootCooldownTimer = 1f;
    private bool notOnCoolDown = true;
    private int counter;
    private float shootDelayTimer = 0.3f;
    [SerializeField] public int bulletCount = 5;

    // Start is called before the first frame update

    //protected gives access to children
    [SerializeField]
    protected int health;
    [SerializeField] protected float speed;

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

    
    public void ToggleCooldown()
    {
        this.StartCoroutine(ResetAttackCooldown());
    }
    private void Start()
    {
        //we put our starting calls into Init so we can override it in our children.
        Init();
        Health = health;
        tank_sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //Get the animation script handler
        _tankLight = transform.GetChild(0).transform.GetChild(1).GetComponent<Light2D>();
        materialReference = transform.GetChild(0).GetComponent<Renderer>();
    }

    public virtual void Init()
    {
        feyLocation = GameObject.FindWithTag("Fey").GetComponent<Transform>();
        destination = new Vector2(feyLocation.position.x, transform.position.y);
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
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
    
    
    
    

    private void MovementLogic()
    {
        feyDistanceAwayVector = feyLocation.position - transform.position;
        materialReference.material = idleMaterial;
        isAggrod = true;
        aggroTimer -= Time.deltaTime;
        if (aggroTimer < 0)
        {
            aggroTimer = aggroTimeLimit;
            isAggrod = false;
        }

        if (CanSeePlayer())
        {
            /*if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
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
            }*/

            //handle chasing to get into hit distance
            if (Mathf.Abs(feyDistanceAwayVector.x) < detectionRadius.x
                && Mathf.Abs(feyDistanceAwayVector.y) < detectionRadius.y
                && Mathf.Abs(feyDistanceAwayVector.x) > enemyAttackRange)
            {
                Debug.Log("In Chase Mode");
                anim.SetBool("Chase", true);
                inCombat = true;
                //tells it to hit
                anim.SetBool("InCombat", false);
                //Debug.Log("Fey is nearby lets chase her");
                if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    Vector2 targetLocation;
                    // have the buddy go down so he can shoot
                    //determine if fey is ahead or behind the buddy
                    // go to fey's level but keep distance for shooting
                    float dotRes = Vector3.Dot(transform.forward, feyLocation.forward);
                    if (dotRes >= 0)
                    {
                        targetLocation = new Vector2(feyLocation.position.x + enemyAttackRange, transform.position.y);
                    }
                    else
                    {
                        targetLocation = new Vector2(feyLocation.position.x - enemyAttackRange, transform.position.y);
                    }

                    transform.position =
                        Vector2.MoveTowards(transform.position, targetLocation, speed * Time.deltaTime);
                }
            }
            materialReference.material = moveMaterial;
        }
        else if (Mathf.Abs(feyDistanceAwayVector.x) < enemyAttackRange)
        {
            transform.GetComponent<Renderer>().material = idleMaterial;
            anim.SetBool("Chase", false);
            anim.SetBool("InCombat", true);

            anim.SetTrigger("Move");


            if (!attackOnCooldown)
                Debug.Log("In attack mode");
        }
    }
    
    protected virtual void Update()
    {

        
        _tankLight.lightCookieSprite = tank_sprite.sprite;

        /*if((this.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("IdleCD"))
        {
            transform.GetComponent<Renderer>().material = idleMaterial;
        }else if((this.anim.GetCurrentAnimatorStateInfo(0).IsName("Move"))
        {
            transform.GetComponent<Renderer>().material = idleMaterial;
        }*/
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
    
    public int Health { get; set; }
    public void Damage(int dmgTaken)
    {
        materialReference.material = hitMaterial;
        Health = Health - dmgTaken;
        Debug.Log(Health);
        anim.SetTrigger("Hit");
        //rigid.AddForce(new Vector2(15f + rigid.mass, 15f + rigid.mass), ForceMode2D.Impulse);
        inCombat = true;
        anim.SetBool("InCombat", true);
        if(Health<1)
        {
            anim.SetBool("Disabled",true);
            disabled = true;
            if (disabled)
            {
                materialReference.material = liberatedMaterial;
            }

        }    
    }
}
