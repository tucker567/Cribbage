using UnityEngine;
using TMPro;
using System.Collections;

public class TextSpawner : MonoBehaviour
{
    public GameObject textPrefab; // Assign in inspector
    public Transform spawnPoint; // Assign in inspector
    public float moveSpeed = 2f; // Speed at which the text moves upwards
    public float fadeDuration = 1f; // Duration over which the text fades out
    public float displayDuration = 1f; // Duration the text stays fully visible
    public Pegging pegging; // Reference to Pegging script to get score info

    public void SpawnFloatingText(string message)
    {
        GameObject go = Instantiate(textPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        if (tmp != null) tmp.text = message;
        StartCoroutine(MoveAndFade(go));
    }

    private IEnumerator MoveAndFade(GameObject go)
    {
        float elapsed = 0f;
        RectTransform rect = go.GetComponent<RectTransform>();
        Vector2 startPos = rect.anchoredPosition;
        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        Color startColor = tmp.color;

        // Move up and stay fully visible for displayDuration
        while (elapsed < displayDuration)
        {
            rect.anchoredPosition = startPos + Vector2.up * moveSpeed * elapsed * 50f;
            tmp.color = startColor;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Fade out after displayDuration
        float fadeElapsed = 0f;
        while (fadeElapsed < fadeDuration)
        {
            rect.anchoredPosition = startPos + Vector2.up * moveSpeed * (elapsed + fadeElapsed) * 50f;
            tmp.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, fadeElapsed / fadeDuration));
            fadeElapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(go);
    }
}
