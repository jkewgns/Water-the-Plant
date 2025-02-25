using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class WateringHandler : MonoBehaviour
{
    public Image image;
    public Image targetImage;

    // UI elements to enable
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public Image extraImage1;
    public Image extraImage2;

    public float displayTime = 2f;
    public float tiltAngle = -15f;
    public float tiltDuration = 0.2f;
    public float targetImageDisplayTime = 3f;
    public float clickCooldown = 1f;

    private bool canShowImage = false;
    private bool isOnCooldown = false;
    private int showCount = 0;
    private int maxShows = 3;

    void Start()
    {
        if (image != null)
            image.gameObject.SetActive(false);

        if (targetImage != null)
            targetImage.gameObject.SetActive(false);

        // Disable UI elements at the start
        if (text1 != null) text1.gameObject.SetActive(false);
        if (text2 != null) text2.gameObject.SetActive(false);
        if (extraImage1 != null) extraImage1.gameObject.SetActive(false);
        if (extraImage2 != null) extraImage2.gameObject.SetActive(false);
    }

    void Update()
    {
        if (showCount >= maxShows || isOnCooldown) return;

        // Show target image when pressing E (Keyboard) or A/X (Gamepad)
        if ((Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)) // A (Xbox) / X (PlayStation)
        {
            if (!canShowImage) // First press shows target image
            {
                ShowTargetImage();
            }
        }

        // Show watering can with LMB (mouse) or A/X (controller)
        if (canShowImage &&
            ((Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))) // A (Xbox) / X (PlayStation)
        {
            StartCoroutine(ClickCooldown());
            ShowImage();
        }
    }

    void ShowTargetImage()
    {
        if (targetImage != null)
        {
            targetImage.gameObject.SetActive(true);
            StartCoroutine(HideTargetImageAfterSeconds(targetImageDisplayTime));
        }
    }

    IEnumerator HideTargetImageAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (targetImage != null)
            targetImage.gameObject.SetActive(false);

        if (showCount < maxShows)
        {
            Debug.Log("Target Image Disabled -> Now you can show the watering can.");
            canShowImage = true;
            EnableUIElements(); // UI elements appear immediately!
        }
    }

    void ShowImage()
    {
        if (showCount >= maxShows || image == null) return;

        Debug.Log("LMB/A/X Pressed -> Showing Watering Can");
        image.gameObject.SetActive(true);
        LeanTween.rotateZ(image.gameObject, tiltAngle, tiltDuration).setEaseOutQuad();
        StartCoroutine(HideAfterSeconds(displayTime));
        showCount++;

        if (showCount >= maxShows)
        {
            Debug.Log("Max limit reached! Disabling watering can.");
            canShowImage = false;
        }
    }

    IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (image != null)
        {
            LeanTween.rotateZ(image.gameObject, 0, tiltDuration).setEaseOutQuad();
            yield return new WaitForSeconds(tiltDuration);
            image.gameObject.SetActive(false);
            Debug.Log("Tilting image hidden");
        }
    }

    IEnumerator ClickCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(clickCooldown);
        isOnCooldown = false;
    }

    void EnableUIElements()
    {
        if (text1 != null) text1.gameObject.SetActive(true);
        if (text2 != null) text2.gameObject.SetActive(true);
        if (extraImage1 != null) extraImage1.gameObject.SetActive(true);
        if (extraImage2 != null) extraImage2.gameObject.SetActive(true);
        Debug.Log("UI elements enabled as soon as canShowImage is true.");
    }
}
