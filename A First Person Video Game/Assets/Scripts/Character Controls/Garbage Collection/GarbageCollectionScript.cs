using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarbageCollectionScript : MonoBehaviour
{
    public GameObject grenadePrefab;

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
        interactionTextObject.text = "".ToString();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(firstPersonCam.transform.position, firstPersonCam.transform.forward, out hitInfo, collectableRange))
        {
            if (hitInfo.collider.CompareTag("Destroyable"))
            {
                interactionTextObject.text = "[E] Throw Explosive".ToString();
            }

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

                FindAnyObjectByType<ScoreSystenScript>().addScore(5);

                FindAnyObjectByType<AudioManagerScript>().Play("Collection");
            }

            if (hitInfo.collider.CompareTag("Destroyable") && target != null)
            {
                FindAnyObjectByType<AudioManagerScript>().Play("Throw");

                GameObject grenade = Instantiate(grenadePrefab, hitInfo.transform.position, hitInfo.transform.rotation);
            }

            if (hitInfo.collider.CompareTag("ExitTrigger"))
            {
                FindAnyObjectByType<ScoreSystenScript>().addScore(20);

                //PLAYER MOVES TO NEXT SCENE WHERE THEY ARE TOLD THAT THEY BEAT THE PREVIOUS LEVEL. KIND OGF LIKE THE ASYLUM BITS OF THE EVIL WITHIN.
                FindAnyObjectByType<SceneManagerScript>().Restart();
            }

            if (hitInfo.collider.CompareTag("BombPickUp"))
            {
                FindAnyObjectByType<ScoreSystenScript>().addScore(0);

                FindAnyObjectByType<AudioManagerScript>().Play("Ammo");

                Destroy(hitInfo.collider.transform.gameObject);
            }
        }
    }
}
