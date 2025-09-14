using System;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public GameObject pointPrefab; // Reference to the Point prefab
    public Transform jar;          // Reference to the jar (Image)
    public Transform canvas;       // Reference to the Canvas

    public float minX = 700f;
    public float maxX = 800f;

    public void SpawnPoint()
    {
        Debug.Log("Spawning point");
        if (pointPrefab == null || canvas == null)
        {
            Debug.LogWarning("PointPrefab or Canvas is not assigned.");
            return;
        }

        // Pick a random X value in the range
        float randomX = UnityEngine.Random.Range(minX, maxX);

        // Instantiate the point at the random X position
        GameObject point = Instantiate(pointPrefab, new Vector3(randomX, 0, 0), Quaternion.identity);

        // Parent the point to the Canvas
        point.transform.SetParent(canvas, false);

        // Optionally, apply a small force to simulate dropping
        Rigidbody2D rb = point.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(Vector2.down * 50f, ForceMode2D.Impulse); // Adjust the force as needed
        }
    }
}