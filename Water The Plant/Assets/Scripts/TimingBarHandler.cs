using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingBarHandler : MonoBehaviour
{
    public GameObject timingBar;
    public Image requiredImage;

    private bool hasImageBeenEnabled = false;
    private bool hasMissed = false;

    void Start()
    {
        timingBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (requiredImage.gameObject.activeSelf)
        {
            hasImageBeenEnabled = true;
        }

        if (hasImageBeenEnabled && !requiredImage.gameObject.activeSelf && !timingBar.activeSelf && !hasMissed && TimingBar.successCount < TimingBar.maxSuccesses)
        {
            timingBar.SetActive(true);
        }
    }

    public void PlayerMissed()
    {
        hasMissed = true;
        timingBar.SetActive(false);
    }
}
