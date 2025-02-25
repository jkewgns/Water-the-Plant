using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WateringHandler : MonoBehaviour
{
    public Image image;
    public Image targetImage;

    public float displayTime = 2f;
    public float tiltAngle = -15f;
    public float tiltDuration = 0.2f;
    public float targetImageDisplayTime = 3f;
    public float clickCooldown = 1f;

    private bool canShowImage = false;
    private bool isOnCooldown = false;
    private int showCount = 0;
    private int maxShows = 3;

    // Start is called before the first frame update
    void Start()
    {
        if (image != null)
            image.gameObject.SetActive(false);

        if (targetImage != null)
            targetImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (showCount >= maxShows || isOnCooldown) return;

        if (Input.GetKeyDown(KeyCode.E) && targetImage != null)
        {
            ShowTargetImage();
        }

        if (canShowImage && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ClickCooldown());
            ShowImage();
        }
    }

    void ShowTargetImage()
    {
        targetImage.gameObject.SetActive(true);
        StartCoroutine(HideTargetImageAfterSeconds(targetImageDisplayTime));
    }

    IEnumerator HideTargetImageAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        targetImage.gameObject.SetActive(false);

        if (showCount < maxShows)
        {
            Debug.Log("Target Image Disabled -> You can now press LMB to show the watering can.");
            canShowImage = true;
        }
    }

    void ShowImage()
    {
        if (showCount >= maxShows) return;

        if (image != null)
        {
            Debug.Log("LMB Clicked -> Showing Watering Can");
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
}
