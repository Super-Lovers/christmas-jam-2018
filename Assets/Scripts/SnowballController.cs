using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour {
    public int SnowballPower = 1;
    [Range(0, 1)]
    public float SnowballSpeed = 0.3f;
    private Rigidbody _rigidbody;

	void Start() {
        _rigidbody = GetComponent<Rigidbody>();

        // After a few seconds from spawning, the snowball will be destroyed
        // from the game to save performance
        Invoke("DestroySnowball", 3f);
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        if (transform.CompareTag("Enemy Snowball"))
        {
            currentPosition.y -= SnowballSpeed;
        } else if (transform.CompareTag("Player Snowball"))
        {
            currentPosition.y += SnowballSpeed;
        }
        transform.position = currentPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.CompareTag("Player Snowball") &&
            other.transform.CompareTag("Enemy"))
        {
            // Makes the enemy flicker damaging effects when hit and lose hit points.
            other.gameObject.GetComponent<EnemyController>().DamageEnemy();

            Destroy(gameObject);
        } else if (transform.CompareTag("Enemy Snowball") &&
            other.transform.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().DamagePlayer();

            Destroy(gameObject);
        }
    }

    private void DestroySnowball()
    {
        Destroy(gameObject);
    }
}
