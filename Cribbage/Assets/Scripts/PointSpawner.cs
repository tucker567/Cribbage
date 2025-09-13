using System;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public GameObject pointPrefab; // Reference to the Point prefab
    public Transform spawnPoint;   // Reference to the SpawnPoint transform
    public Transform jar;          // Reference to the jar (Image)
    public Transform canvas;       // Reference to the Canvas

    public float minX = 750f;
    public float maxX = 770f;
    private float textNum = 100f;

    public void SpawnPoint()
    {
        Debug.Log("Spawning point");
        if (pointPrefab == null || spawnPoint == null || canvas == null)
        {
            Debug.LogWarning("PointPrefab, SpawnPoint, or Canvas is not assigned.");
            return;
        }

        // Pick a random X value in the range
        float randomX = UnityEngine.Random.Range(minX, maxX);

        // Instantiate the point at the random X position
        GameObject point = Instantiate(pointPrefab, new Vector3(randomX, 0, 0), Quaternion.identity);

        // Parent the point to the Canvas
        point.transform.SetParent(canvas, false);

        Debug.Log("Instantiated point: " + point.name + " at " + point.transform.position);

        // Optionally, apply a small force to simulate dropping
        Rigidbody2D rb = point.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(Vector2.down * 50f, ForceMode2D.Impulse); // Adjust the force as needed
        }
    }

    public void Loop()
    {
        for (int i = 0; i < textNum; i++)
        {
            SpawnPoint();
        }
    }
    
    void Start()
    {
        Loop();
    }
}