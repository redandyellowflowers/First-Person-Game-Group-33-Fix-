using TMPro;
using UnityEditor.PackageManager;
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

    public GameObject levelCompletionScreen;

    [Header("'Collectable' Objects")]
    public GameObject counterTextObject;
    public GameObject[] collectable;

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

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectiveText.gameObject.GetComponent<TextMeshProUGUI>().text = currentObjective;
        counterTextObject.gameObject.GetComponent<TextMeshProUGUI>().text = collectable.Length.ToString();

        if (levelCompletionScreen != null)
        {
            levelCompletionScreen.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.Rotate(Vector3.up * 55 * Time.deltaTime);

        StartTimer();

        collectable = GameObject.FindGameObjectsWithTag("Collectable");
        collectable = GameObject.FindGameObjectsWithTag("Destructable");
        int CollectablesInLevel = collectable.Length;

        objectiveText.gameObject.GetComponent<TextMeshProUGUI>().text = currentObjective;
        counterTextObject.gameObject.GetComponent<TextMeshProUGUI>().text = collectable.Length.ToString();

        /*
        if (CollectablesInLevel <= 0)
        {
        //maybe timer starts again but less for retuurning to exit vehicle.
        }
        */

        if (CollectablesInLevel <= 0)
        {
            player.GetComponent<GarbageCollectionScript>().enabled = false;

            objectiveText.gameObject.GetComponent<TextMeshProUGUI>().text = objectiveUponCompletion;
            counterTextObject.gameObject.GetComponent<TextMeshProUGUI>().text = "--".ToString();

            levelCompletionTrigger.SetActive(true);
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        RaycastHit hitInfo;

        if (!context.performed) return;

        if (Physics.Raycast(player.GetComponent<FirstPersonControllerScript>().firstPersonCam.transform.position,player.GetComponent<FirstPersonControllerScript>().firstPersonCam.transform.forward, out hitInfo, 5))
        {
            TargetScript target = hitInfo.transform.GetComponent<TargetScript>();

            if (hitInfo.collider.CompareTag("ExitTrigger") && levelCompletionTrigger != null)
            {
                if (levelCompletionScreen != null)
                {
                    //PLAYER MOVES TO NEXT SCENE WHERE THEY ARE TOLD THAT THEY BEAT THE PREVIOUS LEVEL. KIND OGF LIKE THE ASYLUM BITS OF THE EVIL WITHIN.

                    //gameObject.GetComponent<PauseMenuScript>().enabled = false;
                    Debug.Log("You WIn !!!");
                }
            }
        }
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
