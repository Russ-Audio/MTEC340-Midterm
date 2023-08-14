using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float MaxSpeed;
    public float DashSpeed;
    public Vector3 RotateSpeed;
    private Rigidbody2D _rb;
    private int HealthTracker = 0;

    public GameObject Bullet;
    public float BulletSpeed;
    public float flashSpeed = 30f;
    private SpriteRenderer sR;
    private Color playerColor;

    public PolygonCollider2D playerCollider;
    public CircleCollider2D bulletCollider;

    private GameState _state;

    private bool _isRotating = false;
    private bool _isShooting;
    private bool _isDashing;
    private bool _isShot;
    private bool _flashing;
   

    public KeyCode Rotate;
    public KeyCode Dash;
    public KeyCode Shoot;

    public bool shootCD;
    public float shootCDTime;
    public bool dashReset;

    private int LayerIgnore;
    private int NormalLayer;

    private GameBehaviour gameBehaviour;

    [SerializeField] AudioClip[] playerSFX;
    [SerializeField] AudioClip abilitySFX;
    [SerializeField] AudioClip beepSFX;
    private AudioSource audioSource;

    [SerializeField] int PlayerIndex;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        dashReset = true;
        shootCD = false;
        _flashing = false;
        audioSource = GetComponent<AudioSource>();
        HealthTracker = 3;
        //Debug.Log(transform.forward);
        sR = GetComponent<SpriteRenderer>();
        playerColor = sR.color;

        playerCollider = GetComponent<PolygonCollider2D>();
        bulletCollider = Bullet.GetComponent<CircleCollider2D>();
        InitializePhysics();

        NormalLayer = LayerMask.NameToLayer("Default");
        LayerIgnore = LayerMask.NameToLayer("Non-Colliding Player");

        if (GameBehaviour.Instance)
        {
            GameBehaviour.Instance.SetPlayerRef(PlayerIndex, this.gameObject);
        }

    }

    private void Update()
    {
        //Check for Rotation Input
        if (Input.GetKeyDown(Rotate))
        {
            _isRotating = true;
        }
        else if (Input.GetKeyUp(Rotate))
        {
            _isRotating = false;
        }

        //Check for Dash Input
        _isDashing |= Input.GetKeyDown(Dash);

        //Check for Shoot Input
        _isShooting |= Input.GetKeyDown(Shoot);

        if (_isShot)
        {
            //Physics2D.IgnoreCollision(playerCollider, bulletCollider, ignore: true);
            this.gameObject.layer = LayerIgnore;
            _flashing = true;
        }
        else
        {
            //Physics2D.IgnoreCollision(playerCollider, bulletCollider, ignore: false);
            this.gameObject.layer = NormalLayer;
            _flashing = false;
        }

        if (!_flashing)
        {
            sR.color = playerColor;
        }
        else
        {
            float sineFlashTime = Mathf.Sin(Time.time * flashSpeed) * 0.5f;
            if(Mathf.CeilToInt(sineFlashTime) == 1)
            {
                sR.color = playerColor;
            }
            else
            {
                sR.color = Color.black;
            }
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Rotate
        if (_isRotating == true)
        {
            transform.rotation = Quaternion.Euler(RotateSpeed) * transform.rotation;
        }

        //Accelerate
        _rb.AddForce(transform.up * MaxSpeed);

        //Dash
        if (_isDashing && dashReset)
        {
            _rb.AddForce(transform.right * DashSpeed, ForceMode2D.Impulse);
            audioSource.PlayOneShot(playerSFX[Random.Range(0, 3)]);
            dashReset = false;
        }

        _isDashing = false;

        //Shoot
        if (_isShooting && !shootCD)
        {
            GameObject newBullet = Instantiate(Bullet, transform.position + (transform.up * 0.45f), Quaternion.identity);
            Rigidbody2D BulletRB = newBullet.GetComponent<Rigidbody2D>();
            BulletRB.velocity = transform.up * BulletSpeed;
            audioSource.PlayOneShot(playerSFX[3]);
            shootCD = true;
            Invoke(nameof(CoolDownShoot), shootCDTime);
        }

        _isShooting = false;
    }

    private void CoolDownShoot()
    {
        shootCD = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //This is done so that the player doesn't bounce after colliding with Environment
        if (collision.gameObject.CompareTag("Environment"))
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0;
            dashReset = true;
        }
        //Collision with Laser = Instant Death
        if (collision.gameObject.CompareTag("Laser") && _state != GameState.GameOver)
        {
            //Debug.Log("HIT!!!");
            GameBehaviour.Instance.PlayerDamage(PlayerIndex, 3);
        }
        //Collision with Bullet -= 1 Life
        if (collision.gameObject.CompareTag("Bullet"))
        {

            _isShot = true;

            Debug.Log("Collision Detected");

            if (HealthTracker != 0 && _state != GameState.RoundOver)
            {
                audioSource.PlayOneShot(playerSFX[4], 0.8f);

            }
            GameBehaviour.Instance.PlayerDamage(PlayerIndex, 1);
            HealthTracker = (HealthTracker + 1) % 3;
            //Debug.Log(HealthTracker);

            MaxSpeed = 8;
            DashSpeed = 4;
            shootCDTime = 0;
            if(SceneManager.GetActiveScene().buildIndex != 1)
            {
                audioSource.PlayOneShot(beepSFX);
                StartCoroutine(Invisible());
                Debug.Log("Invisible On");
            }

            /*
            audioSource.PlayOneShot(beepSFX);
            StartCoroutine(Invisible());
            Debug.Log("Invisible On");
            */
        }
    }

    IEnumerator Invisible()
    {
        yield return new WaitForSeconds(3f);
        audioSource.PlayOneShot(abilitySFX);
        _isShot = false;
        InitializePhysics();
        Debug.Log("No Longer Invisible");

    }

    void InitializePhysics()
    {
        MaxSpeed = 4;
        DashSpeed = 2;
        shootCDTime = 0.5f;
    }
}
