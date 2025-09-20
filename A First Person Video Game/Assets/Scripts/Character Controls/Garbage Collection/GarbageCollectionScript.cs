using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarbageCollectionScript : MonoBehaviour
{
    [Header("GameObjects, Transforms, and Text")]
    public Transform spawnPoint;
    public TextMeshProUGUI interactionTextObject;

    [Header("Values")]
    public float collectableRange = 5f;
    public float throwableRange = 10f;
    
    private GameObject firstPersonCam;

    private void Awake()
    {
        firstPersonCam = GameObject.FindWithTag("MainCamera");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionTextObject.text = "".ToString();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(firstPersonCam.transform.position, firstPersonCam.transform.forward, out hitInfo, collectableRange))
        {
            if (hitInfo.collider.CompareTag("Collectable"))
            {
                interactionTextObject.text = "[E] Pick Up".ToString();
            }

            if (hitInfo.collider.CompareTag("ExitTrigger"))
            {
                interactionTextObject.text = "[E] Leave Level".ToString();
            }

            if (hitInfo.collider.CompareTag("Coin"))
            {
                interactionTextObject.text = "Coin".ToString();
            }
        }
        else
        {
            interactionTextObject.text = "".ToString();
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        RaycastHit hitInfo;

        if (!context.performed) return;

        if (Physics.Raycast(firstPersonCam.transform.position, firstPersonCam.transform.forward, out hitInfo, collectableRange))
        {
            TargetScript target = hitInfo.transform.GetComponent<TargetScript>();

            if (hitInfo.collider.CompareTag("Collectable") && target != null)
            {
                target.takeDamage(100);

                FindAnyObjectByType<ScoreSystenScript>().addScore(10);

                FindAnyObjectByType<AudioManagerScript>().Play("Collection");
            }

            if (hitInfo.collider.CompareTag("ExitTrigger"))
            {
                //PLAYER MOVES TO NEXT SCENE WHERE THEY ARE TOLD THAT THEY BEAT THE PREVIOUS LEVEL. KIND OGF LIKE THE ASYLUM BITS OF THE EVIL WITHIN.
                FindAnyObjectByType<SceneManagerScript>().NextLevel();
            }
        }
    }
}
