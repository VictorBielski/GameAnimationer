using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Speed på playermodels fart
    [SerializeField] float runSpeed = 5f;
    // Speed på jumps
    [SerializeField] float jumpSpeed = 5f;
    //Speed på dash/slider
    [SerializeField] float dashSpeed = 0.8f;
    // timer til dash/slider
    [SerializeField] float slideTimer = 0f;
    // max timer til dash/slider
    [SerializeField] float slideTimerMax = 0.2f;

 

    Rigidbody2D myRigidbody;
    Animator myAnimator;
    Collider2D myCollider2D;

    // bool til dash/sliding
    bool sliding = false;


    //bool og variable der tjekker, om playermodellen rør "jorden"
    bool isGrounded;
    public Transform groundCheck;


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
        if(isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        if(Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            myRigidbody.velocity = Vector2.up * jumpForce;
            extraJumps--;
        } else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true)
        {
            myRigidbody.velocity = Vector2.up * jumpForce;
        }
    }

    private void flipSprite()
    {
        bool playerHasHoriSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if(playerHasHoriSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    private void dashSlide()
    {
        if (Input.GetButtonDown("Slide") && !sliding)
        {
            slideTimer = 0f;

            myAnimator.SetBool("isSliding", true);


            sliding = true;
        }
            if(sliding)
            {
                slideTimer += Time.deltaTime;

                if(slideTimer > slideTimerMax)
                {
                    sliding = false;

                    myAnimator.SetBool("isSliding", false);
                }
            }

        }
    }
