using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GarbageCollectionScript : MonoBehaviour
{
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

    }

    // Update is called once per frame
    void Update()
    {

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

            if (hitInfo.collider.CompareTag("Destroyable") && target != null)
            {
                FindAnyObjectByType<BombThrowerScript>().ThrowBomb();
            }
            else if (hitInfo.collider == null || target == null)
            {
                FindAnyObjectByType<BombThrowerScript>().ThrowBomb();
            }
        }
    }
}
