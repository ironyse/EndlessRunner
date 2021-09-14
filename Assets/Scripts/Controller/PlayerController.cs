using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private float raycastDistance = 0.1f;

    private PlayerSoundController sound;
    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;
    private Animator anim;

    [Header("Movement")]
    public float moveAccel = 2f;
    public float maxSpeed = 4f;
    public float limitMaxSpeed = 8f;
    [Range(1f, 2.0f)] public float speedMultiplier = 1.05f;

    [Header("Jump")]
    public float jumpAccel;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;
    private float lastPosX;

    [Header("Currency")]
    public CurrencyController currency;

    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPosY;

    [Header("Camera")]
    public CameraController gameCamera;

    private bool isJumping;
    private bool isDead;

    // Using boxcast instead of raycast, karena bisa terdeteksi tidak di tanah kalau karakter berada di ujung platform
    private bool IsGrounded() {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.down , raycastDistance, platformLayerMask);
        return raycastHit2d.collider != null;
    }

    void Start(){
        rb2d = transform.GetComponent<Rigidbody2D>();
        bc2d = transform.GetComponent<BoxCollider2D>();
        anim = transform.GetComponent<Animator>();
        sound = transform.GetComponent<PlayerSoundController>();
        isDead = false;
        anim.runtimeAnimatorController = GameManager.selectedPlayerAnimator;
    }
    
    void Update(){
        if (isDead) return;
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space)) {
            isJumping = true;
            sound.PlayJump();
        }
        anim.SetBool("IsGrounded", IsGrounded());

        //kalkulasi skor berdasar jarak yang ditempuh
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPosX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);
        if (scoreIncrement > 0) {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPosX += distancePassed;
        }

        //game over bisa terjatuh
        if (transform.position.y < fallPosY) {
            Die();            
        }
    }

    public void GameOver() {        
        score.FinishScoring();
        currency.SaveCurrency();
        gameCamera.enabled = false;
        gameOverScreen.SetActive(true); 
        this.enabled = false; //disable player control
        GameManager.Instance.Save();
    }

    void FixedUpdate(){
        if (isDead) return;
        Vector2 velocity = rb2d.velocity;
        if (isJumping){
            velocity.y += jumpAccel;
            isJumping = !isJumping;
        }
        velocity.x = Mathf.Clamp(velocity.x + moveAccel * Time.deltaTime, .0f, maxSpeed);
        rb2d.velocity = velocity;
    }

    private void Die(){        
        isDead = true;
        rb2d.gravityScale = 0f;
        rb2d.velocity = Vector3.zero;
        anim.SetBool("IsDead", isDead);
        GameOver();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        Spikes spike = collision.GetComponent<Spikes>();
        Collectibles diamond = collision.GetComponent<Collectibles>();
        if (spike) Die(); // panggil fungsi die kalau menyentuh spike
        if (diamond){
            currency.IncreaseCurrentAmount();
            diamond.gameObject.SetActive(false);
        }
    }
}
