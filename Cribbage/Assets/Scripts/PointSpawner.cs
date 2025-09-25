using System;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public GameObject pointPrefab; // Reference to the Point prefab
    public Transform jar;          // Reference to the jar (Image)
    public Transform canvas;       // Reference to the Canvas
    public GameObject PointSpawnerObject; // Reference to the PointSpawner GameObject, that has an animation

    public float minX = 700f;
    public float maxX = 800f;

    private bool isAnimating = false;

    public void SpawnPoint()
    {
        Debug.Log("Spawning point");
        if (pointPrefab == null || canvas == null)
        {
            Debug.LogWarning("PointPrefab or Canvas is not assigned.");
            return;
        }

        // Only play animation if not already animating
        if (PointSpawnerObject != null && !isAnimating)
        {
            PointSpawnerObject.SetActive(true); // Make visible

            Animator animator = PointSpawnerObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Points"); // Replace with your animation's name or use a trigger
            }

            StartCoroutine(HideSpawnerAfterDelay(0.6f));
            isAnimating = true;
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

    private System.Collections.IEnumerator HideSpawnerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (PointSpawnerObject != null)
            PointSpawnerObject.SetActive(false);
        isAnimating = false;
    }

    void Start()
    {
        PointSpawnerObject.SetActive(false); // Initially hide the PointSpawnerObject
        isAnimating = false;
    }
}