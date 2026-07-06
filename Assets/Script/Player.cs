using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveForward = 10f;

    [SerializeField]
    private float jumpForward = 15f;

    private float movement;

    private Rigidbody2D rbody;
    private Animator animator;

    private string WALK_ANIMTION = "Walk";

    private SpriteRenderer spriteRenderer;

    private bool isGrounded = true;

    private string PLATFORM = "Ground";
    private string OBSTACLE = "Obstacle";
    private string SCORE = "scoring";

    private string LEVEL03 = "Level03";
    private string LEVEL02 = "Level02";
    private string LEVEL04 = "Level04";
    private string LEVEL05 = "Level05";
    private string LEVEL06 = "Level06";
    private string LEVEL07 = "Level07";
    private string LEVEL08 = "Level08";
    private string LEVEL09 = "Level09";
    private string LEVEL10 = "Level10";
    private string LEVEL11 = "Level11";
    private string LEVEL12 = "Level12";
    private string LEVEL13 = "Level13";
    private string LEVEL14 = "Level14";
    private string LEVEL15 = "Level15";

    private float leftEdge;

    [SerializeField]
    private float mobileAutoMove = 0.01f;

    [SerializeField]
    private float rightLimit = 10f;

    // Temporary protection after continue
    public bool isInvincible = false;

    private void Start()
    {
        if (Camera.main != null)
        {
            leftEdge =
                Camera.main
                .ScreenToWorldPoint(Vector3.zero)
                .x - 1;
        }
        else
        {
            Debug.LogError(
                "Main Camera not found."
            );
        }
    }

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rbody == null)
        {
            Debug.LogError(
                "Player missing Rigidbody2D."
            );
        }

        if (animator == null)
        {
            Debug.LogError(
                "Player missing Animator."
            );
        }
    }

    void PlayerMovement()
    {
        // Keyboard movement
        movement = Input.GetAxisRaw("Horizontal");

#if UNITY_ANDROID || UNITY_IOS
        movement += mobileAutoMove;
#endif

        transform.position +=
            new Vector3(movement, 0f, 0f) *
            Time.deltaTime *
            moveForward;
    }

    void AnimatePlayer()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(
            WALK_ANIMTION,
            movement > 0
        );
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        if (pos.x > rightLimit)
        {
            pos.x = rightLimit;
            transform.position = pos;
        }
    }
    void PlayerJump()
    {
        if ((Input.GetButtonDown("Jump") ||
             (Input.touchCount > 0 &&
              Input.GetTouch(0).phase ==
              TouchPhase.Began))
            && isGrounded)
        {
            isGrounded = false;

            if (rbody != null)
            {
                rbody.AddForce(
                    new Vector2(
                        0,
                        jumpForward
                    ),
                    ForceMode2D.Impulse
                );
            }
        }
    }



    public void OnCollisionEnter2D(Collision2D collision2D)
    {
        // Ground collision
        if (collision2D.gameObject.CompareTag(PLATFORM))
        {
            isGrounded = true;
        }

        // Obstacle collision
        if (collision2D.gameObject.CompareTag(OBSTACLE))
        {
            // Ignore obstacle collision temporarily
            if (isInvincible)
            {
                return;
            }

            string currentScene =
                SceneManager.GetActiveScene().name;

            Debug.Log("Current Scene ::: " + currentScene);

            if (currentScene.Equals(LEVEL15))
            {
                FindFirstObjectByType<GameManager15>().GameOver();
            }
            else if (currentScene.Equals(LEVEL14))
            {
                FindFirstObjectByType<GameManager14>().GameOver();
            }
            else if (currentScene.Equals(LEVEL13))
            {
                FindFirstObjectByType<GameManager13>().GameOver();
            }
            else if (currentScene.Equals(LEVEL12))
            {
                FindFirstObjectByType<GameManager12>().GameOver();
            }
            else if (currentScene.Equals(LEVEL11))
            {
                FindFirstObjectByType<GameManager11>().GameOver();
            }
            else if (currentScene.Equals(LEVEL10))
            {
                FindFirstObjectByType<GameManager10>().GameOver();
            }
            else if (currentScene.Equals(LEVEL09))
            {
                FindFirstObjectByType<GameManager09>().GameOver();
            }
            else if (currentScene.Equals(LEVEL08))
            {
                FindFirstObjectByType<GameManager08>().GameOver();
            }
            else if (currentScene.Equals(LEVEL07))
            {
                FindFirstObjectByType<GameManager07>().GameOver();
            }
            else if (currentScene.Equals(LEVEL06))
            {
                FindFirstObjectByType<GameManager06>().GameOver();
            }
            else if (currentScene.Equals(LEVEL05))
            {
                FindFirstObjectByType<GameManager05>().GameOver();
            }
            else if (currentScene.Equals(LEVEL04))
            {
                FindFirstObjectByType<GameManager04>().GameOver();
            }
            else if (currentScene.Equals(LEVEL03))
            {
                FindFirstObjectByType<GameManager03>().GameOver();
            }
            else if (currentScene.Equals(LEVEL02))
            {
                FindFirstObjectByType<GameManager02>().GameOver();
            }
            else
            {
                FindFirstObjectByType<GameManager>().GameOver();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(SCORE))
        {
            string currentScene =
                SceneManager.GetActiveScene().name;

            Debug.Log("Current Scene ::: " + currentScene);

            if (currentScene.Equals(LEVEL15))
            {
                FindFirstObjectByType<GameManager15>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL14))
            {
                FindFirstObjectByType<GameManager14>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL13))
            {
                FindFirstObjectByType<GameManager13>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL12))
            {
                FindFirstObjectByType<GameManager12>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL11))
            {
                FindFirstObjectByType<GameManager11>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL10))
            {
                FindFirstObjectByType<GameManager10>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL09))
            {
                FindFirstObjectByType<GameManager09>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL08))
            {
                FindFirstObjectByType<GameManager08>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL07))
            {
                FindFirstObjectByType<GameManager07>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL06))
            {
                FindFirstObjectByType<GameManager06>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL05))
            {
                FindFirstObjectByType<GameManager05>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL04))
            {
                FindFirstObjectByType<GameManager04>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL03))
            {
                FindFirstObjectByType<GameManager03>().IncreaseScore();
            }
            else if (currentScene.Equals(LEVEL02))
            {
                FindFirstObjectByType<GameManager02>().IncreaseScore();
            }
            else
            {
                FindFirstObjectByType<GameManager>().IncreaseScore();
            }
        }
    }

    void Update()
    {
        PlayerMovement();

        AnimatePlayer();

        //    transform.position +=
        //      Vector3.left * moveForward * Time.deltaTime;

        string currentScene =
            SceneManager.GetActiveScene().name;

        if (transform.position.x < leftEdge)
        {
            if (currentScene.Equals(LEVEL15))
            {
                FindFirstObjectByType<GameManager15>().GameOver();
            }
            else if (currentScene.Equals(LEVEL14))
            {
                FindFirstObjectByType<GameManager14>().GameOver();
            }
            else if (currentScene.Equals(LEVEL13))
            {
                FindFirstObjectByType<GameManager13>().GameOver();
            }
            else if (currentScene.Equals(LEVEL12))
            {
                FindFirstObjectByType<GameManager12>().GameOver();
            }
            else if (currentScene.Equals(LEVEL11))
            {
                FindFirstObjectByType<GameManager11>().GameOver();
            }
            else if (currentScene.Equals(LEVEL10))
            {
                FindFirstObjectByType<GameManager10>().GameOver();
            }
            else if (currentScene.Equals(LEVEL09))
            {
                FindFirstObjectByType<GameManager09>().GameOver();
            }
            else if (currentScene.Equals(LEVEL08))
            {
                FindFirstObjectByType<GameManager08>().GameOver();
            }
            else if (currentScene.Equals(LEVEL07))
            {
                FindFirstObjectByType<GameManager07>().GameOver();
            }
            else if (currentScene.Equals(LEVEL06))
            {
                FindFirstObjectByType<GameManager06>().GameOver();
            }
            else if (currentScene.Equals(LEVEL05))
            {
                FindFirstObjectByType<GameManager05>().GameOver();
            }
            else if (currentScene.Equals(LEVEL04))
            {
                FindFirstObjectByType<GameManager04>().GameOver();
            }
            else if (currentScene.Equals(LEVEL03))
            {
                FindFirstObjectByType<GameManager03>().GameOver();
            }
            else if (currentScene.Equals(LEVEL02))
            {
                FindFirstObjectByType<GameManager02>().GameOver();
            }
            else
            {
                FindFirstObjectByType<GameManager>().GameOver();
            }
        }
    }

    private void FixedUpdate()
    {
        PlayerJump();
    }

    private void OnValidate()
    {
        if (moveForward < 0f)
        {
            moveForward = 0f;
        }

        if (jumpForward < 0f)
        {
            jumpForward = 0f;
        }

        if (mobileAutoMove < 0f)
        {
            mobileAutoMove = 0f;
        }
    }
}