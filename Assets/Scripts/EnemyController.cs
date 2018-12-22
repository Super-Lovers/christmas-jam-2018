using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public int EnemyHitPoints = 3;
    [Range(0, 10)]
    public float EnemySpeed = 5f;
    [Range(0, 1)]
    public float EnemyRunningSpeed = 0.3f;
    public GameObject Snowball;
    public Sprite DefaultSprite;
    public Sprite DamagedSprite;
    public LayerMask PlayerLayerMask;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2d;
    private bool _isFlickerComplete = true;
    private bool _canPickNewDirection = true;
    private bool _goLeft = false;
    private bool _canShoot = true;
    private GameObject _player;

    void Start () {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (StageController.CurrentStage == 1)
        {
            if (_canShoot)
            {
                GameObject snowball = Instantiate(Snowball,
                    transform.position, Quaternion.identity);
                snowball.tag = "Enemy Snowball";

                _canShoot = false;
                Invoke("EnableShooting", 1f);
            }

            if (_canPickNewDirection)
            {
                if (Random.Range(0, 101) > 50)
                {
                    _goLeft = true;
                }
                else
                {
                    _goLeft = false;
                }

                _canPickNewDirection = false;
                Invoke("EnablePickingDirection", Random.Range(0.5f, 0.8f));
            }

            Vector3 currentPosition = transform.position;
            if (_goLeft)
            {
                if (currentPosition.x > -5)
                {
                    _rigidbody2d.AddForce(new Vector2(-EnemySpeed, 0));
                }
                else
                {
                    _rigidbody2d.AddForce(new Vector2(EnemySpeed, 0));
                }
            }
            else
            {
                if (currentPosition.x < 5)
                {
                    _rigidbody2d.AddForce(new Vector2(EnemySpeed, 0));
                }
                else
                {
                    _rigidbody2d.AddForce(new Vector2(-EnemySpeed, 0));
                }
            }
        } else if (StageController.CurrentStage == 2)
        {
            if (Vector2.Distance(transform.position, _player.transform.position) < 5)
            {
                Vector2 direction = ((Vector2)transform.forward -
                    (Vector2)_player.transform.position).normalized;

                transform.up = direction;

                transform.position = 
                    Vector2.MoveTowards(transform.position,
                    _player.transform.position, EnemyRunningSpeed);
            }
        }
    }

    private void EnableShooting()
    {
        _canShoot = true;
    }

    private void EnablePickingDirection()
    {
        _canPickNewDirection = true;
    }

    private IEnumerator FlashEnemy()
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

    public void DamageEnemy()
    {
        StartCoroutine(FlashEnemy());
        EnemyHitPoints--;

        if (EnemyHitPoints <= 0 && _isFlickerComplete)
        {
            // Once all opponents are defeated, the next wave
            // can commence (and the break between each wave).
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 1)
            {
                StageController.IsEnemyDefeated = true;
                StageController.CurrentWave++;

                if (StageController.CurrentWave >= 4)
                {
                    StageController.CurrentStage++;
                    StageController.CurrentWave = 1;

                    CutscenesManager.IsCutsceneOver = false;

                    if (StageController.CurrentStage == 2)
                    {
                        CutscenesManager.Stage = "Stage 2";
                    } else if (StageController.CurrentStage == 3)
                    {
                        CutscenesManager.Stage = "Stage 3";
                    }

                    GameObject.Find("Cutscenes Manager")
                        .GetComponent<CutscenesManager>().FadeIn();
                }
            }
            Destroy(gameObject);
        }
    }
}
