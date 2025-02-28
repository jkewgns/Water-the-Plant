using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class WateringHandler : MonoBehaviour
{
    public Image image;
    public Image targetImage;
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
    private int maxShows = 6;

    void Start()
    {
        if (image != null)
            image.gameObject.SetActive(false);
        if (targetImage != null)
            targetImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (showCount >= maxShows || isOnCooldown) return;

        if ((Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
        {
            if (!canShowImage)
            {
                ShowTargetImage();
            }
        }

        if (canShowImage &&
            ((Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)))
        {
            if (showCount == 0 || TimingBar.currentSuccess)
            {
                StartCoroutine(ClickCooldown());
                ShowImage();
            }
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
            yield return new WaitForSeconds(0.1f);
            canShowImage = true;
            ShowUIElements();
        }
    }

    void ShowImage()
    {
        if (showCount >= maxShows || image == null) return;

        if (showCount > 0 && !TimingBar.currentSuccess)
        {
            return;
        }

        if (!image.gameObject.activeSelf)
        {
            image.gameObject.SetActive(true);
            image.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            LeanTween.rotateZ(image.gameObject, tiltAngle, tiltDuration).setEaseOutQuad();
            StartCoroutine(HideAfterSeconds(displayTime));
            showCount++;

            if (showCount >= maxShows)
            {
                canShowImage = false;
            }
        }
    }

    void ShowUIElements()
    {
        if (text1 != null) text1.gameObject.SetActive(true);
        if (text2 != null) text2.gameObject.SetActive(true);
        if (extraImage1 != null) extraImage1.gameObject.SetActive(true);
        if (extraImage2 != null) extraImage2.gameObject.SetActive(true);
    }

    IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        image.gameObject.SetActive(false);
    }

    IEnumerator ClickCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(clickCooldown);
        isOnCooldown = false;
    }
}
