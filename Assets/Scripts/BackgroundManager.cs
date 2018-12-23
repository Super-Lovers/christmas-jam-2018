using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {
    public GameObject[] Clouds;
    private GameObject _cloudsContainer;
    public GameObject Ground;
    public GameObject[] Environment;
    private GameObject _environmentContainer;

    // Use this for initialization
    void Start () {
        _cloudsContainer = GameObject.Find("Clouds");
        _environmentContainer = GameObject.Find("Environmentals");

        InvokeRepeating("SpawnCloud", 1f, 1.5f);

        for (int i = 0; i < 6; i++)
        {
            InvokeRepeating("SpawnEnvironmental", 15f, 20f);
        }
    }
	
    private void SpawnCloud()
    {
        Vector2 newPosition = new Vector2(Random.Range(-5, 5), 7);

        GameObject newCloud = Instantiate(
            Clouds[Random.Range(0, Clouds.Length)],
            newPosition, Quaternion.identity,
            _cloudsContainer.transform);
    }

    private void SpawnEnvironmental()
    {
        Vector2 newPosition = new Vector2(Random.Range(-5, 5), Random.Range(7, 10));

        GameObject newEnvironmental = Instantiate(
            Environment[Random.Range(0, Environment.Length)],
            newPosition, Quaternion.identity,
            _environmentContainer.transform);

        Physics2D.IgnoreLayerCollision(9, 8);
        Physics2D.IgnoreLayerCollision(9, 10);
    }
}
