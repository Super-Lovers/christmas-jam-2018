using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    public float CharacterSpeedField = 0.3f;
    public float CharacterSpeed = 0;
    public static int CharacterHitPoints = 10;
    public string PlayerMode = "Flying";
    public Camera MainCamera;
    
    private Rigidbody2D _rigidbody2d;
    private SpriteRenderer _spriteRenderer;
    public Sprite PlayerMeleeSprite;
    public Sprite DamagedSprite;
    public GameObject Sword;
    private bool _canShoot = true;
    private bool _canUseMelee = true;
    private Animator _animator;
    public RuntimeAnimatorController _animatorMelee;
    private AudioSource[] _audioSources;

    public static AudioSource MusicSource;
    public static AudioSource SoundsSource;
    public AudioClip WeaponSlashSound;
    public AudioClip WindMusic;
    public AudioClip WalkingMusic;
    public AudioClip PlayerHitSound;

    public GameObject SnowBall;

    void Start()
    {
        StageController.IsPlayerSpeedUpdated = false;
        CharacterSpeed = CharacterSpeedField;
        CharacterHitPoints = 10;
        StageController.CurrentStage = 1;
        StageController.CurrentWave = 1;
        StageController.EnemiesCurrentlyAlive = 0;

        Sword.SetActive(false);
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSources = GetComponents<AudioSource>();

        // We split the audiosources so that we have one for
        // background audio and one for sound clips and effects
        foreach (AudioSource source in _audioSources)
        {
            if (source.clip != null)
            {
                MusicSource = source;
                MusicSource.enabled = false;
            } else
            {
                SoundsSource = source;
            }
        }
    }

    void Update() {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        if (PlayerMode == "Flying")
        {
            MusicSource.clip = WindMusic;
            MusicSource.enabled = true;

            if (horizontalMovement != 0)
            {
                _rigidbody2d.AddForce(new Vector2(horizontalMovement * CharacterSpeed, 0));
            }

            if (_canShoot && Input.GetMouseButtonDown(0))
            {
                // Depending on the tag assigned to the snowball, different
                // code blocks will be responsible for different colliding objects
                GameObject snowball = Instantiate(SnowBall, transform.position, Quaternion.identity);
                snowball.tag = "Player Snowball";
                Invoke("EnableShooting", 0.5f);
                _canShoot = false;
            }
        } else if (PlayerMode == "Grounded")
        {
            MusicSource.clip = WalkingMusic;

            // This lets us update the player's animations using
            // the new stage's player sprites and animator and
            // then we adjust the collider for those new sprites as well.
            _animator.runtimeAnimatorController = _animatorMelee;
            GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.5f);

            Vector3 newCameraPosition = MainCamera.transform.position;
            newCameraPosition.x = transform.position.x;
            newCameraPosition.y = transform.position.y;
            MainCamera.transform.position = newCameraPosition;

            if (_canUseMelee)
            {
                Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                (Vector2)transform.position).normalized;

                transform.up = direction;
            }

        //     if (CutscenesManager.IsCutsceneOver)
        //     {
        //         if (horizontalMovement != 0)
        //         {
        //             _rigidbody2d.AddForce(new Vector2(horizontalMovement * CharacterSpeed * Time.deltaTime, 0));
        //         }
        //         if (verticalMovement != 0)
        //         {
        //             _rigidbody2d.AddForce(new Vector2(0, verticalMovement * CharacterSpeed * Time.deltaTime));
        //         }

        //         if (horizontalMovement == 0 && verticalMovement == 0)
        //         {
        //             MusicSource.enabled = false;
        //         } else
        //         {
        //             MusicSource.enabled = true;
        //         }

        //         if (_canUseMelee && Input.GetMouseButtonDown(0))
        //         {
        //             PlayerController.SoundsSource.PlayOneShot(WeaponSlashSound);
        //             _animator.SetBool("isPlayerAttacking", true);

        //             Sword.SetActive(true);
        //             Sword.GetComponent<BoxCollider2D>().enabled = true;

        //             Invoke("EnableHittingMelee", 0.5f);
        //             Invoke("HideWeapon", 0.1f);

        //             _canUseMelee = false;
        //         }
        //     }
        // }
    }

    // private void EnableShooting()
    // {
    //     _canShoot = true;
    }

   /* private IEnumerator FlashPlayer()
    {
        for (int i = 0; i < 3; i++)
        {
            _spriteRenderer.sprite = DamagedSprite;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.sprite = DefaultSprite;
            yield return new WaitForSeconds(0.1f);
        }
    }*/

    public void DamagePlayer()
    {
        SoundsSource.PlayOneShot(PlayerHitSound);

        //StartCoroutine(FlashPlayer());
        _animator.SetBool("isPlayerHit", true);
        Invoke("StopPlayerHitAnimation", 1f);
        CharacterHitPoints--;

        // Game over condition
        if (CharacterHitPoints <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void StopPlayerHitAnimation()
    {
        _animator.SetBool("isPlayerHit", false);
    }

    public void UpdatePlayerPerspective()
    {
        if (PlayerMode == "Flying")
        {
            PlayerMode = "Grounded";
            transform.SetPositionAndRotation(transform.position, new Quaternion(90, 0, 0, 0));
        }
        else if (PlayerMode == "Grounded")
        {
            PlayerMode = "Flying";
            transform.SetPositionAndRotation(transform.position, new Quaternion(0, 0, 0, 0));
        }
    }

    private void EnableHittingMelee()
    {
        _canUseMelee = true;
        _animator.SetBool("isPlayerAttacking", false);
    }

    private void HideWeapon()
    {
        Sword.GetComponent<BoxCollider2D>().enabled = false;
        Sword.SetActive(false);
    }
}
