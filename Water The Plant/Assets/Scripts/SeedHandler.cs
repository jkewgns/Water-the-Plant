using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SeedHandler : MonoBehaviour
{
    public RectTransform imageRectTransform;
    public Image uiImage;
    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI controllerTextMeshPro;
    public float gravity = 9.8f;
    private float currentFallSpeed = 0f;
    private bool hasFallen = false;
    public float fallTime = 2f;
    private float fallTimer = 0f;
    public AudioSource audioSource;
    public AudioClip pressESound;
    public AudioClip disappearSound;
    private bool hasPlayedDisappearSound = false;

    private void Update()
    {
        // Detect keyboard or gamepad input
        if ((Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)) // A (Xbox) / X (PlayStation)
        {
            StartFalling();
        }

        if (hasFallen)
        {
            ApplyGravity();
        }
    }

    private void StartFalling()
    {
        if (!hasFallen)
        {
            if (pressESound != null)
            {
                audioSource.PlayOneShot(pressESound, 0.5f);
            }

            imageRectTransform.gameObject.SetActive(true);
            textMeshPro?.gameObject.SetActive(true);
            controllerTextMeshPro?.gameObject.SetActive(true);
            uiImage?.gameObject.SetActive(true);

            imageRectTransform.anchoredPosition = new Vector2(0, 0);
            hasFallen = true;
            fallTimer = 0f;
            hasPlayedDisappearSound = false;
        }
    }

    private void ApplyGravity()
    {
        currentFallSpeed += gravity * Time.deltaTime;
        imageRectTransform.anchoredPosition += Vector2.down * currentFallSpeed * Time.deltaTime;
        fallTimer += Time.deltaTime;

        if (fallTimer >= fallTime && !hasPlayedDisappearSound)
        {
            if (disappearSound != null)
            {
                audioSource.PlayOneShot(disappearSound, 0.5f);
            }

            hasPlayedDisappearSound = true;
            imageRectTransform.gameObject.SetActive(false);
            uiImage?.gameObject.SetActive(false);
            textMeshPro?.gameObject.SetActive(false);
            controllerTextMeshPro.gameObject.SetActive(false);
        }
    }
}