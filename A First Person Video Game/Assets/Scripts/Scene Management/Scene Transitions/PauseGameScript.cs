using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGameScript : MonoBehaviour
{
    public GameObject pauseScreen;
    private GameObject player;

    private bool gameIsPaused = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseScreen.SetActive(false);
        gameIsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed && gameIsPaused == false && FindAnyObjectByType<IntroTextScript>().dialogueHasEnded)
        {
            PauseGame();
        }
        else if (context.performed && gameIsPaused == true && FindAnyObjectByType<IntroTextScript>().dialogueHasEnded)
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;

        pauseScreen.SetActive(true);

        gameIsPaused = true;

        FindAnyObjectByType<FirstPersonControllerScript>().playerCanMove = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        pauseScreen.SetActive(false);

        gameIsPaused = false;

        FindAnyObjectByType<FirstPersonControllerScript>().playerCanMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
