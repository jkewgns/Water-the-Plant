using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject firstSelectedButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
            if (selectedButton != null && selectedButton.TryGetComponent<Button>(out Button button))
            {
                button.onClick.Invoke();
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting...");
    }

    public void RestartGame()
    {
        TimingBar.successCount = 0;
        TimingBar.currentSuccess = false;
        SceneManager.LoadScene(0);
    }
}