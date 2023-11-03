using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runMultiplier;
    [SerializeField] private float jumpForce;
    [SerializeField] private float horizontalMove = 0f;
    [SerializeField] private float verticalMove = 0f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Checkpoint checkPoint;
    [SerializeField] private int coins = 0;
    [SerializeField] private Transform weapon;

    private Rigidbody2D rb;
    private Animator animator;


    [SerializeField] public static GameObject instanciaPlayerController;
    private static PlayerController _instanciaPlayerController;
    public static PlayerController InstanciaPlayerController
    {
        get
        {
            if (_instanciaPlayerController == null)
            {
                _instanciaPlayerController = instanciaPlayerController.GetComponent<PlayerController>();
            }
            return _instanciaPlayerController;
        }
    }

    private void Awake()
    {
        instanciaPlayerController = FindObjectOfType<PlayerController>().gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CharacterMoveHorizontal();
    }

    // Update is called once per frame
    void Update()
    {
        IsGroundedCheck();
        GetSprint();
        GetHorizontalVerticalMove();
        CharacterMoveVertical();
        FlipSprite();
        //GetAnimations();
    }

    void GetHorizontalVerticalMove()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
    }

    void CharacterMoveHorizontal()
    {
        rb.velocity = new Vector2(horizontalMove * walkingSpeed, rb.velocity.y);
        if (isRunning)
        {
            rb.velocity = new Vector2(rb.velocity.x * runMultiplier, rb.velocity.y);
        }
    }

    void CharacterMoveVertical()
    {
        //Pulo segurando o botão
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //O player já está pulando mas soltou o botão
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0.0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

    }

    private bool IsGroundedCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        return isGrounded;
    }
    /*
    public void FlipSprite()
    {
        if ((isFacingRight && horizontalMove < 0.0f) || (!isFacingRight && horizontalMove > 0.0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;

            Vector3 localScaleWeapon = transform.localScale;
            localScaleWeapon.y *= -1;
            weapon.localScale = localScaleWeapon;
            
        }
    }
    */

    public void FlipSprite()
    {
        if ((isFacingRight && horizontalMove < 0.0f) || (!isFacingRight && horizontalMove > 0.0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;

            Vector2 scaleWeapon = transform.localScale;
            scaleWeapon.y *= -1;
            weapon.localScale = scaleWeapon;
        }
    }

    private void FlipSpriteWeapon(Vector2 direction)
    {
        Vector3 localScale = transform.localScale;
        Vector2 scaleWeapon = transform.localScale;
        if (direction.x < 0)
        {
            isFacingRight = false;
            scaleWeapon.y = -1;
            localScale.x = -1;
        }
        else
        {
            if (direction.x > 0)
            {
                isFacingRight = true;
                scaleWeapon.y = 1;
                localScale.x = 1;
            }
        }

        weapon.localScale = scaleWeapon;
        transform.localScale = localScale;
        
    }

    public bool IsFacingRight()
    {
        return isFacingRight;
    }

    private void GetSprint()
    {
        if (Input.GetButton("Run"))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    public void AddCoin()
    {
        coins++;
    }

    public void SetCheckpoint(Checkpoint newCheckpoint)
    {
        checkPoint = newCheckpoint;
    }


    public void RestartCheckpoint()
    {
        transform.position = checkPoint.Position();

        //Resetar a gravidade do player.
        rb.velocity = new Vector2(0, 0);
    }

    public void RestartCoins()
    {
        coins = 0;
    }

    private void GetAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animator.SetBool("OnGround", isGrounded);
        animator.SetBool("Running", isRunning);
    }
}

