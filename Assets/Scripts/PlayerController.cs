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
    public Sprite DefaultSprite;
    public Sprite DamagedSprite;
    private bool _canShoot = true;

    public GameObject SnowBall;

    void Start() {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
            Vector3 newCameraPosition = MainCamera.transform.position;
            newCameraPosition.x = transform.position.x;
            newCameraPosition.y = transform.position.y;
            MainCamera.transform.position = newCameraPosition;

            Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                (Vector2)transform.position).normalized;

            transform.up = direction;

            if (horizontalMovement != 0)
            {
                _rigidbody2d.AddForce(new Vector2(horizontalMovement * CharacterSpeed, 0));
            }
            if (verticalMovement != 0)
            {
                _rigidbody2d.AddForce(new Vector2(0, verticalMovement * CharacterSpeed));
            }

            if (Input.GetMouseButtonDown(0))
            {
                // Depending on the tag assigned to the snowball, different
                // code blocks will be responsible for different colliding objects
                GameObject snowball = Instantiate(SnowBall, transform.position, Quaternion.identity);
                snowball.tag = "Player Snowball";
            }
        }
    }

    private void EnableShooting()
    {
        _canShoot = true;
    }

    private IEnumerator FlashPlayer()
    {
        for (int i = 0; i < 3; i++)
        {
            // Color32 uses hexadecimals instead of rgba
            _spriteRenderer.sprite = DamagedSprite;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.sprite = DefaultSprite;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DamagePlayer()
    {
        StartCoroutine(FlashPlayer());
        CharacterHitPoints--;

        // Game over condition
        if (CharacterHitPoints <= 0)
        {
            Destroy(gameObject);
        }
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
}
