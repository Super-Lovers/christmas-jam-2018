using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour {
    public int SnowballPower = 1;
    [Range(0, 100)]
    public float SnowballSpeed = 55f;
    private Rigidbody2D _rigidbody2d;

	void Start() {
        _rigidbody2d = GetComponent<Rigidbody2D>();

        // After a few seconds from spawning, the snowball will be destroyed
        // from the game to save performance
        Invoke("DestroySnowball", 3f);
    }

    private void Update()
    {
        if (transform.CompareTag("Enemy Snowball"))
        {
            _rigidbody2d.AddForce(new Vector2(0, -SnowballSpeed));
        } else if (transform.CompareTag("Player Snowball"))
        {
            _rigidbody2d.AddForce(new Vector2(0, SnowballSpeed));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
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
