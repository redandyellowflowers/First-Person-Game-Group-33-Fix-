using UnityEngine;

public class CollectableCoinScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<AudioManagerScript>().Play("Coin Collected");

            FindAnyObjectByType<ScoreSystenScript>().addScore(5);
            Destroy(gameObject);
        }
    }
}
