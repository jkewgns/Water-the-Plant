using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SeedHandler : MonoBehaviour
{
    // Reference to the UI Image
    public RectTransform imageRectTransform;

    // Reference to the UI Image (for hiding both image and text)
    public Image uiImage; // Reference to the UI Image to hide it

    // Reference to the TextMeshPro text
    public TextMeshProUGUI textMeshPro; // Make sure to use TextMeshProUGUI for UI text

    // Gravity-like acceleration (m/s^2)
    public float gravity = 9.8f;

    // The current fall speed (starts at 0)
    private float currentFallSpeed = 0f;

    // Whether the effect has been triggered
    private bool hasFallen = false;

    // Time before the image disappears
    public float fallTime = 2f; // seconds before the image disappears

    private float fallTimer = 0f;

    // Audio sources for the sound effects
    public AudioSource audioSource;

    // Sound effects for pressing E and when the seed disappears
    public AudioClip pressESound; // Sound when E is pressed
    public AudioClip disappearSound; // Sound when seed disappears

    // Flag to track if the disappear sound has been played
    private bool hasPlayedDisappearSound = false;

    void Update()
    {
        // Check if the user presses the 'E' key and the effect has not already triggered
        if (Input.GetKeyDown(KeyCode.E) && !hasFallen)
        {
            // Play the sound when the user presses E
            if (pressESound != null)
            {
                audioSource.PlayOneShot(pressESound, 0.5f); // Adjust the volume if needed
            }

            // Make the image visible
            imageRectTransform.gameObject.SetActive(true);

            // Make the TextMeshPro text visible
            if (textMeshPro != null)
            {
                textMeshPro.gameObject.SetActive(true); // Make the text visible when E is pressed
            }

            // Make the UI Image visible
            if (uiImage != null)
            {
                uiImage.gameObject.SetActive(true); // Make the UI Image visible when E is pressed
            }

            // Set the image position to the center of the screen (using the pivot at (0.5, 0.5))
            imageRectTransform.anchoredPosition = new Vector2(0, 0); // Positioned at the center

            // Start the fall effect
            hasFallen = true;

            // Reset the fall timer when the image starts falling
            fallTimer = 0f;

            // Reset the sound flag
            hasPlayedDisappearSound = false;
        }

        // If the image is falling, update its position
        if (hasFallen)
        {
            // Increase fall speed by gravity over time
            currentFallSpeed += gravity * Time.deltaTime;

            // Move the image down (falling)
            imageRectTransform.anchoredPosition += Vector2.down * currentFallSpeed * Time.deltaTime;

            // Update the timer to track how long the image has been falling
            fallTimer += Time.deltaTime;

            // After the specified time (fallTime), disable the image and play disappear sound
            if (fallTimer >= fallTime && !hasPlayedDisappearSound)
            {
                // Play the sound when the seed disappears (only once)
                if (disappearSound != null)
                {
                    audioSource.PlayOneShot(disappearSound, 0.5f); // Adjust the volume if needed
                }

                // Set the flag to prevent the sound from playing again
                hasPlayedDisappearSound = true;

                // Disable the image
                imageRectTransform.gameObject.SetActive(false);

                // Disable the UI Image
                if (uiImage != null)
                {
                    uiImage.gameObject.SetActive(false); // Hide the UI Image
                }

                // Disable the TextMeshPro text
                if (textMeshPro != null)
                {
                    textMeshPro.gameObject.SetActive(false); // Hide the text when the seed disappears
                }
            }
        }
    }
}