using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SeedHandler : MonoBehaviour
{
    public RectTransform imageRectTransform;
    public Image uiImage;
    public TextMeshProUGUI textMeshPro;
    public float gravity = 9.8f;
    private float currentFallSpeed = 0f;
    private bool hasFallen = false;
    public float fallTime = 2f;
    private float fallTimer = 0f;
    public AudioSource audioSource;
    public AudioClip pressESound;
    public AudioClip disappearSound;
    private bool hasPlayedDisappearSound = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !hasFallen)
        {
            if (pressESound != null)
            {
                audioSource.PlayOneShot(pressESound, 0.5f);
            }

            imageRectTransform.gameObject.SetActive(true);

            if (textMeshPro != null)
            {
                textMeshPro.gameObject.SetActive(true);
            }

            if (uiImage != null)
            {
                uiImage.gameObject.SetActive(true);
            }

            imageRectTransform.anchoredPosition = new Vector2(0, 0);
            hasFallen = true;
            fallTimer = 0f;
            hasPlayedDisappearSound = false;
        }

        if (hasFallen)
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

                if (uiImage != null)
                {
                    uiImage.gameObject.SetActive(false);
                }

                if (textMeshPro != null)
                {
                    textMeshPro.gameObject.SetActive(false);
                }
            }
        }
    }
}