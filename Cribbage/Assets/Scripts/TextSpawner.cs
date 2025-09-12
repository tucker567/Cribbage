using UnityEngine;
using System.Collections;

public class TextSpawner : MonoBehaviour
{
    public GameObject textPrefab; // Assign in inspector
    public Transform spawnPoint; // Assign in inspector
    public float moveSpeed = 2f; // Speed at which the text moves upwards
    public float fadeDuration = 1f; // Duration over which the text fades out
    public Pegging pegging; // Reference to Pegging script to get score info


}
