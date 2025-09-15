using System;
using System.Collections;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConditionsScript : MonoBehaviour
{
    /*
    1.
    Title: Configurable TIMER / STOPWATCH Unity Tutorial
    Author: BMo
    Date: 13 August 2025
    Code version: 1
    Availability: https://www.youtube.com/watch?v=u_n3NEi223E

    2.
    Title: "Sophies Heroic Bloodbath - Assignment 4 - Exam - Playable Game"
    Author: Miguel Marindanise, Fatima Zahraa Bham, Yongama Ntloko 
    Date: 16 August 2025
    Code version: 1
    */

    private GameObject player;

    [Header("Level Initiate")]
    public GameObject introText;
    public GameObject HUD;
    private bool hasInitiatedTimer;

    [Header("'Collectable' Objects")]
    public GameObject counterTextObject;
    public GameObject[] collectable;
    public GameObject[] destroyable;

    [Header("Objective Prompts")]
    public TextMeshProUGUI objectiveText;
    [TextArea(2, 3)] public string currentObjective = "Clear The Area Of The Debris.";
    [TextArea(2, 3)] public string objectiveUponCompletion = "Leave The Area. Return To Your __?.";

    public GameObject levelCompletionTrigger;

    [Header("Timer")]
    public TextMeshProUGUI timerText;
    public float currentTimer;
    public float timerLimit;
    public bool countDown;
    public bool hasLimit;

    [Header("dialogue")]
    public float dialogueSpeed;
    [TextArea(2, 4)] public string[] sentences;
    private int index = 0;
    private bool dialogueHasEnded;
    private bool isDoneTalking;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        levelCompletionTrigger = GameObject.FindWithTag("ExitTrigger");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (introText != null && dialogueHasEnded != null)
        {
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
        }

        objectiveText.gameObject.GetComponent<TextMeshProUGUI>().text = currentObjective;
        counterTextObject.gameObject.GetComponent<TextMeshProUGUI>().text = collectable.Length + destroyable.Length.ToString();

        if (levelCompletionTrigger != null)
        {
            levelCompletionTrigger.SetActive(false);
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
        }
        isDoneTalking = true;
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

            // Update is called once per frame
    void Update()
    {
        if (hasInitiatedTimer)
        {
            StartTimer();
        }

        collectable = GameObject.FindGameObjectsWithTag("Collectable");
        destroyable = GameObject.FindGameObjectsWithTag("Destroyable");

        int CollectablesInLevel = collectable.Length + destroyable.Length;

        objectiveText.gameObject.GetComponent<TextMeshProUGUI>().text = currentObjective;
        counterTextObject.gameObject.GetComponent<TextMeshProUGUI>().text = CollectablesInLevel.ToString();

        /*
        if (CollectablesInLevel <= 0)
        {
        //maybe timer starts again but less for retuurning to exit vehicle.
        }
        */

        if (CollectablesInLevel <= 0)
        {
            FindAnyObjectByType<AudioManagerScript>().Stop("Background");

            //player.GetComponent<GarbageCollectionScript>().enabled = false;
            objectiveText.gameObject.GetComponent<TextMeshProUGUI>().text = objectiveUponCompletion;
            counterTextObject.gameObject.GetComponent<TextMeshProUGUI>().text = "--".ToString();

            levelCompletionTrigger.SetActive(true);
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

    public void OnReset(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        introText.GetComponentInChildren<TextMeshProUGUI>().text = "".ToString();
        index = 0;

        dialogueHasEnded = true;

        hasInitiatedTimer = true;

        if (HUD != null)
        {
            HUD.SetActive(true);
        }

        FindAnyObjectByType<FirstPersonControllerScript>().playerCanMove = true;
    }

    void StartTimer()
    {
        timerText.enabled = true;

        currentTimer = countDown ? currentTimer -= Time.deltaTime : currentTimer += Time.deltaTime;

        if (hasLimit && ((countDown && currentTimer <= timerLimit || (!countDown && currentTimer >= timerLimit))))
        {
            currentTimer = timerLimit;
            timerText.color = Color.red;
            enabled = false;
        }

        timerText.text = currentTimer.ToString("00");

        if (currentTimer == timerLimit)
        {
            FindAnyObjectByType<SceneManagerScript>().Restart();
        }

        if (currentTimer <= 20)
        {
            timerText.color = Color.yellow;
        }

        if (currentTimer <= 10)
        {
            timerText.color = Color.red;
        }
    }
}
