using UnityEngine;

public class destroyExplosion : MonoBehaviour
{

    public float timeBeforeDestroy = 2;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, timeBeforeDestroy); 
    }
}
