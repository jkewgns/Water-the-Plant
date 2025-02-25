using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimingBar : MonoBehaviour
{
    public RectTransform marker;
    public RectTransform greenZone;
    public GameObject timingBar;
    public Image backgroundImage;
    public float speed = 2.0f;
    private float barWidth;

    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip missSound;

    private Vector3 originalScale;
    private UnityEngine.Color originalColor;
    private UnityEngine.Color initialBackgroundColor;
    private float minGreenSize = 20f;
    private float shrinkAmount = 10f; //110
    public static int successCount = 0;
    public static int maxSuccesses = 4;

    void Start()
    {
        barWidth = GetComponent<RectTransform>().rect.width;
        originalScale = marker.localScale;
        originalColor = marker.GetComponent<Image>().color;

        if (backgroundImage != null)
        {
            initialBackgroundColor = backgroundImage.color;
        }
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1);
        marker.anchoredPosition = new Vector2((t * barWidth) - (barWidth / 2), marker.anchoredPosition.y);

        if (Input.GetMouseButtonDown(0))
        {
            if (IsInGreenZone())
            {
                successCount++;
                Debug.Log("Perfect Timing! Hits: " + successCount);
                audioSource.PlayOneShot(hitSound);
                AnimateMarker(UnityEngine.Color.green);
                UpdateBackgroundColor();

                if (successCount >= maxSuccesses)
                {
                    Debug.Log("Max Successes Reached! Hiding Timing Bar.");
                    SceneManager.LoadScene("Win");
                }
                else
                {
                    RandomizeGreenZone();
                }
            }
            else
            {
                Debug.Log("Missed!");
                audioSource.PlayOneShot(missSound);
                Invoke("LoadLossScene", 0.5f);
            }
        }
    }

    void LoadLossScene()
    {
        Debug.Log("Missed! Loading Loss Scene.");
        SceneManager.LoadScene("Loss");
    }

    void UpdateBackgroundColor()
    {
        if (backgroundImage != null)
        {
            float progress = (float)successCount / maxSuccesses;

            UnityEngine.Color darkOrange = new UnityEngine.Color(1f, 0.5f, 0f, 1f);
            UnityEngine.Color yellow = UnityEngine.Color.yellow;
            UnityEngine.Color brightYellow = new UnityEngine.Color(1f, 1f, 0.5f, 1f);
            UnityEngine.Color green = UnityEngine.Color.green;

            UnityEngine.Color newColor;

            if (progress < 0.33f)
            {
                newColor = UnityEngine.Color.Lerp(initialBackgroundColor, darkOrange, progress / 0.33f);
            }
            else if (progress < 0.66f)
            {
                newColor = UnityEngine.Color.Lerp(darkOrange, yellow, (progress - 0.33f) / 0.33f);
            }
            else if (progress < 0.85f)
            {
                newColor = UnityEngine.Color.Lerp(yellow, brightYellow, (progress - 0.66f) / 0.19f);
            }
            else
            {
                newColor = UnityEngine.Color.Lerp(brightYellow, green, (progress - 0.85f) / 0.15f);
            }

            backgroundImage.color = newColor;
            Debug.Log("Background Updated: " + newColor);
        }
    }


    bool IsInGreenZone()
    {
        float markerLeft = marker.anchoredPosition.x;
        float greenLeft = greenZone.anchoredPosition.x - greenZone.rect.width / 2;
        float greenRight = greenZone.anchoredPosition.x + greenZone.rect.width / 2;

        return markerLeft >= greenLeft && markerLeft <= greenRight;
    }

    void AnimateMarker(UnityEngine.Color newColor)
    {
        marker.GetComponent<Image>().color = newColor;
        marker.localScale = originalScale * 1.5f;
        StartCoroutine(ResetMarker());
    }

    IEnumerator ResetMarker()
    {
        yield return new WaitForSeconds(0.2f);
        float elapsedTime = 0;
        float duration = 0.2f;

        Vector3 startScale = marker.localScale;
        UnityEngine.Color startColor = marker.GetComponent<Image>().color;

        while (elapsedTime < duration)
        {
            marker.localScale = Vector3.Lerp(startScale, originalScale, elapsedTime / duration);
            marker.GetComponent<Image>().color = UnityEngine.Color.Lerp(startColor, originalColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        marker.localScale = originalScale;
        marker.GetComponent<Image>().color = originalColor;
    }

    void RandomizeGreenZone()
    {
        greenZone.anchorMin = new Vector2(0.5f, 0.5f);
        greenZone.anchorMax = new Vector2(0.5f, 0.5f);
        greenZone.pivot = new Vector2(0.5f, 0.5f);

        float randomX = Random.Range(-barWidth / 2 + greenZone.sizeDelta.x / 2, barWidth / 2 - greenZone.sizeDelta.x / 2);
        greenZone.anchoredPosition = new Vector2(randomX, greenZone.anchoredPosition.y);

        float newWidth = Mathf.Max(greenZone.sizeDelta.x - shrinkAmount, minGreenSize);
        greenZone.sizeDelta = new Vector2(newWidth, greenZone.sizeDelta.y);
    }
}
