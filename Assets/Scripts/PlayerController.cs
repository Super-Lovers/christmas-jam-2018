using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float CharacterSpeed = 0.3f;
    public int CharacterHitPoints = 3;
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

    public GameObject SnowBall;

    void Start()
    {
        Sword.SetActive(false);
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update() {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        if (PlayerMode == "Flying")
        {
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

            if (CutscenesManager.IsCutsceneOver)
            {
                if (horizontalMovement != 0)
                {
                    _rigidbody2d.AddForce(new Vector2(horizontalMovement * CharacterSpeed, 0));
                }
                if (verticalMovement != 0)
                {
                    _rigidbody2d.AddForce(new Vector2(0, verticalMovement * CharacterSpeed));
                }

                if (_canUseMelee && Input.GetKeyDown(KeyCode.Space))
                {
                    Sword.SetActive(true);
                    Sword.GetComponent<BoxCollider2D>().enabled = true;

                    Invoke("EnableHittingMelee", 0.5f);
                    Invoke("HideWeapon", 0.1f);

                    _canUseMelee = false;
                }
            }
        }
    }

    private void EnableShooting()
    {
        _canShoot = true;
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
        //StartCoroutine(FlashPlayer());
        _animator.SetBool("isPlayerHit", true);
        Invoke("StopPlayerHitAnimation", 1f);
        CharacterHitPoints--;

        // Game over condition
        if (CharacterHitPoints <= 0)
        {
            Destroy(gameObject);
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
    }

    private void HideWeapon()
    {
        Sword.GetComponent<BoxCollider2D>().enabled = false;
        Sword.SetActive(false);
    }
}
