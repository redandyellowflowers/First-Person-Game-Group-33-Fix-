using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.TextCore.Text;

public class IntroTextScript : MonoBehaviour
{
    public static IntroTextScript introTextScript;

    public bool isLevel = true;
    private GameObject player;

    [Header("Level Initiate")]
    public GameObject introText;
    public GameObject HUD;
    private bool hasInitiatedTimer;

    [Header("dialogue")]
    public float dialogueSpeed;
    [TextArea(2, 4)] public string[] sentences;
    private int index = 0;
    public bool dialogueHasEnded;
    private bool isDoneTalking;
    public GameObject continueButton;

    private void Awake()
    {
        introText.GetComponentInChildren<TextMeshProUGUI>().text = "";

        player = GameObject.FindWithTag("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (introText != null)
        {
            continueButton.SetActive(false);

            introText.SetActive(true);
            hasInitiatedTimer = false;

            if (HUD != null)
            {
                HUD.SetActive(false);
            }

            FindAnyObjectByType<FirstPersonControllerScript>().playerCanMove = false;

            StartCoroutine(WriteSentence());
        }
        else
        {
            hasInitiatedTimer = true;

            if (HUD != null)
            {
                HUD.SetActive(true);
            }

            FindAnyObjectByType<FirstPersonControllerScript>().playerCanMove = true;

            if (isLevel == false)
            {
                FindAnyObjectByType<SceneManagerScript>().NextLevel();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasInitiatedTimer)
        {
            FindAnyObjectByType<ConditionsScript>().StartTimer();
        }
    }

    public void OnInitiate(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isDoneTalking)
        {
            NextSentence();
        }

        if (dialogueHasEnded)
        {
            if (introText != null)
            {
                introText.SetActive(false);
            }

            hasInitiatedTimer = true;

            if (HUD != null)
            {
                HUD.SetActive(true);
            }

            FindAnyObjectByType<FirstPersonControllerScript>().playerCanMove = true;
        }
    }

    IEnumerator WriteSentence()
    {
        foreach (char character in sentences[index].ToCharArray())
        {
            introText.GetComponentInChildren<TextMeshProUGUI>().text += character;

            FindAnyObjectByType<AudioManagerScript>().PlayOnButtonPress("Typing");

            yield return new WaitForSeconds(dialogueSpeed);
            isDoneTalking = false;
            continueButton.SetActive(false);
        }
        isDoneTalking = true;
        continueButton.SetActive(true);
        index++;
    }

    void NextSentence()
    {
        if (index <= sentences.Length - 1)
        {
            introText.GetComponentInChildren<TextMeshProUGUI>().text = "".ToString();
            StartCoroutine(WriteSentence());
        }
        else
        {
            dialogueHasEnded = true;

            introText.GetComponentInChildren<TextMeshProUGUI>().text = "".ToString();
            index = sentences[^1].Length;
        }
    }
}
