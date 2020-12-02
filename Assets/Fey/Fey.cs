using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Fey : MonoBehaviour, IDamage
{
    //get Fey's Rigid Body
    private Rigidbody2D _fey_rigid;

    //get Fey's Sprite
    public SpriteRenderer _fey_sprite;

    //get Fey's animation script
    private Fey_Animation _fey_animation;

    //get fey's hitbox manager NO LONGER USED
    //private Fey_HitBoxManager _feyHitBoxManager;
    [SerializeField] public float fey_speed;

    [SerializeField] private float jumpForce = 5.0f;

    //lets our logic know if Fey is able to jump or if it is on cooldown.
    private bool jumpCooldown = false;

    //used to handle condition checks if fey is on the ground (jump, spin, etc)
    private bool touchingGround = true;

    //handles Fey's custom sprite lighting
    private Light2D _feyLight;
    [SerializeField] private float punchForce = 5;

    protected Animator anim;

    // Level on death
    [SerializeField] private string level;
    // colliders for button activation
    //List<Collider2D> inColliders = new List<Collider2D>();


    // Start is called before the first frame update
    void Start()
    {
        //assign Fey's rigid Body
        _fey_rigid = GetComponent<Rigidbody2D>();
        //assign Fey's Sprite
        _fey_sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //Get the animation script handler
        _fey_animation = GetComponent<Fey_Animation>();
        //get fey hitbox manager No Longer Used
        //_feyHitBoxManager = transform.GetChild(0).GetComponent<Fey_HitBoxManager>();
        //get fey light
        _feyLight = transform.GetChild(0).transform.GetChild(2).GetComponent<Light2D>();

        anim = GetComponentInChildren<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (Input.GetMouseButtonDown(0) && OnGround() == true)
        {
            _fey_animation.animateAttack();
        }

        {

        }

        // modified engine code, for this to work you need the modified Light2D.cs script!!!
        // Message Kevin for it!!!
        _feyLight.lightCookieSprite = _fey_sprite.sprite;

        // keypress E to interact with buttons
        //if (Input.GetKeyDown((KeyCode.E)))
        //inColliders.ForEach(n => n.SendMessage("Use", SendMessageOptions.DontRequireReceiver));
    }

    //TODO Implement contracted functions
    public int Health { get; set; }

    public void Damage(int dmgTaken)
    {
        Health = Health - dmgTaken;
        //toggle injured animation
        anim.SetTrigger("Hit");
        _fey_rigid.AddForce(new Vector2(punchForce, punchForce), ForceMode2D.Impulse);

        if (Health < 1)
        {
            //SceneManager.LoadScene(level);
            //TODO we don't want to destroy, this is temporary
            //anim.SetBool("Disabled", true);

        }
    }

    private void FlipFey(float horizontalInput)
    {
        if (horizontalInput > 0)
        {
            //_fey_sprite.flipX = false;
            _fey_sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontalInput < 0)
        {
            //_fey_sprite.flipX = true;
            _fey_sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Movement()
    {

        touchingGround = OnGround();
        //get a/d or joystick horizontal values (0.0f to 1.0f)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        //Change Fey's movement Velocity, we don't want to change y.
        FlipFey(horizontalInput);
        Jump();
        _fey_rigid.velocity = new Vector2(horizontalInput * fey_speed, _fey_rigid.velocity.y);

    }

    private void Jump()
    {
        if (Input.GetKeyDown((KeyCode.Space)) && OnGround() == true)
        {
            _fey_rigid.velocity = new Vector2(_fey_rigid.velocity.x, jumpForce);
            StartCoroutine(ResetJumpRoutine());
            _fey_animation.animateJump(true);
        }
    }

    //check if we are actually on the ground before we try to jump.
    //TODO Add condition if on top of enemy
    bool OnGround()
    {
        //raycasts on layer 8 (floor) 1<<8
        var currPos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(currPos, Vector2.down, 0.9f, 1 << 8);
        RaycastHit2D hitEnemy = Physics2D.Raycast(currPos, Vector2.down, 0.9f, 1 << 13);

        // draws the ray in our scene view
        // Debug.DrawRay(transform.position, Vector2.down, Color.red);
        //This means we hit something, which can only be our layer 8 (floor)
        if (hit.collider != null || hitEnemy.collider != null)
        {
            if (jumpCooldown == false)
            {
                //tell animator to jump
                _fey_animation.animateJump(false);
                return true;
            }
        }

        return false;
    }

    //handles our jump cooldown
    IEnumerator ResetJumpRoutine()
    {
        jumpCooldown = true;
        yield return new WaitForSeconds(0.1f);
        jumpCooldown = false;

    }

}

