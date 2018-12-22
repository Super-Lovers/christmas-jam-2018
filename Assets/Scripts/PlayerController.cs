using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float CharacterSpeed = 0.3f;
    public int CharacterHitPoints = 3;

    private CharacterController _characterController;
    private MeshRenderer _meshRenderer;

    public GameObject SnowBall;

    void Start() {
        _characterController = GetComponent<CharacterController>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update() {
        float horizontalMovement = Input.GetAxis("Horizontal");

        if (horizontalMovement != 0)
        {
            _characterController.Move(
                new Vector3(horizontalMovement * CharacterSpeed, 0, 0));
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Depending on the tag assigned to the snowball, different
            // code blocks will be responsible for different colliding objects
            GameObject snowball = Instantiate(SnowBall, transform.position, Quaternion.identity);
            snowball.tag = "Player Snowball";
        }
    }

    private IEnumerator FlashPlayer()
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
}
