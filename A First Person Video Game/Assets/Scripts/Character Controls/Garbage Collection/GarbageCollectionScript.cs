using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarbageCollectionScript : MonoBehaviour
{
    public int currentAmmo = 5;
    public TextMeshProUGUI ammoCountText;

    public float collectableRange = 5f;
    public float throwableRange = 10f;

    public TextMeshProUGUI interactionTextObject;

    private GameObject firstPersonCam;

    private void Awake()
    {
        firstPersonCam = GameObject.FindWithTag("MainCamera");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ammoCountText.text = currentAmmo.ToString();
        interactionTextObject.text = "".ToString();
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

            if (hitInfo.collider.CompareTag("BombPickUp"))
            {
                interactionTextObject.text = "[E] Ammunition".ToString();
            }
        }
        else
        {
            interactionTextObject.text = "".ToString();
        }

        if (Physics.Raycast(firstPersonCam.transform.position, firstPersonCam.transform.forward, out hitInfo, collectableRange) && hitInfo.collider.CompareTag("Destroyable"))
        {
            interactionTextObject.text = "[E] Throw Explosive".ToString();
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

                FindAnyObjectByType<AudioManagerScript>().Play("Collection");
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

        if (Physics.Raycast(firstPersonCam.transform.position, firstPersonCam.transform.forward, out hitInfo, collectableRange))
        {
            if (hitInfo.collider.CompareTag("BombPickUp"))
            {
                FindAnyObjectByType<AudioManagerScript>().Play("Ammo");

                currentAmmo += 3;
                Destroy(hitInfo.collider.transform.gameObject);
            }
        }
    }
}
