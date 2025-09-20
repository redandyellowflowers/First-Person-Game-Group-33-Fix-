using UnityEngine;
using UnityEngine.InputSystem;

public class MusicAfterIntroScript : MonoBehaviour
{
    public IntroTextScript textScript;

    private void Awake()
    {
        textScript = gameObject.GetComponentInParent<IntroTextScript>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (textScript.dialogueHasEnded)
        {
            FindAnyObjectByType<AudioManagerScript>().Play("Background");

            Destroy(gameObject);
        }
    }
}
