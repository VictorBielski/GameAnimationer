using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] int maxJump = 2;
    [SerializeField] float dashSpeed = 1.5f;
    [SerializeField] float slideTimer = 0f;
    [SerializeField] float slideTimerMax = 1f;

 

    Rigidbody2D myRigidbody;
    Animator myAnimator;

    bool sliding = false;
    public bool canDoubleJump;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        flipSprite();
        Jump();
        dashSlide();
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
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidbody.velocity += jumpVelocityToAdd;
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
