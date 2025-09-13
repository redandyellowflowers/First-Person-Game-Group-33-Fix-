using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GarbageCollectionScript : MonoBehaviour
{
    public int currentAmmo = 5;
    public TextMeshProUGUI ammoCountText;

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
        ammoCountText.text = currentAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        ammoCountText.text = currentAmmo.ToString();

        if (currentAmmo <= 0)
        {
            currentAmmo = 0;
            ammoCountText.text = "--".ToString();
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
            }
        }
        else if (Physics.Raycast(firstPersonCam.transform.position, firstPersonCam.transform.forward, out hitInfo, throwableRange))
        {
            TargetScript target = hitInfo.transform.GetComponent<TargetScript>();

            if (hitInfo.collider.CompareTag("Destroyable") && target != null && currentAmmo > 0)
            {
                FindAnyObjectByType<BombThrowerScript>().ThrowBomb();
                currentAmmo -= 1;
            }

            /*
            else if (hitInfo.collider == null || target == null && currentAmmo > 0)
            {
                FindAnyObjectByType<BombThrowerScript>().ThrowBomb();
                currentAmmo -= 1;
            }
            */
        }
        
        if (Physics.Raycast(firstPersonCam.transform.position, firstPersonCam.transform.forward, out hitInfo, collectableRange))
        {
            if (hitInfo.collider.CompareTag("ExitTrigger"))
            {
                //PLAYER MOVES TO NEXT SCENE WHERE THEY ARE TOLD THAT THEY BEAT THE PREVIOUS LEVEL. KIND OGF LIKE THE ASYLUM BITS OF THE EVIL WITHIN.
                FindAnyObjectByType<SceneManagerScript>().Restart();
            }
        }
    }
}
