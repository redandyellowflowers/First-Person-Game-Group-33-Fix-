using UnityEngine;
using UnityEngine.InputSystem;

public class BombThrowerScript : MonoBehaviour
{
    public static BombThrowerScript bombThrower;

    public float throwForce = 40f;
    public GameObject grenadePrefab;
    public GameObject spawnPoint;

    // Update is called once per frame
    void Update()
    {

    }

    public void ThrowBomb()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(spawnPoint.transform.forward * throwForce, ForceMode.VelocityChange);
    }
}
