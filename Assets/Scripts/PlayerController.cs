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
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Checkpoint checkPoint;
    [SerializeField] private int coins = 0;
    [SerializeField] private int playerCurrentHealth = 5;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float invencibilityTime = 1.0f;
    [SerializeField] private bool gamePaused;
    [SerializeField] private float playerAttackSlowMultiplier = 0.6f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isPlayerHit = false;
    private SpriteRenderer spriteRenderer;
    private bool playerKilled = false;
    private int playerFullHealth;
    private GameObject swordCollider;
    private bool isPlayerAttacking = false;
    private SwordAnimation playerSword;
    private SpriteRenderer swordArm;
    private SpriteRenderer swordAxe;


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
        playerFullHealth = playerCurrentHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gamePaused = false;
        swordCollider = transform.Find("ArmPivot/SwordCollider").gameObject;
        playerSword = transform.Find("ArmPivot/Arm/Axe").GetComponent<SwordAnimation>();
        swordArm = transform.Find("ArmPivot/Arm/Arm").GetComponent<SpriteRenderer>();
        swordAxe = transform.Find("ArmPivot/Arm/Axe").GetComponent<SpriteRenderer>();
        swordCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetGamePaused();

        if(!gamePaused) {
            IsGroundedCheck();
            GetHorizontalVerticalMove();
            CharacterMoveHorizontal();
            CharacterMoveVertical();
            GetSprint();
            PlayerAttack();
            GetAnimations();
            PlayerKilledState();
        }
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
    
    public void FlipSprite()
    {
        Vector3 localScale = transform.localScale;

        if (horizontalMove < 0.0f){
            localScale.x = -1;
        }else{
            localScale.x = 1;
        }

        transform.localScale = localScale;
    }
    

    public void FlipSprite(int scale)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = scale;
        transform.localScale = localScale;

    }

    public void SetLocalScale(int value)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = value;
        transform.localScale = localScale;
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

        PlayerAddHealth(playerFullHealth);
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
        animator.SetBool("Paused", gamePaused);
        animator.SetBool("Killed", playerKilled);
        animator.SetBool("AttackSwordGround", isPlayerAttacking);
    }

    private void GetGamePaused() {
        if(Time.timeScale > 0) {
            gamePaused = false;
        } else {
            gamePaused = true;
        }
    }

    public void PlayerDamage(int damage) {
        StartCoroutine(PlayerDamageCoroutine(damage));
    }

    public bool IsPlayerHit() {
        return isPlayerHit;
    }

    private void NormalizePlayerDamage()
    {
        if(playerCurrentHealth <= 0)
        {
            playerCurrentHealth = 0;
        }
    }

    private IEnumerator PlayerDamageCoroutine(int damage) {
        playerCurrentHealth -= damage;
        NormalizePlayerDamage();


        healthBar.SetHealth(playerCurrentHealth);

        if(!IsPlayerKilled()) {
            isPlayerHit = true;
            PlayerDamagedAnimation();
            yield return new WaitForSeconds(invencibilityTime);
            isPlayerHit = false;
        } else {
            KillPlayer();
            yield return new WaitForSeconds(3.0f);
            ResetScene.InstanciaResetScene.Reset();
        }

    }

    private void PlayerDamagedAnimation() {
        StartCoroutine(PlayerDamagedCoroutine());
    }

    private IEnumerator PlayerDamagedCoroutine() {
        bool isEnabled = true;
        while(isPlayerHit && !playerKilled) {
            isEnabled = !isEnabled;
            spriteRenderer.enabled = isEnabled;
            swordArm.enabled = isEnabled;
            swordAxe.enabled = isEnabled;
            yield return new WaitForSeconds(0.02f);
        }
        spriteRenderer.enabled = true;
        swordArm.enabled = true;
        swordAxe.enabled = true;

    }

    private bool IsPlayerKilled() {
        return playerCurrentHealth <= 0;
    }

    private void KillPlayer() {
        playerKilled = true;
        isPlayerHit = true;
        playerCurrentHealth = 0;
        swordArm.enabled = false;
        swordAxe.enabled = false;
    }

    private void PlayerKilledState() {
        if(playerKilled) {
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void RestartPlayer() {
        playerKilled = false;
        playerCurrentHealth = playerFullHealth;
        isPlayerHit = false;
        swordArm.enabled = true;
        swordAxe.enabled = true;
        healthBar.SetHealth(playerCurrentHealth);
    }

    public void PlayerAddHealth(int amount) {
        if(playerCurrentHealth + amount > playerFullHealth) {
            playerCurrentHealth = playerFullHealth;
        } else {
            playerCurrentHealth += amount;
        }

        healthBar.SetHealth(playerCurrentHealth);
    }

    public int GetPlayerMaxHealth() {
        return playerFullHealth;
    }

    private void PlayerAttack() {
        if(!isPlayerAttacking) {
            if(Input.GetMouseButtonDown(0) && isGrounded) {
                InitPlayerAttack();
            }
        } else {
            //Player não pode correr enquanto ataca, então agora ele irá apenas andar
            if(isRunning) {
                isRunning = false;
                rb.velocity = new Vector2(horizontalMove * walkingSpeed, 0);
            }
            
        }
    }

    private void InitPlayerAttack() {
        StartCoroutine(InitPlayerAttackCoroutine());
    }

    private IEnumerator InitPlayerAttackCoroutine() {
        rb.velocity = new Vector2(0, 0);
        isPlayerAttacking = true;
        playerSword.SetAttack(isPlayerAttacking);
        
        //Sacando a espada
        //yield return new WaitForSeconds(0.1f);

        //Aplicando dano
        swordCollider.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        //Deadframes espada e fim animação
        swordCollider.SetActive(false);
        //yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(1.0f);
        isPlayerAttacking = false;
        playerSword.SetAttack(isPlayerAttacking);

    }
}

