using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private int health = 100;

    public void takeDamage(int amount)
    {
        health -= amount;


        if (health == 0 && gameObject.tag == "Collectable")
        {
            Destroy(gameObject);
            Debug.Log("SMALL Object Collected!");
        }
        else if (health == 0 && gameObject.tag == "Destroyable")
        {
            Destroy(gameObject);
            Debug.Log("LARGE Object Destroyed!");
        }
        else
        {
            Debug.Log("Nothing.");
        }
    }
}
