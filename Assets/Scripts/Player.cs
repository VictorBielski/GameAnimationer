using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Speed på playermodels fart
    [SerializeField] float runSpeed = 7f;
    //Speed på dash/slider
    [SerializeField] float dashSpeed = 0.8f;
    // timer til dash/slider
    [SerializeField] float slideTimer = 0f;
    // max timer til dash/slider
    [SerializeField] float slideTimerMax = 0f;

    Rigidbody2D myRigidbody;
    Animator myAnimator;
    Collider2D myCollider2D;

    // bool til dash/sliding
    bool sliding = false;


    //bool og variable der tjekker, om playermodellen rør "jorden"
    bool isGrounded;
    public Transform groundCheck;

    //variabler til attack
    public float attackTime;
    public float startTimeAttack;

    public Transform attackLocation;
    public float attackRange;
    public LayerMask enemies;


    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpForce;

    // Extra jumps
    private int extraJumps;
    public int extraJumpsValue;


    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        flipSprite();
        Jump();
        dashSlide();
        attackOne();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHoriSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHoriSpeed);
    }

    private void Jump()
    {
        bool playerIsJumping = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        // if grounded = no jump
        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
            myAnimator.SetBool("isJumping", false);
        }

        // double jumping
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            myAnimator.SetBool("isJumping", playerIsJumping);
            myRigidbody.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        // jump-smash / jump-slide
        else if (Input.GetKeyDown(KeyCode.F) && isGrounded == false)
        {
            runSpeed = 20f;
            myAnimator.SetBool("JumpSmash", true);

        } else if (isGrounded == true)
        {
            runSpeed = 7f;
            myAnimator.SetBool("JumpSmash", false);
        }
    }

    // coin counter
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Coins"))
        {
            Destroy(other.gameObject);
        }
    }

    // flipping playermodel sprite in the direction you are moving
    private void flipSprite()
    {
        bool playerHasHoriSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if(playerHasHoriSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    // slide/dash method
    private void dashSlide()
    {
        // "Slide" is a self made "button" with crossplatforminputmanager
        if (Input.GetButtonDown("Slide") && !sliding)
        {
            // starting slide and timer
            slideTimer = 0f;
            myAnimator.SetBool("isSliding", true);
            sliding = true;
        }

        if(sliding)
         {
           // setting slideTimer to the complete time in seconds since last frame
           slideTimer += Time.deltaTime;

            // setting sliding false if slideTimer is more than slideTimerMax
            if (slideTimer > slideTimerMax)
                {
                    sliding = false;
                    myAnimator.SetBool("isSliding", false);
                }
            }
        }

    // attack method
    private void attackOne()
    {
        if (attackTime <= 0)
        {
            if (Input.GetButton("Fire1"))
            {
                myAnimator.SetBool("isAttackingOne", true);
                Collider2D[] damage = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, enemies);

                for (int i = 0; i < damage.Length; i++)
                {
                    Destroy(damage[i].gameObject);
                }
            }
            attackTime = startTimeAttack;
        }
        else
        {
            attackTime -= Time.deltaTime;
            myAnimator.SetBool("isAttackingOne", false);
        }
    }

    }
