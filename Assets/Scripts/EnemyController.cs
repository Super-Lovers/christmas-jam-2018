using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public int EnemyHitPoints = 3;
    [Range(0, 1)]
    public float EnemySpeed = 0.05f;
    public GameObject Snowball;

    private MeshRenderer _meshRenderer;
    private Vector3 _startPosition;
    private bool _isFlickerComplete = true;
    private bool _canPickNewDirection = true;
    private bool _goLeft = false;
    private bool _canShoot = true;

	void Start () {
        _meshRenderer = GetComponent<MeshRenderer>();
        _startPosition = transform.position;
	}

    private void Update()
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
            } else
            {
                _goLeft = false;
            }

            _canPickNewDirection = false;
            Invoke("EnablePickingDirection", 0.5f);
        }

        Vector3 currentPosition = transform.position;
        if (_goLeft)
        {
            if (currentPosition.x > -5)
            {
                currentPosition.x -= EnemySpeed;
            } else
            {
                currentPosition.x += EnemySpeed;
            }
        } else
        {
            if (currentPosition.x < 5)
            {
                currentPosition.x += EnemySpeed;
            }
            else
            {
                currentPosition.x -= EnemySpeed;
            }
        }
        transform.position = currentPosition;
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
            _meshRenderer.material.color = new Color32(0xFF, 0x00, 0x00, 0x00);
            yield return new WaitForSeconds(0.1f);
            _meshRenderer.material.color = new Color32(0xFF, 0xFF, 0xFF, 0x00);
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
            }
            Destroy(gameObject);
        }
    }
}
