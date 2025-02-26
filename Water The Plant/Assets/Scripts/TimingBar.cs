using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TimingBar : MonoBehaviour
{
    public RectTransform marker;
    public RectTransform greenZone;
    public GameObject timingBar;
    public Image backgroundImage;
    public Image changingImage;
    public Sprite[] successSprites;

    public float speed = 2.0f;
    private float barWidth;

    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip missSound;

    private Vector3 originalScale;
    private Color originalColor;
    private Color initialBackgroundColor;
    private float minGreenSize = 20f;
    private float shrinkAmount = 10f; //110
    public static int successCount = 0;
    public static int maxSuccesses = 4;

    private bool isOnCooldown = false;
    public float clickCooldown = 1f;

    public static bool currentSuccess = false;

    void Start()
    {
        successCount = 0;
        currentSuccess = false;

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

        if ((Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
        {
            HandleButtonPress();
        }
    }

    void HandleButtonPress()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(ClickCooldown());
            currentSuccess = IsInGreenZone();

            if (currentSuccess)
            {
                successCount++;
                audioSource.PlayOneShot(hitSound);
                AnimateMarker(Color.green);
                UpdateBackgroundColor();
                UpdateChangingImage();

                if (successCount >= maxSuccesses)
                {
                    StartCoroutine(ShowWinScreen());
                }
                else
                {
                    RandomizeGreenZone();
                }
            }
            else
            {
                audioSource.PlayOneShot(missSound);
                Invoke(nameof(LoadLossScene), 0.5f);
            }
        }
    }

    IEnumerator ShowWinScreen()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Win");
    }


    void LoadLossScene()
    {
        SceneManager.LoadScene("Loss");
    }

    IEnumerator ClickCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(clickCooldown);
        isOnCooldown = false;
    }

    bool IsInGreenZone()
    {
        float markerLeft = marker.anchoredPosition.x;
        float greenLeft = greenZone.anchoredPosition.x - greenZone.rect.width / 2;
        float greenRight = greenZone.anchoredPosition.x + greenZone.rect.width / 2;

        return markerLeft >= greenLeft && markerLeft <= greenRight;
    }

    void AnimateMarker(Color newColor)
    {
        marker.GetComponent<Image>().color = newColor;
        marker.localScale = originalScale * 1.5f;
        StartCoroutine(ResetMarker());
    }

    IEnumerator ResetMarker()
    {
        yield return new WaitForSeconds(0.2f);
        marker.localScale = originalScale;
        marker.GetComponent<Image>().color = originalColor;
    }

    void UpdateBackgroundColor()
    {
        if (backgroundImage != null)
        {
            float progress = (float)successCount / maxSuccesses;

            Color darkOrange = new Color(1f, 0.4f, 0f, 1f);
            Color orange = new Color(1f, 0.6f, 0f, 1f);
            Color yellow = new Color(1f, 1f, 0f, 1f);
            Color brightYellow = new Color(1f, 1f, 0.5f, 1f);

            Color newColor;

            if (progress < 0.33f)
                newColor = Color.Lerp(darkOrange, orange, progress / 0.33f);
            else if (progress < 0.66f)
                newColor = Color.Lerp(orange, yellow, (progress - 0.33f) / 0.33f);
            else
                newColor = Color.Lerp(yellow, brightYellow, (progress - 0.66f) / 0.34f);

            backgroundImage.color = newColor;
        }
    }

    void UpdateChangingImage()
    {
        if (changingImage != null && successSprites.Length > 0 && successCount <= successSprites.Length)
        {
            changingImage.sprite = successSprites[successCount - 1];
        }
    }

    void RandomizeGreenZone()
    {
        float newWidth = Mathf.Max(greenZone.sizeDelta.x - shrinkAmount, minGreenSize);
        greenZone.sizeDelta = new Vector2(newWidth, greenZone.sizeDelta.y);

        float randomX = Random.Range(-barWidth / 2 + greenZone.sizeDelta.x / 2, barWidth / 2 - greenZone.sizeDelta.x / 2);
        greenZone.anchoredPosition = new Vector2(randomX, greenZone.anchoredPosition.y);
    }
}
