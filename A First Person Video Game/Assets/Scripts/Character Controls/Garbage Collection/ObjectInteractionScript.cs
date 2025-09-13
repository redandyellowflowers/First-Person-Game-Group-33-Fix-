using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectInteractionScript : MonoBehaviour
{
    [Header("Small Interactions")]
    public float range = 10f;

    [Header("User Interface")]
    public Image reticle;
    public GameObject interactionTextBorder;
    public TextMeshProUGUI interactionText;

    private GameObject firstPersonCam;

    private void Awake()
    {
        firstPersonCam = GameObject.FindWithTag("MainCamera");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (interactionText != null)
        {
            interactionTextBorder.SetActive(false);
            interactionText.text = " ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(firstPersonCam.GetComponent<Camera>().transform.position, firstPersonCam.GetComponent<Camera>().transform.forward, out hitInfo, range)
            && hitInfo.transform.gameObject.CompareTag("Interactable") && interactionText != null)
        {
            interactionTextBorder.SetActive(true);
            interactionText.text = "[E] Pick Up".ToString();
        }
        else if (interactionText != null)
        {
            interactionTextBorder.SetActive(false);
            interactionText.text = " ";
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        RaycastHit hitInfo;

        if (!context.performed) return;

        if (Physics.Raycast(firstPersonCam.transform.position, firstPersonCam.transform.forward, out hitInfo, range))
        {
            TargetScript target = hitInfo.transform.GetComponent<TargetScript>();

            if (hitInfo.collider.CompareTag("Interactable") && target != null)
            {
                target.takeDamage(100);
            }
        }
    }
}
