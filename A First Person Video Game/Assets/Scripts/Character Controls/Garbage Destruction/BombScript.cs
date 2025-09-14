using UnityEngine;

public class BombScript : MonoBehaviour
{
    public float delay = 0f;
    public float blastRadius = 5f;
    public float explosionForce = 700f;

    //public GameObject explosionEffect;

    float countDown;
    bool hasExploded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countDown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;//time.deltatime is amount of time since last frame was drawn so by minusing from it, you're essentially reducing countdown by 1, thus creating a timer
        if (countDown <= 0f && hasExploded == false)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        //Instantiate(explosionEffect, transform.position, transform.rotation);//spawns the object at grenades position and rotation

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);//unity creates a sphere that checks for everything inside of it

        foreach (Collider nearbyObject in colliders)//once the objects within the blastradius have been confirmed, this transitions into what we want to do with those objects -- looping through all of them to perform the below serach for rgidbodies
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();//the first two, "rigidbody rb" is where the found rigidbodies for the gameobjects will be stored
            if (rb != null)//checks if the game objects have rigid bodies
            {
                rb.AddExplosionForce(explosionForce, transform.position, blastRadius);//1.how big the explosion is, 2. where it will be set of(in this case the grenades current possiton), 3. the blast radius
            }

            TargetScript target = nearbyObject.GetComponent<TargetScript>();//checkinh to see if we hit object with target script

            if (target != null)//meaning that this will only happen when the object being fired at has a target script
            {
                target.takeDamage(100);
            }
        }

        FindAnyObjectByType<AudioManagerScript>().Play("Explosion");

        FindAnyObjectByType<ScoreSystenScript>().addScore(10);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, blastRadius);
    }
}
