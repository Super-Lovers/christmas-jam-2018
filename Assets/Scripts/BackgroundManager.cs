using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {
    public GameObject[] clouds;
    private GameObject _cloudsContainer;

	// Use this for initialization
	void Start () {
        _cloudsContainer = GameObject.Find("Clouds");

        InvokeRepeating("SpawnCloud", 1f, 1.5f);
    }
	
    private void SpawnCloud()
    {
        Vector2 newCloudPosition = new Vector2(Random.Range(-5, 5), 7);

        GameObject newCloud = Instantiate(
            clouds[Random.Range(0, clouds.Length)],
            newCloudPosition, Quaternion.identity,
            _cloudsContainer.transform);
    }
}
