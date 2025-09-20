using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ScoreSystenScript : MonoBehaviour
{
    /*
    Title: The Unity Tutorial For Complete Beginners
    Author: Game Maker's Toolkit
    Date: 21 May 2025
    Code version: 1
    Availability: https://www.youtube.com/watch?v=XtQMytORBmM
    */

    public static ScoreSystenScript Score;

    public bool isMenu = false;

    [Header("Values")]
    public int playerScore;

    [Header("UI Elements (Main Menu)")]
    public TextMeshProUGUI menuHighScoreText;


    [Header("UI Elements (HUD)")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScore;
    public GameObject scoreIcon;


    [Header("UI Elements (Intro)")]
    public TextMeshProUGUI introScoreText;
    public GameObject introHighScoreText;

    private void Awake()
    {
        PlayerPrefs.GetInt("highscore", 0);

        if (!isMenu)
        {
            highScore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
            introScoreText.text = "Highscore: " + PlayerPrefs.GetInt("highscore", 0).ToString();
        }

        if (isMenu)
        {
            menuHighScoreText.text = "Highscore: " + PlayerPrefs.GetInt("highscore", 0).ToString();
        }
    }

    private void Start()
    {
        if (!isMenu)
        {
            if (introHighScoreText != null && PlayerPrefs.GetInt("highscore") > 0)
            {
                introHighScoreText.SetActive(true);
            }
            else
            {
                introHighScoreText.SetActive(false);
            }

            scoreText.text = 0.ToString();
        }
    }

    private void Update()
    {
        scoreIcon.transform.localScale = new Vector4(Mathf.PingPong(.5f, .1f) + 1, scoreIcon.transform.localScale.x, scoreIcon.transform.localScale.y, scoreIcon.transform.localScale.z);
    }

    //the below is used for testing within unity - it creates a "button" that allows you to run the function while the game is running - similar to the debug dot log but nor really
    [ContextMenu("increase Score")]
    //"public" in public void means it can now be accessed in other scripts
    public void addScore(int scoreToAdd)//scoreToAdd makes it so that in any other script using this score function, they can add different ammounts, i.e. if you want the player to get 5 points rather than 1 after the 50th pipe?
    {
        playerScore = playerScore + scoreToAdd;
        scoreText.text = playerScore.ToString();

        //highscore using player prefs
        if (playerScore > PlayerPrefs.GetInt("highscore", 0))
        {
            //FindAnyObjectByType<AudioManager>().Play("highScore");
            PlayerPrefs.SetInt("highscore", playerScore);
            highScore.text = playerScore.ToString();
            introScoreText.text = "Highscore: " + playerScore.ToString();
        }
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        ResetHighScore();

        if (!isMenu)
        {
            if (introHighScoreText != null && PlayerPrefs.GetInt("highscore") <= 0)
            {
                introHighScoreText.SetActive(false);
            }
            else
            {
                introHighScoreText.SetActive(true);
            }
        }
    }

    [ContextMenu("ResetScore")]
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("highscore");

        if (!isMenu)
        {
            highScore.text = "0";//this is to have it update automatically rather than when the program is reopened
            introScoreText.text = "HIghscore: " + 0;
        }
        else
        {
            menuHighScoreText.text = "HIghscore: " + 0;
        }
    }
}
