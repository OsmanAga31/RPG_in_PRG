using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSampleScript : MonoBehaviour
{
    [SerializeField] private float minScaleX = 0.5f;
    [SerializeField] private float maxScaleX = 1.5f;
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private bool doPause = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1f, .75f, 1f);
        StartCoroutine(BounceScaleOverTime());
    }

    private IEnumerator BounceScaleOverTime()
    {
        while (true)
        {
            float targetScaleX = Random.Range(minScaleX, maxScaleX);
            float scaleSpeed = Random.Range(minSpeed, maxSpeed);

            float startScaleX = transform.localScale.x;
            float elapsedTime = 0f;
            float duration = Mathf.Abs(targetScaleX - startScaleX) / scaleSpeed; // Dauer basierend auf Geschwindigkeit

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // Easing: Verwende eine quadratische Interpolation für sanftes Beschleunigen/Verlangsamen
                t = t * t * (3f - 2f * t);

                float newScaleX = Mathf.Lerp(startScaleX, targetScaleX, t);
                transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
                yield return null;
            }

            // Zielwert sicherstellen
            transform.localScale = new Vector3(targetScaleX, transform.localScale.y, transform.localScale.z);

            if (doPause)
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f)); // Optional: Pause zwischen den Bounces
        }
    }
}
